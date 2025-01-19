using UnityEngine;
using UnityEngine.UI;

public class IAPCoinBuy : BasicPopup
{
    [SerializeField] private Button _backButton;

    private void Start()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }

    private void Subscribe()
    {
        _backButton.onClick.AddListener(Back);
    }

    private void UnSubscribe()
    {
        _backButton.onClick.RemoveListener(Back);
    }

    public override void ResetPopup()
    {
    }

    public override void SetPopup()
    {
    }

    private void Back()
    {
        Hide();
    }
}
