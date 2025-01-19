using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InputSystem
{
    [SerializeField] private MobileInputSystem _mobileInputSystem;

    private List<IInputable> _inputSystems = new List<IInputable>();

    public void Init()
    {
        DeclareInputSystems();
    }

    public void Destroy()
    {
        _mobileInputSystem.Unsubscribe();
    }

    public void UpdateInputs()
    {
        foreach (var system in _inputSystems)
        {
            system.UpdateInput();
        }
    }

    private void DeclareInputSystems()
    {
        _inputSystems.Add(new KeyboardInputSystem());
        _inputSystems.Add(_mobileInputSystem);
    }
}