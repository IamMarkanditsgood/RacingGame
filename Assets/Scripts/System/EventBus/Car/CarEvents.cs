using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CarEvents 
{
    public static event Action<bool> OnDrift;
    public static event Action<bool> OnTireSkid;

    public static void Drift(bool state) => OnDrift?.Invoke(state);
    public static void TireSkid(bool state) => OnTireSkid?.Invoke(state);
}
