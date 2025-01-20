using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Garage : BasicScreen
{
    [SerializeField] private CarShopManager _carShopManager;
    [SerializeField] private CustomizerManager _customizerManager;
    [SerializeField] private CarDataManager _carDataManager;

    [SerializeField] private List<CastomizationButton> _customizationButtons;

    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private Button _addCoinsButton;
    [SerializeField] private Button _mainMenuButton;

    private const CarTypes _basicCar = CarTypes.CarBasic;

    [Serializable]
    public class CastomizationButton
    {
        public Button button;
        public CarCastomizedComponents componentsType;
    }

    private void Start()
    {
        Init();
        Subscribe();  
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }

    private void Init()
    {
        if(!SaveManager.PlayerPrefs.IsSaved(GarageSaveKeys.CurrentSelectedCar))
        {
            SaveManager.PlayerPrefs.SaveEnum(GarageSaveKeys.CurrentSelectedCar, _basicCar);
        }
    }

    private void Subscribe()
    {
        ResourcesManager.Instance.OnResourceModified += UpdateResource;

        for (int i = 0; i < _customizationButtons.Count; i++)
        {
            int index = i;
            _customizationButtons[index].button.onClick.AddListener(() => UpdateCastomizationPanels(_customizationButtons[index].componentsType));
        }

        _addCoinsButton.onClick.AddListener(AddCoins);
        _mainMenuButton.onClick.AddListener(MainMenu);

        _carShopManager.Subscribe();
    }

    private void UnSubscribe()
    {
        ResourcesManager.Instance.OnResourceModified -= UpdateResource;

        for (int i = 0; i < _customizationButtons.Count; i++)
        {
            int index = i;
            _customizationButtons[index].button.onClick.RemoveListener(() => UpdateCastomizationPanels(_customizationButtons[index].componentsType));
        }
        _addCoinsButton.onClick.RemoveListener(AddCoins);
        _mainMenuButton.onClick.RemoveListener(MainMenu);

        _carShopManager.Unsubscribe();
    }

    public override void ResetScreen()
    {
        _carShopManager.ResetCarShop();
        _customizerManager.Reset();  
    }

    public override void SetScreen()
    {
        _coinsText.text = "Coins: " + ResourcesManager.Instance.GetResource(ResourceTypes.Coins);

        _carShopManager.ConfigureCarShop(_carDataManager.CarData.carMaterial);
        _customizerManager.SetCastomizer(CarCastomizedComponents.Color);
    }

    public void UpdateResource(ResourceTypes type, int value)
    {
        if(type == ResourceTypes.Coins)
        {
            _coinsText.text = "Coins: " + value;
        }
    }

    private void UpdateCastomizationPanels(CarCastomizedComponents panelsType)
    {
        _customizerManager.SetCastomizer(panelsType);
    }

    private void AddCoins()
    {
        UIManager.Instance.ShowPopup(PopupTypes.IAPCoins);
    }

    private void MainMenu()
    {
        CarEvents.SaveParameters();
        UIManager.Instance.ShowScreen(ScreenTypes.MainMenu);
    }
}