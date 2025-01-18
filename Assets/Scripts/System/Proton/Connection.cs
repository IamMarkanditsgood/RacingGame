using UnityEngine;
using Photon.Pun;
using System;

public class Connection : MonoBehaviourPunCallbacks
{
    public event Action OnPhotonConnected;

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server, succesful!");
        OnPhotonConnected?.Invoke();
    }
}