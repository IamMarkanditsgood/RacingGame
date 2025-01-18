using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class PhotonRoomsManager : MonoBehaviourPunCallbacks
{
    public void CreateRoom(LeveTypes level, CarTypes carType)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "Level", level },
            { "CarType", carType }
        };
        roomOptions.CustomRoomProperties = roomProperties;
        roomOptions.CustomRoomPropertiesForLobby = new[] { "Level", "CarType" };

        PhotonNetwork.CreateRoom("Room", roomOptions, TypedLobby.Default);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected to room");

        PhotonNetwork.LoadLevel("Game");
    }
}
