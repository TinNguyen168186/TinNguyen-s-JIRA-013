using System.IO;
using System.Reflection;

namespace Core.Extensions
{
    public static class StringExtension
    {
        public static string GetAbsolutePath(this string filePath)
        {
            var baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            string directoryPath = Path.Combine(baseDirectory, filePath);

            if (File.Exists(directoryPath))
            {
                return directoryPath;
            }
            return string.Empty;
        }

        public static string GetTextFromJsonFile(this string filePath)
        {
            string pathFile = filePath.GetAbsolutePath();
            return File.ReadAllText(pathFile);
        }
    }
}