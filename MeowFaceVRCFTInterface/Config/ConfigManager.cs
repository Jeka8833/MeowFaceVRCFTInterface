using System.Text;
using MeowFaceVRCFTInterface.Config.Migration;
using MeowFaceVRCFTInterface.VRCFT.Mappers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MeowFaceVRCFTInterface.Config;

// Class looks like s***, need to simplify
public class ConfigManager
{
    private readonly MigrationManager _migrationManager;
    private readonly UwpConfigPathFinder _uwpConfigPathFinder;
    private readonly string _configPath;
    private readonly MeowFaceVRCFTInterface _module;
    private readonly ILogger _logger;

    private readonly object _saveLock = new();

    public MeowConfig Config { get; private set; } = null!;
    public MapperCft[] Mappers { get; private set; } = null!;

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
        MeowConfig config = ReadConfig() ?? new MeowConfig();

        _migrationManager.TryMigrate(config);

        ChangeConfigAndDisableBrokenMappers(config);

        SaveConfigAsync();
    }

    public void LoadConfig()
    {
        try
        {
            if (File.Exists(_configPath))
            {
                MeowConfig? data = JsonConvert.DeserializeObject<MeowConfig>(File.ReadAllText(_configPath));
                if (data != null)
                {
                    ChangeConfigAndDisableBrokenMappers(data);

                    _logger.LogInformation("Config loaded");
                }
                else
                {
                    _logger.LogWarning("Failed to load configuration, data equals null.\n" +
                                       "All configuration data from MeowConfig will be deleted, sorry");
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Failed to load configuration.\n" +
                                  "All configuration data from MeowConfig will be deleted, sorry");
        }
    }

    public void SaveConfigAsync()
    {
        Thread thread = new(() => // Parent thread can be Background, don't kill this thread when app is closing
        {
            // Ideally, you should have forbidden saving if the operation happens too often. But I'm too lazy to do it.
            if (Monitor.TryEnter(_saveLock)) // User is too fast want to save config, ignore it
            {
                try
                {
                    TryCreateConfigFolder();

                    _migrationManager.UpdateConfigVersion(Config);

                    string configJson = JsonConvert.SerializeObject(Config, Formatting.Indented);
                    File.WriteAllText(_configPath, configJson, Encoding.UTF8);

                    _logger.LogDebug("Config saved");

                    _uwpConfigPathFinder.PrintConfigLocationOnce();
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "Failed to save config");
                }
                finally
                {
                    Monitor.Exit(_saveLock);
                }
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
                MeowConfig? data = JsonConvert.DeserializeObject<MeowConfig>(File.ReadAllText(_configPath));
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

    private void ChangeConfigAndDisableBrokenMappers(MeowConfig config)
    {
        MapperCft[] newList = config.GetAllMappers();

        foreach (MapperCft mapper in newList)
        {
            if (!mapper.IsEnabled) continue;

            try
            {
                mapper.Initialize(_module);
            }
            catch (Exception e)
            {
                mapper.IsMapperCrashed = true;

                _logger.LogWarning(e,
                    "Failed to initialize mapper: {}, Disabling it...", mapper.GetType().Name);
            }
        }

        Config = config;
        Mappers = newList;
    }

    private void TryCreateConfigFolder()
    {
        try
        {
            string? directory = Path.GetDirectoryName(_configPath);
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