using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class OnlineLoby : BasicPopup
{
    [SerializeField] private PhotonRoomsManager _roomsManager;
    [SerializeField] private Button _createRoom;
    [SerializeField] private Button _joinRandomRoom;

    public override void Subscribe()
    {
        base.Subscribe();
        _createRoom.onClick.AddListener(CreateRoom);
        _joinRandomRoom.onClick.AddListener(JoinRandomRoom);
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
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