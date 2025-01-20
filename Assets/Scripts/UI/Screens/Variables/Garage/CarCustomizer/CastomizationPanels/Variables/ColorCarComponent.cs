using TMPro;

public class ColorCarComponent : BasicComponentPanel
{
    public override void InteractButtonPressed()
    {
        if (!_isBought && ResourcesManager.Instance.IsEnoughResource(ResourceTypes.Coins, _config.Price))
        {
            ResourcesManager.Instance.ModifyResource(ResourceTypes.Coins, -_config.Price);
            _interactionButton.GetComponentInChildren<TMP_Text>().text = "Use";
            SaveBoughtCarComponent();   
        }

        else if(_isBought)
        {
            CarColorConfig carColorConfig = (CarColorConfig)_config;
            CarEvents.ModifyCarParameter(carColorConfig.CarParameter, carColorConfig.Material);
            CarCustomizationEvents.ChangeMaterial(carColorConfig.Material);
        }
    }    
}