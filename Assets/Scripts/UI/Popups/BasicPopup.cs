using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasicPopup : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _view;
    [SerializeField] private PopupTypes _popupType;
    [SerializeField] private Button _closeButton;

    public PopupTypes PopupType => _popupType;

    private void Start()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    public virtual void Subscribe()
    {
        if(_closeButton != null)
        {
            _closeButton.onClick.AddListener(ClosePressed);
        }
    }

    public virtual void Unsubscribe()
    {
        if (_closeButton != null)
        {
            _closeButton.onClick.RemoveAllListeners();
        }
    }

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

    public virtual void ClosePressed()
    {
        Hide();
    }

    public abstract void SetPopup();

    public abstract void ResetPopup();
}
