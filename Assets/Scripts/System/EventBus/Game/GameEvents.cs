using System;

public static class GameEvents
{
    public static event Action OnGameFinish;
    public static event Action<int> OnPointsUpdate;
    public static event Action<float> OnTimerUpdate;
    public static event Action<float> OnSpeedUpdate;
    public static event Action<float, bool> OnDriftScoreeUpdate;

    public static void FinishGame() => OnGameFinish?.Invoke();  
    public static void PointsUpdate(int newAmount) => OnPointsUpdate?.Invoke(newAmount);
    public static void TimerUpdate(float newAmount) => OnTimerUpdate?.Invoke(newAmount);
    public static void UpdateSpeed(float newAmount) => OnSpeedUpdate?.Invoke(newAmount);
    public static void DriftScoreUpdate(float newAmount, bool state) => OnDriftScoreeUpdate?.Invoke(newAmount, state); 
}