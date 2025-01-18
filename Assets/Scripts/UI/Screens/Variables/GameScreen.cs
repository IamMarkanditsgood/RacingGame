using TMPro;
using UnityEngine;

public class GameScreen : BasicScreen
{
    [SerializeField] private TMP_Text _driftScoreText;
    [SerializeField] private TMP_Text _pointsText;
    [SerializeField] private TMP_Text _speedText;
    [SerializeField] private TMP_Text _timerText;

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
        _timerText.text = "00:00";
        _pointsText.text = "0";
        _speedText.text = "0";
        _driftScoreText.text = "0";
        _driftScoreText.gameObject.SetActive(false);
    }

    public override void SetScreen()
    {
        ResetScreen();
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

    private void UpdateTimer(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60);
        int secs = Mathf.FloorToInt(seconds % 60);
        _timerText.text = $"{minutes:D2}:{secs:D2}";
    }

    private void UpdateDriftScore(float score, bool state)
    {
        float absoluteScore = Mathf.Abs(score);
        _driftScoreText.text = Mathf.RoundToInt(absoluteScore).ToString();
        _driftScoreText.gameObject.SetActive(state);
    }

    private void UpdateCarSpeed(float speed)
    {
        float absoluteCarSpeed = Mathf.Abs(speed);
        string speedText = Mathf.RoundToInt(absoluteCarSpeed).ToString();
        _speedText.text = "Speed: " + speedText;
    }

    private void UpdatePoints(int amount)
    {
        TextManager textManager = new TextManager();

        points += amount;
        textManager.SetText(points, _pointsText, true);
    }
}
