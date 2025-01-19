using System;
using Photon.Pun;
using UnityEngine;

public class IsMine : MonoBehaviour
{
    public bool IsItMe(PhotonView _photonView)
    {
        return _photonView != null && _photonView.IsMine;
    }
}