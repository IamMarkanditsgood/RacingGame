using System;
using Photon.Pun;
using UnityEngine;

public class CarEffectManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private bool useEffects = false;

    [SerializeField] private ParticleSystem RLWParticleSystem;
    [SerializeField] private ParticleSystem RRWParticleSystem;

    [SerializeField] private TrailRenderer RLWTireSkid;
    [SerializeField] private TrailRenderer RRWTireSkid;

    private PhotonView _photonView;

    public void Init(PhotonView photonView)
    {
        _photonView = photonView;
        Subscribe();
    }
    private void OnDestroy()
    {
        UnSubscribe();
    }
    public void Subscribe()
    {
        CarEvents.OnDrift += HandleDriftEffect;
        CarEvents.OnTireSkid += HandleTireSkid;
    }

    public void UnSubscribe()
    {
        CarEvents.OnDrift -= HandleDriftEffect;
        CarEvents.OnTireSkid -= HandleTireSkid;
    }

    private void HandleDriftEffect(bool state)
    {
        if (!useEffects) return;
        SyncDriftEffect(state);
    }

    private void HandleTireSkid(bool state)
    {
        if (!useEffects) return;
        SyncTireSkidEffect(state);
    }
    public void SyncDriftEffect(bool state)
    {
        _photonView.RPC("RPC_HandleDriftEffect", RpcTarget.All, state);
    }

    public void SyncTireSkidEffect(bool state)
    {
        _photonView.RPC("RPC_HandleTireSkidEffect", RpcTarget.All, state);
    }

    [PunRPC]
    private void RPC_HandleDriftEffect(bool state)
    {
        if (state)
        {
            StartDriftEffect();
        }
        else
        {
            StopDriftEffect();
        }
    }

    [PunRPC]
    private void RPC_HandleTireSkidEffect(bool state)
    {
        if (state)
        {
            StartTireSkid();
        }
        else
        {
            StopTireSkid();
        }
    }
    public void StartDriftEffect()
    {
        if (RLWParticleSystem != null) RLWParticleSystem.Play();
        if (RRWParticleSystem != null) RRWParticleSystem.Play();
    }

    public void StopDriftEffect()
    {
        if (RLWParticleSystem != null) RLWParticleSystem.Stop();
        if (RRWParticleSystem != null) RRWParticleSystem.Stop();
    }

    public void StartTireSkid()
    {
        if (RLWTireSkid != null) RLWTireSkid.emitting = true;
        if (RRWTireSkid != null) RRWTireSkid.emitting = true;
    }

    public void StopTireSkid()
    {
        if (RLWTireSkid != null) RLWTireSkid.emitting = false;
        if (RRWTireSkid != null) RRWTireSkid.emitting = false;
    }
}