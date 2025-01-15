using System.Xml.Serialization;
using UnityEngine;

public interface IInputable
{
    public void CheckInput();
    public bool IsPressed(KeyCode key);
}
