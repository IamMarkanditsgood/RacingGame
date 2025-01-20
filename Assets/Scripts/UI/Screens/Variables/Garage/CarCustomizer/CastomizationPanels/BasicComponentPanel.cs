using System.Collections.Generic;
using TMPro;
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

    private TextManager _textManager = new TextManager();

    public bool IsBought { get { return _isBought; } set { _isBought = value; } }

    private void Start()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }

    public virtual void Subscribe()
    {
        _interactionButton.onClick.AddListener(InteractButtonPressed);
    }

    public virtual void UnSubscribe()
    {
        _interactionButton.onClick.RemoveListener(InteractButtonPressed);
    }

    public virtual void Init(BasicCarComponentConfig config, CarCastomizedComponents carComponentType)
    {
        _config = config;
        _carComponentType = carComponentType;
    }

    public virtual void SetComponentPanel()
    {
        _textManager.SetText(_config.Name, _name);
        _textManager.SetText(_config.Price, _price, false, "Price: ");
        _textManager.SetText("Buy", _interactionButton.GetComponentInChildren<TMP_Text>());

        if (_isBought)
        {
            _textManager.SetText("Bought", _price);
            _textManager.SetText("Use", _interactionButton.GetComponentInChildren<TMP_Text>());
        }
        if (_image != null)
        {
            _image.sprite = _config.ComponentImage;
        }
    }

    protected virtual void SetParametersText<T>(List<CarParameters> parameters, List<T> values, TMP_Text parametersText)
    {
        for (int i = 0; i < parameters.Count; i++)
        {
            _textManager.SetText(values[i], parametersText, false, parameters[i].ToString(), "\n", true);
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

    public abstract void InteractButtonPressed();
}