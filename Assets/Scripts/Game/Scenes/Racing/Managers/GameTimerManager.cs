using Photon.Pun;
using UnityEngine;
using System.Collections;

public class GameTimerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private float _timeLeft;
    private bool _isTimerRunning = false;

    // ��� ����� ���� ��������� ��������� ������, ���� �� ������� ������ ���
    public void StartTimer(float time)
    {
        if (PhotonNetwork.IsMasterClient) // ����� ��� ���������
        {
            _timeLeft = time;
            _isTimerRunning = true;
        }
    }

    private void Update()
    {
        if (_isTimerRunning)
        {
            if (PhotonNetwork.IsMasterClient) // ������ �������� ����� �� �볺�� ���������
            {
                _timeLeft -= Time.deltaTime;

                if (_timeLeft <= 0)
                {
                    _timeLeft = 0;
                    FinishGame();  // ��������� ���������� ��� �� Master Client
                }

                // ��������� ������ � ��� �볺��� ����� RPC
                photonView.RPC("SyncTimer", RpcTarget.Others, _timeLeft);
            }

            // ���������� ��� ��������� ������� ��� ��� �������
            GameEvents.TimerUpdate(_timeLeft);  // ��� ��������� UI � ��� �������
        }
    }

    // ����������� �� ��� �볺����, ��� ������������� ������
    [PunRPC]
    private void SyncTimer(float time)
    {
        _timeLeft = time;

        // ���������� ��� ��������� ������� �� ��� �볺����
        GameEvents.TimerUpdate(_timeLeft);  // ��������� ����������� ������� �� ��� �볺����
    }

    private void FinishGame()
    {
        // ����������� ��� ������� ��� ���������� ���
        photonView.RPC("FinishGameOnAllClients", RpcTarget.All);  // ��������� RPC ��� ��� �볺���

        _isTimerRunning = false;
        // ��������� ��� ��� ���
    }

    // RPC, ��� ������������� ���������� ��� �� ��� �볺����
    [PunRPC]
    private void FinishGameOnAllClients()
    {
        GameEvents.FinishGame();  // ��������� ���� ���������� ��� �� ��� �볺����
    }

    // ��������� IPunObservable ��� ������������ ������� �� �볺�����
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

    public float GetTimeLeft()
    {
        return _timeLeft;
    }
}
