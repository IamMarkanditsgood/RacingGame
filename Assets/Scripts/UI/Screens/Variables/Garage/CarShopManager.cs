using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CarShopManager
{
    [SerializeField] private Transform _carSpawnPos;
    [SerializeField] private Button _interactButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _prevButton;
    [SerializeField] private TMP_Text _carPriceText;
    [SerializeField] private CarsCollection[] _cars;

    private List<CarTypes> _boughtCars = new List<CarTypes>();
    private CarsCollection _currentSelectedCar;

    private GameObject _carOnScene;
    private int _currentCarIndex;

    [Serializable]
    public class CarsCollection
    {
        public CarTypes carType = CarTypes.CarBasic;
        public GameObject carPrefab;
        public int price;
        public bool isBought;
    }

    public void Subscribe()
    {
        _interactButton.onClick.AddListener(InteractButton);
        _nextButton.onClick.AddListener(NextCar);
        _prevButton.onClick.AddListener(PreviousCar);
    }
    public void Unsubscribe()
    {
        _interactButton.onClick.RemoveListener(InteractButton);
        _nextButton.onClick.RemoveListener(NextCar);
        _prevButton.onClick.RemoveListener(PreviousCar);
    }

    public void ResetCarShop()
    {
        foreach (var _car in _cars)
        {
            _car.isBought = false;
        }

        if (_carOnScene != null)
        {
            UnityEngine.Object.Destroy(_carOnScene);
        }

        _nextButton.interactable = true;
        _prevButton.interactable = false;
        _interactButton.interactable = true;
        _currentCarIndex = 0;
    }

    public void ConfigureCarShop(Material carMaterial)
    {
        _currentSelectedCar = _cars[_currentCarIndex];

        SetBoughtCars();
        SetCurrentPage();
    }

    private void SetBoughtCars()
    {
        List<CarTypes> boughtCars = LoadBoughtCars();

        for (int i = 0; i < _cars.Length; i++)
        {
            for (int j = 0; j < boughtCars.Count; j++)
            {
                if (_cars[i].carType == boughtCars[j])
                {
                    _cars[i].isBought = true;
                }
            }
        }
    }

    private void SetCurrentPage()
    {
        SetButtons();
        SetCarModel();
        SetPageText();
    }

    private void SetCarModel()
    {
        if (_carOnScene != null)
        {
            UnityEngine.Object.Destroy(_carOnScene);
        }

        _carOnScene = UnityEngine.Object.Instantiate(_cars[_currentCarIndex].carPrefab, _carSpawnPos.position, _carSpawnPos.rotation);

        _carOnScene.GetComponent<CarVisualCustomizer>().Init();
    }

    private void SetButtons()
    {
        CarTypes carTypes = SaveManager.PlayerPrefs.LoadEnum(GarageSaveKeys.CurrentSelectedCar, CarTypes.CarBasic);

        if (carTypes == _currentSelectedCar.carType)
        {
            _interactButton.interactable = false;
        }
        else
        {
            _interactButton.interactable = true;
        }
    }

    private void SetPageText()
    {
        string priceText = "Price: " + _currentSelectedCar.price ;
        string buttonText = "Buy";

        if (_currentSelectedCar.isBought)
        {
            buttonText = "Use";
            priceText = "Bought";
        }

        _interactButton.GetComponentInChildren<TMP_Text>().text = buttonText;
        _carPriceText.text = priceText;
    }

    private List<CarTypes> LoadBoughtCars()
    {
        if (SaveManager.PlayerPrefs.IsSaved(GarageSaveKeys.BoughtCars))
        {
            _boughtCars = SaveManager.PlayerPrefs.LoadEnumList<CarTypes>(GarageSaveKeys.BoughtCars);

            return _boughtCars;
        }
        else
        {
            _boughtCars.Add(CarTypes.CarBasic);
            SaveManager.PlayerPrefs.SaveEnumList(GarageSaveKeys.BoughtCars,_boughtCars);

            return _boughtCars;
        }
    }

    private void NextCar()
    {
        CarEvents.SaveParameters();

        _prevButton.interactable = true;
        _currentCarIndex++;
        _currentSelectedCar = _cars[_currentCarIndex];

        SetCurrentPage();

        if(_currentCarIndex + 1 == _cars.Length)
        {
            _nextButton.interactable = false;
        }
    }

    private void PreviousCar()
    {
        CarEvents.SaveParameters();

        _nextButton.interactable = true;
        _currentCarIndex--;
        _currentSelectedCar = _cars[_currentCarIndex];

        SetCurrentPage();

        if (_currentCarIndex - 1 < 0)
        {
            _prevButton.interactable = false;
        }
    }

    private void InteractButton()
    {
        if (_currentSelectedCar.isBought)
        {
            SetCar();
        }
        else
        {
            TryToBuy();
        }
    }

    private void SetCar()
    {
        SaveManager.PlayerPrefs.SaveEnum(GarageSaveKeys.CurrentSelectedCar, _currentSelectedCar.carType);
        CarEvents.ModifyCarParameter(CarParameters.CarType, _currentSelectedCar.carType);

        _interactButton.interactable = false;
    }

    private void TryToBuy()
    {
        if (ResourcesManager.Instance.IsEnoughResource(ResourceTypes.Coins, _currentSelectedCar.price))
        {
            ResourcesManager.Instance.ModifyResource(ResourceTypes.Coins, -_currentSelectedCar.price);

            _currentSelectedCar.isBought = true;

            _boughtCars.Add(_currentSelectedCar.carType);
            SaveManager.PlayerPrefs.SaveEnumList(GarageSaveKeys.BoughtCars, _boughtCars);

            SetPageText();
        }
    }
}