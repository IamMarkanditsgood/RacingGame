using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsDecision : BasicPopup
{
    [SerializeField] private PhotonRoomsManager _roomsManager;
    [SerializeField] private Transform _container;
    [SerializeField] private Button _buttonPref;
    [SerializeField] private List<LevelTypes> _levels;

    private List<Button> _levelButtons = new List<Button>();

    public override void Unsubscribe()
    {
        UnsubscribeLevelButtons();
    }

    public override void ResetPopup()
    {
        foreach (var button in _levelButtons)
        {
            Destroy(button.gameObject);
        }
        _levelButtons.Clear();
    }

    public override void SetPopup()
    {
        for (int i = 0; i < _levels.Count; i++)
        {
            Button button = Instantiate(_buttonPref, _container);
            button.GetComponentInChildren<TMP_Text>().text = $"Level{i + 1}";
            _levelButtons.Add(button);
        }
        SubscribeLevelButtons();
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

    public void PlayLevel(int levelIndex)
    {
        CarEvents.SaveParameters();

        CarData carData = SaveManager.JsonStorage.LoadFromJson<CarData>(GameSaveKeys.CarData);
        CarTypes carType = carData.carType;
        _roomsManager.CreateRoom(_levels[levelIndex], carType);
    }
}