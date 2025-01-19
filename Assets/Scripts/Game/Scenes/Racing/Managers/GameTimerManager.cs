using Photon.Pun;
using UnityEngine;
using System.Collections;

public class GameTimerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private float _timeLeft;
    private bool _isTimerRunning = false;

    // Цей метод буде викликати створювач кімнати, коли він готовий почати гру
    public void StartTimer(float time)
    {
        if (PhotonNetwork.IsMasterClient) // тільки для господаря
        {
            _timeLeft = time;
            _isTimerRunning = true;
        }
    }

    private void Update()
    {
        if (_isTimerRunning)
        {
            if (PhotonNetwork.IsMasterClient) // Таймер рухається тільки на клієнті господаря
            {
                _timeLeft -= Time.deltaTime;

                if (_timeLeft <= 0)
                {
                    _timeLeft = 0;
                    FinishGame();  // Викликаємо завершення гри на Master Client
                }

                // Оновлюємо таймер у всіх клієнтів через RPC
                photonView.RPC("SyncTimer", RpcTarget.Others, _timeLeft);
            }

            // Сигналізуємо про оновлення таймера для усіх гравців
            GameEvents.TimerUpdate(_timeLeft);  // Для оновлення UI у всіх гравців
        }
    }

    // Викликається на всіх клієнтах, щоб синхронізувати таймер
    [PunRPC]
    private void SyncTimer(float time)
    {
        _timeLeft = time;

        // Сигналізуємо про оновлення таймера на всіх клієнтах
        GameEvents.TimerUpdate(_timeLeft);  // Оновлюємо відображення таймера на всіх клієнтах
    }

    private void FinishGame()
    {
        // Повідомляємо всіх гравців про завершення гри
        photonView.RPC("FinishGameOnAllClients", RpcTarget.All);  // Викликаємо RPC для всіх клієнтів

        _isTimerRunning = false;
        // Завершуємо гру для всіх
    }

    // RPC, щоб синхронізувати завершення гри на всіх клієнтах
    [PunRPC]
    private void FinishGameOnAllClients()
    {
        GameEvents.FinishGame();  // Викликаємо подію завершення гри на всіх клієнтах
    }

    // Реалізація IPunObservable для синхронізації таймера між клієнтами
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
