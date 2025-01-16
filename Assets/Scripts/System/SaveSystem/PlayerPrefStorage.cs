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
