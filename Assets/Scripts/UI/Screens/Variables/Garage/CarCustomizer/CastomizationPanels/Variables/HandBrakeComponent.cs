using TMPro;
using UnityEngine;

public class HandBrakeComponent : BasicComponentPanel
{
    [SerializeField] private TMP_Text _parametersText;

    public override void SetComponentPanel()
    {
        base.SetComponentPanel();

        _parametersText.text = "Unlock HandBrake";
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
            CarEvents.ModifyCarParameter(CarParameters.CanHandbrake, true);
        }
    }
}