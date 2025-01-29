using System;
using System.IO;
using System.Collections.Generic;

namespace Data
{
    public static class Config
    {
        private static readonly HashSet<string> loadedEnvVariables = new HashSet<string>();

        static Config()
        {
            LoadEnv(".env"); // Cargar automáticamente al inicializar
        }

        private static void LoadEnv(string filePath)
        {
            if (!File.Exists(filePath)) return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                if (!string.IsNullOrWhiteSpace(line) && line.Contains('=') && !line.StartsWith("#"))
                {
                    var parts = line.Split('=', 2);
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();

                    Environment.SetEnvironmentVariable(key, value);
                    loadedEnvVariables.Add(key);
                }
            }
        }

        public static string Get(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}