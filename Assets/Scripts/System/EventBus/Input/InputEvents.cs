using System;
using Unity.VisualScripting;
using UnityEngine;

public static class InputEvents
{
    public static event Action<bool> OnFPressed;
    public static event Action<bool> OnBPressed;
    public static event Action<bool> OnLPressed;
    public static event Action<bool> OnRPressed;

    public static event Action<bool> OnHandBrakePressed;

    public static void InvokeForward(bool state) => OnFPressed?.Invoke(state);
    public static void InvokeBack(bool state) => OnBPressed?.Invoke(state);
    public static void InvokeLeft(bool state) => OnLPressed?.Invoke(state);
    public static void InvokeRight(bool state) => OnRPressed?.Invoke(state);
    public static void InvokeHandBrake(bool state) => OnHandBrakePressed?.Invoke(state);
}