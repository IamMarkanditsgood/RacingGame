using TMPro;
using UnityEngine;

public class WheelsCarComponent : BasicComponentPanel
{
    [SerializeField] private TMP_Text _parametersText;

    public override void SetComponentPanel()
    {
        base.SetComponentPanel();

        CarWheelsComponent carColorConfig = (CarWheelsComponent)_config;
        SetParametersText(carColorConfig.CarIntParameters, carColorConfig.IntValue, _parametersText);
        SetParametersText(carColorConfig.CarFloatParameters, carColorConfig.FloatValue, _parametersText);
    }

    public override void InteractButtonPressed()
    {
        if (!_isBought && ResourcesManager.Instance.IsEnoughResource(ResourceTypes.Coins, _config.Price))
        {
            ResourcesManager.Instance.ModifyResource(ResourceTypes.Coins, -_config.Price);

            _interactionButton.GetComponentInChildren<TMP_Text>().text = "Use";
            SaveBoughtCarComponent();
        }
        else
        {
            CarWheelsComponent carColorConfig = (CarWheelsComponent)_config;

            ModifyParameters(carColorConfig.CarIntParameters, carColorConfig.IntValue);
            ModifyParameters(carColorConfig.CarFloatParameters, carColorConfig.FloatValue);

        }
    }
}