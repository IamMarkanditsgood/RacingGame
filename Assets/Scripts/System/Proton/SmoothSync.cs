using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SmoothSync : MonoBehaviourPun, IPunObservable
{
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    public bool isMine = true;

    void Update()
    {
        if (!isMine)
        {
            // Плавное перемещение и поворот
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Відправка даних позиції та ротації
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Отримання даних позиції та ротації
            targetPosition = (Vector3)stream.ReceiveNext();
            targetRotation = (Quaternion)stream.ReceiveNext();

            // Встановіть початкову позицію одразу, щоб уникнути телепортацій
            if (Vector3.Distance(transform.position, targetPosition) > 10f) // Якщо об'єкт дуже далеко
            {
                transform.position = targetPosition;
                transform.rotation = targetRotation;
            }
        }
    }
}
