using Microsoft.Extensions.Logging;

namespace MeowFaceVRCFTInterface.Core.Config;

public class UwpConfigPathFinder
{
    public string? UwpConfigPath { get; }

    private readonly string _configPath;
    private readonly ILogger _logger;

    private bool _isPrinted;

    public UwpConfigPathFinder(string configPath, ILogger logger)
    {
        _configPath = configPath;
        _logger = logger;

        UwpConfigPath = GetUwpPath();
    }

    public void PrintConfigLocationOnce()
    {
        if (_isPrinted) return;
        _isPrinted = true;

        PrintLocation();
    }

    private void PrintLocation()
    {
        if (!File.Exists(_configPath))
        {
            _logger.LogWarning("The Meow Configuration file is not found, try to find in: {}", _configPath);

            return;
        }

        if (UwpConfigPath == null)
        {
            _logger.LogInformation(
                "UWP path not found, are you debugging VRCFT or Windows.SDK.NET version in VRCFT changed?\n" +
                "Maybe Meow Configuration file located in: {}", _configPath);
        }
        else
        {
            _logger.LogInformation("The Meow Configuration file is located in: {}", UwpConfigPath);
        }
    }

    private string? GetUwpPath()
    {
        try
        {
            string regularRoamingFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string? subtractedPath = SubtractPath(_configPath, regularRoamingFolder);
            if (subtractedPath != null)
            {
                string uwpLocalFolder = Windows.Storage.ApplicationData.Current.LocalCacheFolder.Path;

                return Path.Combine(uwpLocalFolder, "Roaming", subtractedPath);
            }
        }
        catch (Exception e) // If the Windows.SDK.NET version does not match, an Exception will be thrown.
        {
            _logger.LogDebug(e, "Get UWP Location fail");
        }

        return null;
    }

    private static string? SubtractPath(string fullPath, string partPath)
    {
        if (fullPath.StartsWith(partPath, StringComparison.CurrentCultureIgnoreCase))
        {
            return fullPath[partPath.Length..].TrimStart(Path.DirectorySeparatorChar);
        }

        return null;
    }
}