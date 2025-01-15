using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CarEvents 
{
    public static event Action<bool> OnDriftEffect;
    public static event Action<bool> OnTireSkid;

    public static void DriftEffect(bool state)
    {
        OnDriftEffect?.Invoke(state);   
    }
    public static void TireSkid(bool state)
    {
        OnTireSkid?.Invoke(state);
    }
}
