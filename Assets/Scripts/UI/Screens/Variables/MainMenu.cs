using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : BasicScreen
{
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
        _garageButton.onClick.AddListener(Garage);
    }

    private void UnSubscribe()
    {
        _garageButton.onClick.RemoveListener(Garage);
    }
    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
    }
    private void Garage()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Garage);
    }
}
