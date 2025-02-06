using System.Text;
using MeowFaceVRCFTInterface.VRCFTMappers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MeowFaceVRCFTInterface.Config;

public class ConfigManager
{
    private readonly string _configPath;
    private readonly MeowFaceVRCFTInterface _module;
    private readonly ILogger _logger;

    public MeowConfig Config { get; private set; } = null!;
    public MapperCft[] Mappers { get; private set; } = null!;

    public ConfigManager(string configPath, MeowFaceVRCFTInterface module, ILogger logger)
    {
        _configPath = configPath;
        _module = module;
        _logger = logger;

        InitializeConfigAndDisableBrokenMappers(new MeowConfig());
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
                    InitializeConfigAndDisableBrokenMappers(data);

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

        SaveConfig(); // Always save the config to ensure that user see latest changes
    }

    public void SaveConfig()
    {
        try
        {
            try
            {
                string? directory = Path.GetDirectoryName(_configPath);
                if (!string.IsNullOrEmpty(directory))
                {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(directory);

                    _logger.LogDebug("Directory for config created {}", directoryInfo);
                }
                else
                {
                    _logger.LogWarning("Are you trying to create a config in the root directory?");
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Failed to create directory for config");
            }

            string configJson = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(_configPath, configJson, Encoding.UTF8);

            _logger.LogDebug("Config saved");
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Failed to save config");
        }
    }

    private void InitializeConfigAndDisableBrokenMappers(MeowConfig config)
    {
        MapperCft[] newList = GetMappers(config);

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

    private static MapperCft[] GetMappers(MeowConfig config)
    {
        return new MapperCft[]
        {
            config.EyeMapper, config.BrowMapper, config.CheekMapper, config.JawMapper, config.LipAndMouthMapper,
            config.NoseMapper, config.TongueMapper
        };
    }
}