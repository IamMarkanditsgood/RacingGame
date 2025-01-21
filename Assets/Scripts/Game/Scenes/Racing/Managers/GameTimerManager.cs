using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameTimerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private float _timeLeft;
    private bool _isTimerRunning = false;

    public void StartTimer(float time)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _timeLeft = time;
            _isTimerRunning = true;
        }
    }

    private void Update()
    {
        if (_isTimerRunning)
        {
            CalculateTimer();
        }
    }

    private void CalculateTimer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _timeLeft -= Time.deltaTime;

            if (_timeLeft <= 0)
            {
                _timeLeft = 0;
                FinishGame();
            }

            photonView.RPC("SyncTimer", RpcTarget.Others, _timeLeft);
        }

        GameEvents.TimerUpdate(_timeLeft);
    }

    [PunRPC]
    private void SyncTimer(float time)
    {
        _timeLeft = time;

        GameEvents.TimerUpdate(_timeLeft); 
    }

    private void FinishGame()
    {
        photonView.RPC("FinishGameOnAllClients", RpcTarget.All); 

        _isTimerRunning = false;
    }

    [PunRPC]
    private void FinishGameOnAllClients()
    {
        GameEvents.FinishGame();  
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_timeLeft);
        }
        else
        {
            _timeLeft = (float)stream.ReceiveNext();
        }
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"MasterClient switched to {newMasterClient.NickName}");

        if (PhotonNetwork.IsMasterClient)
        {
            StartTimer(_timeLeft);
        }
    }
}