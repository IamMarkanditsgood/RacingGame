using System;

public static class CarEvents 
{
    public static event Action OnParameterSaved;
    public static event Action<bool> OnDrift;
    public static event Action<bool> OnTireSkid;
    public static event Action<CarParameters, object> OnCarParameterModified;

    public static void SaveParameters() => OnParameterSaved?.Invoke();
    public static void Drift(bool state) => OnDrift?.Invoke(state);
    public static void TireSkid(bool state) => OnTireSkid?.Invoke(state);
    public static void ModifyCarParameter(CarParameters carParameter, object value) => OnCarParameterModified?.Invoke(carParameter, value);
}