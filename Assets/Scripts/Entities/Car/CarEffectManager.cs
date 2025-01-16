using System;
using UnityEngine;

[Serializable] 
public class CarEffectManager
{
    [SerializeField] private bool useEffects = false;

    [SerializeField] private ParticleSystem RLWParticleSystem;
    [SerializeField] private ParticleSystem RRWParticleSystem;

    [SerializeField] private TrailRenderer RLWTireSkid;
    [SerializeField] private TrailRenderer RRWTireSkid;

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

        if (state)
        {
            StartDriftEffect();
        }
        else
        {
            StopDriftEffect();
        }
    }

    private void HandleTireSkid(bool state)
    {
        if (!useEffects) return;

        if (state)
        {
            StartTireSkid();
        }
        else
        {
            StopTireSkid();
        }
    }

    private void StartDriftEffect()
    {
        if (RLWParticleSystem != null) RLWParticleSystem.Play();
        if (RRWParticleSystem != null) RRWParticleSystem.Play();
    }

    private void StopDriftEffect()
    {
        if (RLWParticleSystem != null) RLWParticleSystem.Stop();
        if (RRWParticleSystem != null) RRWParticleSystem.Stop();
    }

    private void StartTireSkid()
    {
        if (RLWTireSkid != null) RLWTireSkid.emitting = true;
        if (RRWTireSkid != null) RRWTireSkid.emitting = true;
    }

    private void StopTireSkid()
    {
        if (RLWTireSkid != null) RLWTireSkid.emitting = false;
        if (RRWTireSkid != null) RRWTireSkid.emitting = false;
    }
}