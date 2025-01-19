using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPopup : BasicPopup
{
    [SerializeField] private GameObject _leavingLoader;
    [SerializeField] private Button _mainMenu;
    [SerializeField] private Button _getReward;

    private int points;

    public override void ResetPopup()
    {
    }

    public override void SetPopup()
    {
    }

    public override void Subscribe()
    {
        base.Subscribe();

        IronSource.Agent.init("20c5b769d", IronSourceAdUnits.REWARDED_VIDEO);

        _mainMenu.onClick.AddListener(MainMenu);
        _getReward.onClick.AddListener(GetReward);

        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
    }
    public override void Unsubscribe()
    {
        base.Unsubscribe(); 

        _mainMenu.onClick.RemoveListener(MainMenu);
        _getReward.onClick.RemoveListener(GetReward);

        IronSourceRewardedVideoEvents.onAdAvailableEvent -= RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent -= RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;
    }

    public void Init(int winedPoints)
    {
        points = winedPoints;
    }

    private void MainMenu()
    {
        if (PhotonNetwork.NetworkClientState != ClientState.Joined)
        {
            Debug.LogWarning($"Cannot leave room. Client is in state: {PhotonNetwork.NetworkClientState}");
            return;
        }
        if (PhotonNetwork.IsMasterClient)
        {
            HandleMasterClientLeave();
        }

        Time.timeScale = 1f;
        PhotonNetwork.LeaveRoom();
        _leavingLoader.SetActive(true);
    }

    private void HandleMasterClientLeave()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (!player.IsMasterClient)
                {
                    PhotonNetwork.SetMasterClient(player);
                    break;
                }
            }
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Successfully left the room. Loading MainMenu scene...");
        AddReward();
        SceneManager.LoadScene("MainMenu");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("MasterClient switched to: " + newMasterClient.NickName);
    }

    private void GetReward()
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            Debug.Log("Rewarded video is not available");
        }
    }

    private void AddReward()
    {
        ResourcesManager.Instance.ModifyResource(ResourceTypes.Coins, points);
        ResourcesManager.Instance.ModifyResource(ResourceTypes.TotalPoints, points);
    }
    /************* RewardedVideo AdInfo Delegates *************/
    // Indicates that there’s an available ad.
    // The adInfo object includes information about the ad that was loaded successfully
    // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
        Debug.Log("Video Unavailable");
    }
    // Indicates that no ads are available to be displayed
    // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    void RewardedVideoOnAdUnavailable()
    {
        Debug.Log("Video Unavailable");
    }
    // The user completed to watch the video, and should be rewarded.
    // The placement parameter will include the reward data.
    // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        AddReward();
        Debug.Log("Total points: " + points);
    }
}