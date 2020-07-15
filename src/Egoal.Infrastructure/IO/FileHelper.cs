using System.IO;

namespace Egoal.IO
{
    public static class FileHelper
    {
        public static void DeleteIfExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
