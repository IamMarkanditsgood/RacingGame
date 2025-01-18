using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class OnlineLoby : BasicPopup
{
    [SerializeField] private PhotonRoomsManager _roomsManager;
    [SerializeField] private Button _createRoom;
    [SerializeField] private Button _joinRandomRoom;

    private void Start()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }

    private void Subscribe()
    {
        _createRoom.onClick.AddListener(CreateRoom);
        _joinRandomRoom.onClick.AddListener(JoinRandomRoom);
    }
    private void UnSubscribe()
    {
        _createRoom.onClick.RemoveListener(CreateRoom);
        _joinRandomRoom.onClick.RemoveListener(JoinRandomRoom);
    }

    public override void ResetPopup()
    {
    }

    public override void SetPopup()
    {
    }

    private void CreateRoom()
    {
        UIManager.Instance.ShowPopup(PopupTypes.Levels);
    }

    private void JoinRandomRoom()
    {
        _roomsManager.JoinRandomRoom();
    }
}