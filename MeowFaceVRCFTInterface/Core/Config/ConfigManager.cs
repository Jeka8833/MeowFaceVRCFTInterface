using System.Text;
using MeowFaceVRCFTInterface.Core.Config.Converter;
using MeowFaceVRCFTInterface.Core.Config.Migration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MeowFaceVRCFTInterface.Core.Config;

public class ConfigManager
{
    private readonly MigrationManager _migrationManager;
    private readonly UwpConfigPathFinder _uwpConfigPathFinder;
    private readonly string _configPath;
    private readonly MeowFaceVRCFTInterface _module;
    private readonly ILogger _logger;

    private readonly object _fileAccessLock = new();
    private readonly JsonConverter[] _jsonConverters = { new OnlyFinityNumbersConverter() };

    private int _lastWriteHash;

    public MeowConfig Config { get; private set; } = null!;
    public MapperBase[] Mappers { get; private set; } = null!;

    public ConfigManager(string configPath, MeowFaceVRCFTInterface module, ILogger logger)
    {
        _migrationManager = new MigrationManager(ModuleVersion.FileVersion, logger);
        _uwpConfigPathFinder = new UwpConfigPathFinder(configPath, logger);

        _configPath = configPath;
        _module = module;
        _logger = logger;

        ChangeConfigAndDisableBrokenMappers(new MeowConfig());
    }

    public void LoadAndMigrateConfig()
    {
        MeowConfig? config = ReadConfig();
        if (config != null)
        {
            _migrationManager.TryMigrate(config);
            ChangeConfigAndDisableBrokenMappers(config);

            _logger.LogInformation("Config loaded");
        }

        SaveConfigAsync();
    }

    public void LoadConfig()
    {
        MeowConfig? config = ReadConfig();
        if (config == null) return;

        ChangeConfigAndDisableBrokenMappers(config);

        _logger.LogInformation("Config loaded");

        SaveConfigAsync();
    }

    public void SaveConfigAsync()
    {
        // May hang operating system if someone spam SaveConfigAsync
        Thread thread = new(() => // Parent thread can be Background, don't kill this thread when app is closing
        {
            try
            {
                _migrationManager.UpdateConfigVersion(Config);
                string configJson = JsonConvert.SerializeObject(Config, Formatting.Indented, _jsonConverters);

                bool saved = false;
                if (_uwpConfigPathFinder.UwpConfigPath != null)
                {
                    try
                    {
                        WriteConfig(configJson, _uwpConfigPathFinder.UwpConfigPath);
                        saved = true;
                    }
                    catch (Exception e)
                    {
                        _logger.LogDebug(e, "First try to write config has failed");
                    }
                }

                if (!saved)
                {
                    WriteConfig(configJson, _configPath);
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Failed to save config");
            }
        });

        thread.Priority = ThreadPriority.Lowest;
        thread.Start();
    }

    private MeowConfig? ReadConfig()
    {
        try
        {
            if (File.Exists(_configPath))
            {
                string jsonText;
                lock (_fileAccessLock)
                {
                    jsonText = File.ReadAllText(_configPath, Encoding.UTF8);

                    _lastWriteHash = jsonText.GetHashCode();
                }

                MeowConfig? data = JsonConvert.DeserializeObject<MeowConfig>(jsonText, _jsonConverters);
                if (data == null)
                {
                    _logger.LogWarning("Failed to load configuration, data equals null.\n" +
                                       "All configuration data from MeowConfig will be deleted, sorry");
                }

                return data;
            }
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Failed to load configuration.\n" +
                                  "All configuration data from MeowConfig will be deleted, sorry");
        }

        return null;
    }

    /// <exception cref="Exception"></exception>
    private void WriteConfig(string configJson, string configPath)
    {
        _uwpConfigPathFinder.PrintConfigLocationOnce();

        TryCreateConfigFolder(configPath);

        lock (_fileAccessLock)
        {
            int stringHashCode = configJson.GetHashCode();
            if (stringHashCode == _lastWriteHash) return; // Can be collision

            File.WriteAllText(configPath, configJson, Encoding.UTF8);

            _lastWriteHash = stringHashCode;
        }

        _logger.LogDebug("Config saved");
    }

    private void ChangeConfigAndDisableBrokenMappers(MeowConfig config)
    {
        MapperBase[] newList = config.GetAllMappers();

        foreach (MapperBase mapper in newList)
        {
            if (!mapper.IsEnabled) continue;

            try
            {
                mapper.Initialize(_module);
            }
            catch (Exception e)
            {
                mapper.IsMapperCrashed = true;

                _logger.LogWarning("Failed to initialize mapper: {}, Disabling it...", mapper.GetType().Name);
                _logger.LogDebug(e, "Additional StackTrace");
            }
        }

        MapperBase[] oldMappers = Mappers;

        Config = config;
        Mappers = newList;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (oldMappers == null) return;

        foreach (MapperBase mapper in oldMappers)
        {
            try
            {
                mapper.Dispose();
            }
            catch (Exception)
            {
                _logger.LogWarning("Failed to dispose mapper: {}", mapper.GetType().Name);
            }
        }
    }

    private void TryCreateConfigFolder(string configPath)
    {
        try
        {
            string? directory = Path.GetDirectoryName(configPath);
            if (string.IsNullOrEmpty(directory))
            {
                _logger.LogWarning("Are you trying to create a config in the root directory?");
            }
            else
            {
                Directory.CreateDirectory(directory);
            }
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Failed to create directory for config");
        }
    }
}