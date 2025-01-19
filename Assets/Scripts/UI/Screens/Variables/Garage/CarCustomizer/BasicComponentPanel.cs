using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasicComponentPanel : MonoBehaviour
{
    [SerializeField] protected Image _image;
    [SerializeField] protected TMP_Text _name;
    [SerializeField] protected TMP_Text _price;
    [SerializeField] protected Button _interactionButton;

    protected CarCastomizedComponents _carComponentType;
    protected BasicCarComponentConfig _config;
    protected bool _isBought;

    public bool IsBought { get { return _isBought; } set { _isBought = value; } }

    private void Start()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }

    public virtual void Init(BasicCarComponentConfig config, CarCastomizedComponents carComponentType)
    {
        _config = config;
        _carComponentType = carComponentType;
    }

    public virtual void SetComponentPanel()
    {
        _name.text = _config.Name;
        _price.text = "Price: " + _config.Price;
        _interactionButton.GetComponentInChildren<TMP_Text>().text = "Buy";
        if (_isBought)
        {
            _price.text = "Bought";
            _interactionButton.GetComponentInChildren<TMP_Text>().text = "Use";
        }
        if (_image != null)
        {
            _image.sprite = _config.ComponentImage;
        }
    }

    public virtual void Subscribe()
    {
        _interactionButton.onClick.AddListener(InteractButtonPressed);
    }
    public virtual void UnSubscribe()
    {
        _interactionButton.onClick.RemoveListener(InteractButtonPressed);
    }

    public abstract void InteractButtonPressed();

    protected virtual void SetParametersText<T>(List<CarParameters> parameters, List<T> values, TMP_Text parametersText)
    {
        
        for (int i = 0; i < parameters.Count; i++)
        {
            parametersText.text += parameters[i] + " = " + values[i] + "\n";
        }
    }
    protected virtual void ModifyParameters<T>(List<CarParameters> parameters, List<T> values)
    {
        for (int i = 0; i < parameters.Count; i++)
        {
            CarEvents.ModifyCarParameter(parameters[i], values[i]);
        }
    }

    public virtual void SaveBoughtCarComponent()
    {
        _isBought = true;
        List<string> savedComponents = new List<string>();
        savedComponents = SaveManager.PlayerPrefs.LoadStringList(_carComponentType.ToString());
        for (int i = 0; i < savedComponents.Count; i++)
        {
            if (savedComponents[i] == _config.name)
            {
                return;
            }
        }
        savedComponents.Add(_config.name);
        SaveManager.PlayerPrefs.SaveStringList(_carComponentType.ToString(), savedComponents);
    }

}
