using System.Text.Json;
using UnityEngine;

namespace CustomKeybindApi
{
    public static class KeybindHandler
    {
        private static readonly string _filePath = Path.Combine(Application.persistentDataPath, "DEMO_CustomKeybinds.json");
        private static Dictionary<string, Dictionary<string, object>> _data = [];

        public static void SaveKeybind(string category, string key, object value)
        {
            if(!_data.ContainsKey(category))
            {
                _data[category] = [];
            }

            _data[category][key] = value;
            SaveToFile();
        }

        public static void RemoveKeybind(string category, string key)
        {
            if(_data.ContainsKey(category) && _data[category].ContainsKey(key))
            {
                _data[category].Remove(key);
                SaveToFile();
            }
        }

        public static string GetKeybind(string category, string key, string defaultValue = "")
        {
            LoadFromFile();

            if(_data.TryGetValue(category, out var categoryDict))
            {
                if(categoryDict.TryGetValue(key, out var value))
                {
                    return value?.ToString() ?? defaultValue;
                }
            }
            return defaultValue;
        }

        private static void SaveToFile()
        {
            if(File.Exists(_filePath))
            {
                return;
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(_data, options);
            File.WriteAllText(_filePath, jsonString);
        }

        private static void LoadFromFile()
        {
            if(File.Exists(_filePath))
            {
                try
                {
                    string jsonString = File.ReadAllText(_filePath);
                    var loadedData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(jsonString);

                    if(loadedData != null)
                    {
                        _data = loadedData;
                    }
                }
                catch (Exception)
                {
                    _data = [];
                }
            }
            else
            {
                _data = [];
            }
        }
    }
}