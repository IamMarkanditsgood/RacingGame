using System.Collections.Generic;
using UnityEngine;

public class CarInputSystem
{
    private List<IInputable> _inputSystems = new List<IInputable>();

    public void Init()
    {
        DeclareInputSystems();
    }

    public void CheckInput()
    {
        foreach (var system in _inputSystems)
        {
            system.CheckInput();
        }
    }
    public bool IsPressed(IInputable input, KeyCode key)
    {
        foreach (var system in _inputSystems)
        {
            if (system.GetType() == input.GetType() && system.IsPressed(key))
            {
                return true;
            }
        }
        return false;
    }
    private void DeclareInputSystems()
    {
        _inputSystems.Add(new KeyboardInputSystem());
    }
}