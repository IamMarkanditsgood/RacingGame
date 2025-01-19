using UnityEngine;

public class KeyboardInputSystem : IInputable
{
    public void UpdateInput()
    {
        CheckButtonsInput();
    }

    public bool IsPressed(KeyCode key)
    {
        if(Input.GetKey(key))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckButtonsInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            InputEvents.InvokeForward(true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            InputEvents.InvokeForward(false);
        }

        if (Input.GetKey(KeyCode.S))
        {
            InputEvents.InvokeBack(true);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            InputEvents.InvokeBack(false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            InputEvents.InvokeLeft(true);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            InputEvents.InvokeLeft(false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            InputEvents.InvokeRight(true);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            InputEvents.InvokeRight(false);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            InputEvents.InvokeHandBrake(true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            InputEvents.InvokeHandBrake(false);
        }
    }
}