using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicScreen : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private ScreenTypes _screenType;

    public ScreenTypes ScreenType => _screenType;

    public virtual void Show()
    {
        SetScreen();
        _view.SetActive(true);
    }
    public virtual void Hide() 
    {
        ResetScreen();
        _view.SetActive(false);
    }

    public abstract void SetScreen();

    public abstract void ResetScreen();
}
