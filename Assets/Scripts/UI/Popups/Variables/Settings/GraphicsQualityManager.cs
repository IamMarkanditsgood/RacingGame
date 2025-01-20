using System;
using TMPro;
using UnityEngine;

[Serializable]
public class GraphicsQualityManager
{
    [SerializeField] private TMP_Dropdown qualityDropdown;

    public void Init()
    {
        if (qualityDropdown != null)
        {
            SetDropdown();
            Subscribe();
        }
    }
    
    private void Subscribe()
    {
        qualityDropdown.onValueChanged.AddListener(SetQuality);
    }

    public void UnSubscribe()
    {
        qualityDropdown.onValueChanged.RemoveListener(SetQuality);
    }

    private void SetDropdown()
    {
        int savedQuality = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.QualityKey, QualitySettings.GetQualityLevel());

        PopulateDropdown();
        qualityDropdown.value = savedQuality; 
    }

    private void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        Debug.Log($"Graphics quality set to: {QualitySettings.names[index]}");

        SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.QualityKey, index);
    }

    private void PopulateDropdown()
    {
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(QualitySettings.names));
    }
}
