using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class MobileInputSystem : IInputable
{
    [SerializeField] private Button _forwardButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Button _handBrakeButton;

    private bool _isSubscribed;

    public void UpdateInput()
    {
        if(!_isSubscribed)
        {
            _isSubscribed = true;
            Subscribe();
        }
    }

    private void Subscribe()
    {
        AddPointerEvents(_forwardButton, ForwardPressed);
        AddPointerEvents(_backButton, BackPressed);
        AddPointerEvents(_leftButton, LeftPressed);
        AddPointerEvents(_rightButton, RightPressed);
        AddPointerEvents(_handBrakeButton, HandBrakePressed);
    }

    public void Unsubscribe()
    {
        RemovePointerEvents(_forwardButton);
        RemovePointerEvents(_backButton);
        RemovePointerEvents(_leftButton);
        RemovePointerEvents(_rightButton);
        RemovePointerEvents(_handBrakeButton);
    }

    private void AddPointerEvents(Button button, Action<bool> action)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        ClickEvent(ref trigger, action);
        ReleaseEvent(ref trigger, action);
    }

    private void ClickEvent(ref EventTrigger trigger, Action<bool> action)
    {
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        pointerDownEntry.callback.AddListener((data) => action.Invoke(true));
        trigger.triggers.Add(pointerDownEntry);
    }

    private void ReleaseEvent(ref EventTrigger trigger, Action<bool> action)
    {
        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        pointerUpEntry.callback.AddListener((data) => action.Invoke(false));
        trigger.triggers.Add(pointerUpEntry);
    }

    private void RemovePointerEvents(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger != null)
        {
            trigger.triggers.Clear();
        }
    }

    private void ForwardPressed( bool state)
    {
        InputEvents.InvokeForward(state);
    }

    private void BackPressed(bool state)
    {
        InputEvents.InvokeBack(state);
    }

    private void LeftPressed(bool state)
    {
        InputEvents.InvokeLeft(state);
    }

    private void RightPressed(bool state)
    {
        InputEvents.InvokeRight(state);
    }

    private void HandBrakePressed(bool state)
    {
        InputEvents.InvokeHandBrake(state);
    }
}