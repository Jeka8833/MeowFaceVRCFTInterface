using System.Reflection;

namespace MeowFaceVRCFTInterface.Config.Migration;

public static class ModuleVersion
{
    public static readonly Version? FileVersion = GetFileVersion();

    private static Version? GetFileVersion()
    {
        try
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return assembly.GetName().Version;
        }
        catch (Exception)
        {
            return null;
        }
    }
}