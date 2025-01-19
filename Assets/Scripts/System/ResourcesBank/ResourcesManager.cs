using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager Instance { get; private set; }

    public event Action<ResourceTypes, int> OnResourceModified;

    private Dictionary<ResourceTypes, int> _resources = new Dictionary<ResourceTypes, int>();

    public void Init()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        InitResourceDictionary(); 
    }

    public int GetResource(ResourceTypes resource)
    {
        return _resources[resource];
    }

    public void ModifyResource(ResourceTypes resource, int updateAmount)
    {
        _resources[resource] += updateAmount;
        SaveManager.Resources.SaveResource(resource, _resources[resource]);

        OnResourceModified?.Invoke(resource, _resources[resource]);
    }

    public bool IsEnoughResource(ResourceTypes resource, int price)
    {
        if (_resources[resource] < price)
        {
            return false;
        }

        return true;
    }

    private void InitResourceDictionary()
    {
        _resources[ResourceTypes.Coins] = SaveManager.Resources.LoadResource(ResourceTypes.Coins);
        _resources[ResourceTypes.TotalPoints] = SaveManager.Resources.LoadResource(ResourceTypes.TotalPoints);
    }
}