using System;

public static class GameEvents
{
    public static event Action<float> OnPointsUpdate;
    public static event Action<float> OnSpeedUpdate;
    public static event Action<float, bool> OnDriftScoreeUpdate;

    public static void PointsUpdate(float newAmount) => OnPointsUpdate?.Invoke(newAmount);
    public static void UpdateSpeed(float newAmount) => OnSpeedUpdate?.Invoke(newAmount);
    public static void DriftScoreUpdate(float newAmount, bool state) => OnDriftScoreeUpdate?.Invoke(newAmount, state);
}
