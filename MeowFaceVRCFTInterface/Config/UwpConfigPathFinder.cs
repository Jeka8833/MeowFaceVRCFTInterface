using Microsoft.Extensions.Logging;

namespace MeowFaceVRCFTInterface.Config
{
    public class UwpConfigPathFinder
    {
        public static void PrintConfigPath(ILogger logger, string configPath)
        {
            string? uwpPath = GetUwpPath(configPath);

            string paths = "";
            if (uwpPath == null)
            {
                logger.LogInformation("UWP path not found, are you debugging VRCFT or Windows.SDK.NET version in VRCFT changed?");
            }
            else if (File.Exists(uwpPath))
            {
                paths = uwpPath;
            }

            if (File.Exists(configPath))
            {
                if (paths.Length != 0) paths += "or\n";
                paths += $"\n{configPath}";
            }

            if (paths.Length == 0)
            {
                logger.LogWarning("The Meow Configuration file is not found");
            }
            else
            {
                logger.LogInformation("The Meow Configuration file is located in: {}", paths);
            }
        }

        private static string? GetUwpPath(string originalPath)
        {
            // If the Windows.SDK.NET version does not match, an Exception will be thrown.
            try
            {
                string regularRoamingFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                string? substractedPathN = SubstractPath(originalPath, regularRoamingFolder);
                if (substractedPathN is string substractedPath)
                {
                    string uwpLocalFolder = Windows.Storage.ApplicationData.Current.LocalCacheFolder.Path;

                    return Path.Combine(uwpLocalFolder, "Roaming", substractedPath);
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        private static string? SubstractPath(string fullPath, string partPath)
        {
            if (fullPath.StartsWith(partPath, StringComparison.CurrentCultureIgnoreCase))
            {
                return fullPath[partPath.Length..].TrimStart(Path.DirectorySeparatorChar);
            }

            return null;
        }
    }
}
