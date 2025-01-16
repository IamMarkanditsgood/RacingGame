using System.IO;
using UnityEngine;

public class JsonStorage
{
    public void SaveToJson<T>(string key, T data)
    {
        string filePath = GetFilePath(key);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    public T LoadFromJson<T>(string key)
    {
        string filePath = GetFilePath(key);

        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"File {filePath} has not been found");
            return default;
        }

        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<T>(json);
    }

    public bool Exists(string key)
    {
        return File.Exists(GetFilePath(key));
    }

    private string GetFilePath(string key)
    {

        return Path.Combine(Application.persistentDataPath, $"{key}.json");
    }
}
