using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Utilities
{
    public static class JsonUtils
    {
        public static List<T> GetTestData<T>(string fileName, string dataSection)
        {
            var jsonContent = File.ReadAllText(fileName);
            var jsonObject = JObject.Parse(jsonContent);

            var dataArray = jsonObject[dataSection];
            if (dataArray == null)
            {
                throw new ArgumentException($"Section '{dataSection}' not found in file '{fileName}'");
            }

            return dataArray.ToObject<List<T>>()!;
        }

        public static string ReadJsonFile(string path)
        {
            if (!File.Exists(path))
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), path);

                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"Cannot find JSON file at path: {path}");
                }
            }

            return File.ReadAllText(path);
        }

        public static T ReadAndParse<T>(string path) where T : class
        {
            var jsonContent = ReadJsonFile(path);
            var result = JsonConvert.DeserializeObject<T>(jsonContent);

            if (result == null)
            {
                throw new InvalidOperationException($"Failed to deserialize JSON from file: {path}");
            }

            return result;
        }

    }
}