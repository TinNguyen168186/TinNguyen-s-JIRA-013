using System;
using System.IO;

using Microsoft.Extensions.Configuration;

namespace Core.Utilities
{
    public static class ConfigurationUtils
    {
        private static IConfiguration? _config;

        public static IConfiguration ReadConfiguration(string path)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);
            Console.WriteLine($"Loading config from: {fullPath}");

            if (!File.Exists(fullPath))
            {
                Console.WriteLine("File NOT FOUND!");
            }

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path, optional: false, reloadOnChange: true)
                .Build();

            _config = config;
            return config;
        }
        public static string GetConfigurationByKey(string key, IConfiguration? config = null)
        {
            if (_config == null && config == null)
                throw new InvalidOperationException("Configuration has not been initialized. Call ReadConfiguration first.");

            var value = config == null ? _config?[key] : config[key];

            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            throw new InvalidDataException($"Attribute [{key}] is not set in appsettings.json.");
        }
    }
}
