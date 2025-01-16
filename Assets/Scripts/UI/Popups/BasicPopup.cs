using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicPopup : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private PopupTypes _popupType;

    public PopupTypes PopupType => _popupType;

    public virtual void Show()
    {
        SetPopup();
        _view.SetActive(true);
    }
    public virtual void Hide()
    {
        ResetPopup();
        _view.SetActive(false);
    }

    public abstract void SetPopup();

    public abstract void ResetPopup();
}
