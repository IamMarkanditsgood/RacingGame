using UnityEngine;
using UnityEngine.UI;

public class OnlineLoby : BasicPopup
{
    [SerializeField] private PhotonRoomsManager _roomsManager;

    [SerializeField] private Button _createRoom;
    [SerializeField] private Button _joinRandomRoom;

    public override void ResetPopup()
    {
    }

    public override void SetPopup()
    {
    }

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

    private void CreateRoom()
    {
        UIManager.Instance.ShowPopup(PopupTypes.Levels);
    }

    private void JoinRandomRoom()
    {
        CarEvents.SaveParameters();
        CarTypes carType = SaveManager.PlayerPrefs.LoadEnum(GarageSaveKeys.CurrentSelectedCar, CarTypes.CarBasic); 
        _roomsManager.JoinRandomRoom(carType);
    }
}