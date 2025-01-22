using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPopup : BasicPopup
{
    [SerializeField] private GameObject _leavingLoader;
    [SerializeField] private Button _mainMenu;

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

    }
    public override void Unsubscribe()
    {
        base.Unsubscribe(); 

        _mainMenu.onClick.RemoveListener(MainMenu);
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

    private void AddReward()
    {
        ResourcesManager.Instance.ModifyResource(ResourceTypes.Coins, points);
        ResourcesManager.Instance.ModifyResource(ResourceTypes.TotalPoints, points);
    }
    
}