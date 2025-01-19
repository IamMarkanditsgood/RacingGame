using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceManager : MonoBehaviour
{
    private void Start()
    {
        IronSource.Agent.init("20c5b769d", IronSourceAdUnits.REWARDED_VIDEO);
    }
    private void OnEnable()
    {
        
    }
    private void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }
}
