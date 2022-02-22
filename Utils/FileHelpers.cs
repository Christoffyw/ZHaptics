using System.IO;

namespace ZHaptics
{
    public class FileHelpers
    {
        public static string RootDirectory => Directory.GetCurrentDirectory() + @"\BepInEx\plugins\ZHaptics";

        public static void EnforceDirectory()
        {
            Directory.CreateDirectory(RootDirectory);
        }
    }
}