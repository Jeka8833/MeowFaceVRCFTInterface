using Microsoft.Extensions.Logging;

namespace MeowFaceVRCFTInterface.Config;

public static class UwpConfigPathFinder
{
    public static void PrintConfigPath(ILogger logger, string configPath)
    {
        if (!File.Exists(configPath))
        {
            logger.LogWarning("The Meow Configuration file is not found");

            return;
        }

        string? uwpPath = GetUwpPath(configPath);

        if (uwpPath == null)
        {
            logger.LogInformation(
                "UWP path not found, are you debugging VRCFT or Windows.SDK.NET version in VRCFT changed?\n" +
                "Maybe Meow Configuration file located in: {}", configPath);
        }
        else
        {
            logger.LogInformation("The Meow Configuration file is located in: {}", uwpPath);
        }
    }

    private static string? GetUwpPath(string originalPath)
    {
        // If the Windows.SDK.NET version does not match, an Exception will be thrown.
        try
        {
            string regularRoamingFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string? subtractedPath = SubtractPath(originalPath, regularRoamingFolder);
            if (subtractedPath != null)
            {
                string uwpLocalFolder = Windows.Storage.ApplicationData.Current.LocalCacheFolder.Path;

                return Path.Combine(uwpLocalFolder, "Roaming", subtractedPath);
            }
        }
        catch (Exception)
        {
            // ignored
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