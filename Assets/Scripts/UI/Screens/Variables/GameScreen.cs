using TMPro;
using UnityEngine;

public class GameScreen : BasicScreen
{
    [SerializeField] private GameObject _buttons;

    [SerializeField] private TMP_Text _driftScoreText;
    [SerializeField] private TMP_Text _pointsText;
    [SerializeField] private TMP_Text _speedText;
    [SerializeField] private TMP_Text _timerText;

    private TextManager textManager = new TextManager();

    private int points;

    private void Start()
    {
        Subscribe();
        SetScreen();
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }

    public override void ResetScreen()
    {
        textManager.SetText("00:00", _timerText);
        textManager.SetText("0", _pointsText);
        textManager.SetText("0", _speedText);
        textManager.SetText("0", _driftScoreText);
        _driftScoreText.gameObject.SetActive(false);
    }

    public override void SetScreen()
    {
        ResetScreen();
        SetButtons();      
    }

    private void Subscribe()
    {
        GameEvents.OnTimerUpdate += UpdateTimer;
        GameEvents.OnDriftScoreeUpdate += UpdateDriftScore;
        GameEvents.OnSpeedUpdate += UpdateCarSpeed;
        GameEvents.OnPointsUpdate += UpdatePoints;
    }

    private void UnSubscribe()
    {
        GameEvents.OnTimerUpdate -= UpdateTimer;
        GameEvents.OnDriftScoreeUpdate -= UpdateDriftScore;
        GameEvents.OnSpeedUpdate -= UpdateCarSpeed;
        GameEvents.OnPointsUpdate -= UpdatePoints;
    }

    private void SetButtons()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            _buttons.SetActive(false);
        }
        else if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            _buttons.SetActive(true);
        }
    }

    private void UpdateTimer(float seconds)
    {
        textManager.SetTimerText(_timerText, seconds, true);
    }

    private void UpdateDriftScore(float score, bool state)
    {
        float absoluteScore = Mathf.Abs(score);

        textManager.SetText(Mathf.RoundToInt(absoluteScore), _driftScoreText);

        _driftScoreText.gameObject.SetActive(state);
    }

    private void UpdateCarSpeed(float speed)
    {
        float absoluteCarSpeed = Mathf.Abs(speed);
        string speedText = Mathf.RoundToInt(absoluteCarSpeed).ToString();

        textManager.SetText(speedText, _speedText, false, "Speed: ");
    }

    private void UpdatePoints(int amount)
    {
        points += amount;
        textManager.SetText(points, _pointsText, true);
    }
}