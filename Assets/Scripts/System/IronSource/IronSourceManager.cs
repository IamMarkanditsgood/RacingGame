using UnityEngine;

public class IronSourceManager : MonoBehaviour
{
    private void Start()
    {
        IronSource.Agent.init("20c5b769d", IronSourceAdUnits.REWARDED_VIDEO);
    }

    private void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }
}