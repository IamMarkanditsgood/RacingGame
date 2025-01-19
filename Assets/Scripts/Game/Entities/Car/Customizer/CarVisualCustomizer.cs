using Photon.Pun;
using UnityEngine;

public class CarVisualCustomizer : MonoBehaviourPunCallbacks
{
    [SerializeField] private MeshRenderer _carRenderer;
    [SerializeField] private PhotonView _photonView;

    private CarData _carData = new CarData();
    private int _materialIndex; 

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

    private void Subscribe()
    {
        CarCustomizationEvents.OnMaterialChanged += OnMaterialChanged;
    }

    private void Unsubscribe()
    {
        CarCustomizationEvents.OnMaterialChanged -= OnMaterialChanged;
    }

    private void Config()
    {
        if (_carData != null && _carData.carMaterial != null)
        {
            CustomizeMaterial(_carData.carMaterial);

            if (_photonView != null && _photonView.IsMine)
            {
                _materialIndex = GetMaterialIndex(_carData.carMaterial);
                _photonView.RPC("SyncMaterial", RpcTarget.AllBuffered, _materialIndex);
            }
        }
    }

    private void OnMaterialChanged(Material material)
    {
        CustomizeMaterial(material);

        if (_photonView  != null && _photonView.IsMine)
        {
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
        Material material = GetMaterialByIndex(materialIndex);
        CustomizeMaterial(material);
    }

    private int GetMaterialIndex(Material material)
    {
        return MaterialDatabase.GetIndex(material);
    }

    private Material GetMaterialByIndex(int index)
    {
        return MaterialDatabase.GetMaterial(index);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_materialIndex);
        }
        else
        {
            _materialIndex = (int)stream.ReceiveNext();
            Material syncedMaterial = GetMaterialByIndex(_materialIndex);
            CustomizeMaterial(syncedMaterial);
        }
    }
}