public class CarInputManager
{
    //Front
    public bool IsFPressed { get; set; }
    //Back
    public bool IsBPressed { get; set; }
    //Left
    public bool IsLPressed { get; set; }
    //Right
    public bool IsRPressed { get; set; }
    //HandBreack
    public bool IsDPressed { get; set; }

    public void Subscribe()
    {
        InputEvents.OnFPressed += ForwardPressed;
        InputEvents.OnBPressed += BackPressed;
        InputEvents.OnLPressed += LeftPressed;
        InputEvents.OnRPressed += RightPressed;
        InputEvents.OnHandBrakePressed += DriftPressed;
    }
    public void Unsubscribe()
    {
        InputEvents.OnFPressed -= ForwardPressed;
        InputEvents.OnBPressed -= BackPressed;
        InputEvents.OnLPressed -= LeftPressed;
        InputEvents.OnRPressed -= RightPressed;
        InputEvents.OnHandBrakePressed -= DriftPressed;
    }

    private void ForwardPressed(bool state)
    {
        IsFPressed = state;
    }
    private void BackPressed(bool state)
    {
        IsBPressed = state;
    }
    private void LeftPressed(bool state)
    {
        IsLPressed = state;
    }
    private void RightPressed(bool state)
    {
        IsRPressed = state;
    }
    private void DriftPressed(bool state)
    {
        IsDPressed = state;
    }

}