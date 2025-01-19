using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : BasicScreen
{
    [SerializeField] private TMP_Text _totalDriftPoints;

    [SerializeField] private Button _startButton;
    [SerializeField] private Button _garageButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }

    private void Subscribe()
    {
        _startButton.onClick.AddListener(StartPressed);
        _garageButton.onClick.AddListener(Garage);
        _settingsButton.onClick.AddListener(Settings);
        _exitButton.onClick.AddListener(Exit);
    }

    private void UnSubscribe()
    {
        _startButton.onClick.RemoveListener(StartPressed);
        _garageButton.onClick.RemoveListener(Garage);
        _settingsButton.onClick.RemoveListener(Settings);
        _exitButton.onClick.RemoveListener(Exit);
    }

    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
        TextManager textManager = new TextManager();

        int points = ResourcesManager.Instance.GetResource(ResourceTypes.TotalPoints);
        textManager.SetText(points, _totalDriftPoints, true, "Total drift points:");
    }

    private void StartPressed()
    {
        UIManager.Instance.ShowPopup(PopupTypes.Lobby);
    }

    private void Garage()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Garage);
    }

    private void Settings()
    {
        UIManager.Instance.ShowPopup(PopupTypes.Settings);
    }

    private void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // якщо гра запущена €к б≥лд
        Application.Quit();
#endif
    }
}