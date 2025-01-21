using Photon.Pun;
using UnityEngine;

public class CarSoundsManager : MonoBehaviourPunCallbacks
{
    [Header("RigidBody is used for engine sound")]
    [SerializeField] private Rigidbody _carRigidbody;

    [Header("Engine sound")]
    [SerializeField] private AudioSource _carEngineSound;
    [SerializeField] private float minEnginePitch = 0.15f;
    [SerializeField] private float pitchSmoothTime = 0.2f;

    [Header("TireSkid sound")]
    [SerializeField] private AudioSource _tireSkidSound;

    private PhotonView _photonView;

    private bool _isDrifting;
    private bool _isTireSkid;
    private bool _playSounds;
    private float initialCarEngineSoundPitch;
    private float newPitch;
    private float pitchVelocity;



    private void Update()
    {
        if (_playSounds)
        {
            EngineSound();

        }
    }
    private void OnDestroy()
    {
        UnSubscribe();
    }

    public void Init(PhotonView photonView)
    {
        _photonView = photonView;

        if (_carRigidbody == null && GetComponent<Rigidbody>())
        {
            _carRigidbody = GetComponent<Rigidbody>();
        }

        InitSounds();
        Subscribe();
    }

    private void InitSounds()
    {
        _playSounds = true;

        _tireSkidSound.Stop();

        _carEngineSound.Play();
        newPitch = initialCarEngineSoundPitch;
        initialCarEngineSoundPitch = _carEngineSound.pitch;
    }

    public void Subscribe()
    {
        CarEvents.OnTireSkid += TireSkid;
        CarEvents.OnDrift += Drift;
    }

    public void UnSubscribe()
    {
        CarEvents.OnTireSkid -= TireSkid;
        CarEvents.OnDrift -= Drift;
    }

    private void Drift(bool state)
    {
        if (_photonView.IsMine)
        {
            _photonView.RPC("RPC_DriftSound", RpcTarget.All, state);
        }
    }

    private void TireSkid(bool state)
    {
        if (_photonView.IsMine)
        {
            _photonView.RPC("RPC_TireSkidSound", RpcTarget.All, state);
        }
    }

    [PunRPC]
    private void RPC_DriftSound(bool state)
    {
        if (state != _isDrifting)
        {
            _isDrifting = state;
            TireSkidSounds();
        }
    }

    [PunRPC]
    private void RPC_TireSkidSound(bool state)
    {
        _isTireSkid = state;
        TireSkidSounds();
    }

    private void TireSkidSounds()
    {
        if ((_isTireSkid || _isDrifting) && !_tireSkidSound.isPlaying)
        {
            _tireSkidSound.Play();
        }
        else if ((!_isTireSkid && !_isDrifting) && _tireSkidSound.isPlaying)
        {
            _tireSkidSound.Stop();
        }
    }

    private void EngineSound()
    {
        if (_carRigidbody == null)
            return;

        float targetPitch = initialCarEngineSoundPitch + (Mathf.Abs(_carRigidbody.velocity.magnitude) / 25f);
        newPitch = Mathf.SmoothDamp(newPitch, targetPitch, ref pitchVelocity, pitchSmoothTime);

        if (minEnginePitch > newPitch)
        {
            newPitch = minEnginePitch;
        }

        _carEngineSound.pitch = newPitch;

        if (_photonView.IsMine) 
        {
            _photonView.RPC("RPC_SetEnginePitch", RpcTarget.All, newPitch);
        }
    }
    [PunRPC]
    private void RPC_SetEnginePitch(float newPitch)
    {
        _carEngineSound.pitch = newPitch;
    }
}