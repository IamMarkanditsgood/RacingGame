using TMPro;
using UnityEngine;

public class EngineComponent : BasicComponentPanel
{
    [SerializeField] private TMP_Text _parametersText;
    public override void SetComponentPanel()
    {
        base.SetComponentPanel();

        CarEngineComponent carColorConfig = (CarEngineComponent)_config;
        SetParametersText(carColorConfig.CarIntParameters, carColorConfig.IntValue, _parametersText);
    }
 
    public override void InteractButtonPressed()
    {
        if (!_isBought && ResourcesManager.Instance.IsEnoughResource(ResourceTypes.Coins, _config.Price))
        {
            ResourcesManager.Instance.ModifyResource(ResourceTypes.Coins, -_config.Price);
            _interactionButton.GetComponentInChildren<TMP_Text>().text = "Use";

            SaveBoughtCarComponent();
        }
        else if (_isBought)
        {
            CarEngineComponent carColorConfig = (CarEngineComponent)_config;

            ModifyParameters(carColorConfig.CarIntParameters, carColorConfig.IntValue);
        }
    }    
}