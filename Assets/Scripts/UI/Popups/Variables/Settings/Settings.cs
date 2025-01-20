using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Settings : BasicPopup
{
    [SerializeField] private GraphicsQualityManager _graphicsQualityManager;

    public override void Subscribe()
    {
        base.Subscribe();
        _graphicsQualityManager.Init();
    }
    public override void Unsubscribe()
    {
        base.Unsubscribe();
        _graphicsQualityManager.UnSubscribe();
    }

    public override void ResetPopup()
    {
    }

    public override void SetPopup()
    {
    }
}