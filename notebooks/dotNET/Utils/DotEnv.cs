using System;
using System.Collections.Generic;
using System.IO;


public static class DotEnv
{
    private static Dictionary<string, string> envVariables;

    static DotEnv()
    {
        envVariables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        LoadEnvFile();
    }

    private static void LoadEnvFile()
    {
        string envFilePath = FindEnvFile();
        if (envFilePath != null)
        {
            string[] lines = File.ReadAllLines(envFilePath);
            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                {
                    int equalsIndex = line.IndexOf('=');
                    if (equalsIndex > 0)
                    {
                        string key = line.Substring(0, equalsIndex).Trim();
                        string value = line.Substring(equalsIndex + 1).Trim();
                        envVariables[key] = value;
                    }
                }
            }
        }
    }

    private static string FindEnvFile()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        while (currentDirectory != null)
        {
            string envFilePath = Path.Combine(currentDirectory, ".env");
            if (File.Exists(envFilePath))
            {
                return envFilePath;
            }
            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        }
        return null;
    }

    public static string Get(string key)
    {
        if (envVariables.ContainsKey(key))
        {
            return envVariables[key];
        }
        return null;
    }
}