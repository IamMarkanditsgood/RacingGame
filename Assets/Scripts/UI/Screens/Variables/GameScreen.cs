using TMPro;
using UnityEngine;

public class GameScreen : BasicScreen
{
    [SerializeField] private TMP_Text _driftScoreText;
    [SerializeField] private TMP_Text _pointsText;
    [SerializeField] private TMP_Text _speedText;

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
        GameEvents.OnDriftScoreeUpdate += UpdateDriftScore;
        GameEvents.OnSpeedUpdate += UpdateCarSpeed;
        GameEvents.OnPointsUpdate += UpdatePoints;
    }

    private void UnSubscribe()
    {
        GameEvents.OnDriftScoreeUpdate -= UpdateDriftScore;
        GameEvents.OnSpeedUpdate -= UpdateCarSpeed;
        GameEvents.OnPointsUpdate -= UpdatePoints;
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
    private void UpdatePoints(float amount)
    {
        TextManager textManager = new TextManager();

        points += (int)amount;
        textManager.SetText(points, _pointsText, true);
    }
}
