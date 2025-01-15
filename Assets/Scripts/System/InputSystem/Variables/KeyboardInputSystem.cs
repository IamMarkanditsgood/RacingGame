using UnityEngine;

public class KeyboardInputSystem : IInputable
{
    public void CheckInput()
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
            InputEvents.InvokeW();
        }
        if (Input.GetKey(KeyCode.S))
        {
            InputEvents.InvokeS();
        }
        if (Input.GetKey(KeyCode.A))
        {
            InputEvents.InvokeA();
        }
        if (Input.GetKey(KeyCode.D))
        {
            InputEvents.InvokeD();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            InputEvents.InvokeSpace(1);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            InputEvents.InvokeSpace(0);
        }
    }
}