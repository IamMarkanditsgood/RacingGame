using Photon.Pun;
using UnityEngine;

public class CarVisualCustomizer : MonoBehaviourPunCallbacks
{
    [SerializeField] private MeshRenderer _carRenderer;
    [SerializeField] private PhotonView _photonView;

    private CarData _carData = new CarData();
    private int _materialIndex; // Індекс матеріалу для синхронізації (відповідає матеріалу в базі даних)

    public void Init()
    {
        _carData = SaveManager.JsonStorage.LoadFromJson<CarData>(GameSaveKeys.CarData);
        Config();
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Config()
    {
        if (_carData != null && _carData.carMaterial != null)
        {
            // Встановлюємо локальний матеріал
            CustomizeMaterial(_carData.carMaterial);

            // Синхронізуємо матеріал із мережею
            if (_photonView != null&&_photonView.IsMine)
            {
                _materialIndex = GetMaterialIndex(_carData.carMaterial);
                _photonView.RPC("SyncMaterial", RpcTarget.AllBuffered, _materialIndex);
            }
        }
    }

    private void Subscribe()
    {
        CarCustomizationEvents.OnMaterialChanged += OnMaterialChanged;
    }

    private void Unsubscribe()
    {
        CarCustomizationEvents.OnMaterialChanged -= OnMaterialChanged;
    }

    private void OnMaterialChanged(Material material)
    {
        // Оновлення матеріалу локально
        CustomizeMaterial(material);

        if (_photonView  != null && _photonView.IsMine)
        {
            // Синхронізація з мережею
            _materialIndex = GetMaterialIndex(material);
            _photonView.RPC("SyncMaterial", RpcTarget.AllBuffered, _materialIndex);
        }
    }

    private void CustomizeMaterial(Material material)
    {
        Material[] materials = _carRenderer.materials;
        materials[0] = material;
        _carRenderer.materials = materials;
    }

    [PunRPC]
    private void SyncMaterial(int materialIndex)
    {
        // Отримуємо матеріал за індексом і застосовуємо його
        Material material = GetMaterialByIndex(materialIndex);
        CustomizeMaterial(material);
    }

    private int GetMaterialIndex(Material material)
    {
        // Логіка отримання індексу матеріалу (наприклад, з бази даних або масиву)
        return MaterialDatabase.GetIndex(material);
    }

    private Material GetMaterialByIndex(int index)
    {
        // Логіка отримання матеріалу за індексом
        return MaterialDatabase.GetMaterial(index);
    }

    // Реалізація IPunObservable для додаткової синхронізації
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Передача індексу матеріалу від власника
            stream.SendNext(_materialIndex);
        }
        else
        {
            // Отримання індексу матеріалу на інших клієнтах
            _materialIndex = (int)stream.ReceiveNext();
            Material syncedMaterial = GetMaterialByIndex(_materialIndex);
            CustomizeMaterial(syncedMaterial);
        }
    }
}
