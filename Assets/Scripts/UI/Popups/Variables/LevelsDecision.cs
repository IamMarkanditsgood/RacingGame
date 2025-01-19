using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsDecision : BasicPopup
{
    [SerializeField] private PhotonRoomsManager _roomsManager;
    [SerializeField] private Transform _container;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _buttonPref;
    [SerializeField] private List<LeveTypes> _levels;

    private List<Button> _levelButtons = new List<Button>();

    private void Start()
    {
        Subscribe();
    }
    private void OnDestroy()
    {
        UnsubscribeLevelButtons();
        UnSubscribe();
    }

    private void Subscribe()
    {
        _backButton.onClick.AddListener(Back);
    }

    private void UnSubscribe()
    {
        _backButton.onClick.RemoveListener(Back);
    }

    private void SubscribeLevelButtons()
    {
        for(int i = 0; i < _levelButtons.Count; i++)
        {
            int index = i;
            _levelButtons[index].onClick.AddListener(() =>  PlayLevel(index));
        }
    }
    private void UnsubscribeLevelButtons()
    {
        for (int i = 0; i < _levelButtons.Count; i++)
        {
            int index = i;
            _levelButtons[index].onClick.RemoveListener(() => PlayLevel(index));
        }
    }
    public override void ResetPopup()
    {
        foreach(var button in _levelButtons)
        {
            Destroy(button.gameObject);
        }
        _levelButtons.Clear();
    }

    public override void SetPopup()
    {
        for(int i = 0; i <  _levels.Count; i++)
        {
            Button button = Instantiate(_buttonPref, _container);
            button.GetComponentInChildren<TMP_Text>().text = $"Level{i+1}";
            _levelButtons.Add(button);
        }
        SubscribeLevelButtons();
    }

    public void PlayLevel(int levelIndex)
    {
        CarTypes carType = SaveManager.PlayerPrefs.LoadEnum(GarageSaveKeys.CurrentSelectedCar, CarTypes.CarBasic);
        _roomsManager.CreateRoom(_levels[levelIndex], carType);
    }

    private void Back()
    {
        Hide();
    }
}
