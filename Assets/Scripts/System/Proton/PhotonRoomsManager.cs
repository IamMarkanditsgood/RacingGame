using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class PhotonRoomsManager : MonoBehaviourPunCallbacks
{
    private const int _maxPlayers = 2;

    public void CreateRoom(LevelTypes level, CarTypes carType)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = _maxPlayers;

        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable
    {
        { "Level", level }
    };

        roomOptions.CustomRoomProperties = roomProperties;
        roomOptions.CustomRoomPropertiesForLobby = new[] { "Level" };

        PhotonNetwork.CreateRoom("Room", roomOptions, TypedLobby.Default);

        // «бер≥гаЇмо виб≥р машини у властивост€х гравц€
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
    {
        { "CarType", carType }
    });
    }

    public void JoinRandomRoom(CarTypes carType)
    {
        PhotonNetwork.JoinRandomRoom();

        // «бер≥гаЇмо виб≥р машини у властивост€х гравц€
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
    {
        { "CarType", carType }
    });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected to room");

        PhotonNetwork.LoadLevel("Game");
    }
}