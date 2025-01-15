using System;
using Unity.VisualScripting;
using UnityEngine;

public static class InputEvents
{
    public static event Action OnWPressed;
    public static event Action OnSPressed;
    public static event Action OnAPressed;
    public static event Action OnDPressed;

    public static event Action<int> OnSpacePressed;

    public static void InvokeW() => OnWPressed?.Invoke();
    public static void InvokeS() => OnSPressed?.Invoke();
    public static void InvokeA() => OnAPressed?.Invoke();
    public static void InvokeD() => OnDPressed?.Invoke();
    public static void InvokeSpace(int value) => OnSpacePressed?.Invoke(value);
}