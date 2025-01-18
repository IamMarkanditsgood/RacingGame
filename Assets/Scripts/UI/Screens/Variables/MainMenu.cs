using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : BasicScreen
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _garageButton;

    private void Awake()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }

    private void Subscribe()
    {
        _startButton.onClick.AddListener(StartPressed);
        _garageButton.onClick.AddListener(Garage);
    }

    private void UnSubscribe()
    {
        _startButton.onClick.RemoveListener(StartPressed);
        _garageButton.onClick.RemoveListener(Garage);
    }
    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
    }
    private void StartPressed()
    {
        UIManager.Instance.ShowPopup(PopupTypes.Lobby);
    }

    private void Garage()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Garage);
    }
}
