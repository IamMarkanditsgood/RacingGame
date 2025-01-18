using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPrefStorage
{
    public void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public int LoadInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }
    public void SaveString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    public string LoadString(string key, string defaultValue = "")
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }
    public void SaveStringList(string key, List<string> list)
    {
        string serializedList = string.Join("|", list);
        PlayerPrefs.SetString(key, serializedList);
        PlayerPrefs.Save();
    }

    public List<string> LoadStringList(string key)
    {
        string serializedList = PlayerPrefs.GetString(key, "");
        if (string.IsNullOrEmpty(serializedList))
        {
            return new List<string>();
        }

        return new List<string>(serializedList.Split('|'));
    }
    public void SaveEnumList<T>(string key, List<T> list)
    {
        string serializedList = string.Join(",", list.Select(a => a.ToString()).ToArray());
        PlayerPrefs.SetString(key, serializedList);
        PlayerPrefs.Save();
        Debug.Log(key);
    }

    public T LoadEnum<T>(string key, T defaultValue) where T : struct, Enum
    {
        string savedValue = PlayerPrefs.GetString(key, defaultValue.ToString());
        if (Enum.TryParse(typeof(T), savedValue, out var result))
        {
            return (T)result;
        }

        Debug.LogWarning($"Failed to load enum of type {typeof(T)} from key '{key}'. Returning default value.");
        return defaultValue;
    }

    public void SaveEnum<T>(string key, T value) where T : Enum
    {
        PlayerPrefs.SetString(key, value.ToString());
        PlayerPrefs.Save();
    }

    public List<T> LoadEnumList<T>(string key)
    {
        string serializedList = PlayerPrefs.GetString(key, string.Empty);
        if (string.IsNullOrEmpty(serializedList))
        {
            return new List<T>();
        }

        return serializedList.Split(',')
                             .Select(s => (T)System.Enum.Parse(typeof(T), s))
                             .ToList();
    }

    public bool IsSaved(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return true;
        }
        return false;
    }

    public void ResetSaves()
    {
        PlayerPrefs.DeleteAll();
    }
}
