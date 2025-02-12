using Microsoft.Extensions.Logging;

namespace MeowFaceVRCFTInterface.Core.Config.Migration;

public class MigrationManager
{
    private readonly Version? _moduleVersion;
    private readonly ILogger _logger;

    public MigrationManager(Version? moduleVersion, ILogger logger)
    {
        _moduleVersion = moduleVersion;
        _logger = logger;

        if (_moduleVersion == null)
        {
            _logger.LogDebug("Are you know that module version is null?");
        }
    }

    public void TryMigrate(MeowConfig config)
    {
        if (_moduleVersion == null) return;

        try
        {
            Version configVersion = new(config.ConfigVersion);
            if (configVersion == _moduleVersion) return;

            try
            {
                Migrate(config, configVersion);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Failed to migrate configuration");
            }
        }
        catch (Exception e)
        {
            _logger.LogDebug(e, "Failed to parse version, I think user is too smart");
        }
    }

    public void UpdateConfigVersion(MeowConfig config)
    {
        if (_moduleVersion == null) return;

        int build = _moduleVersion.Build == -1 ? 0 : _moduleVersion.Build;
        int revision = _moduleVersion.Revision == -1 ? 0 : _moduleVersion.Revision;

        config.ConfigVersion = new Version(_moduleVersion.Major, _moduleVersion.Minor, build, revision).ToString();
    }

    private void Migrate(MeowConfig config, Version configVersion)
    {
        /* Example migration
        if (_moduleVersion > configVersion)
        {
            config.EyeMapper.EyeGazeX = false;
        }*/
    }
}