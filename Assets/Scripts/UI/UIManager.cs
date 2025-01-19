using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private BasicScreen[] _screens;
    [SerializeField] private BasicPopup[] _popups;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public BasicScreen GetScreen(ScreenTypes ScreenType)
    {
        var screen = _screens.FirstOrDefault(s => s.ScreenType == ScreenType);

        if (screen == null)
        {
            Debug.LogWarning($"Popup of type {ScreenType} was not found.");
        }

        return screen;
    }

    public BasicPopup GetPopup(PopupTypes popupType)
    {
        var popup = _popups.FirstOrDefault(p => p.PopupType == popupType);

        if (popup == null)
        {
            Debug.LogWarning($"Popup of type {popupType} was not found.");
        }

        return popup;
    }

    public void ShowScreen(ScreenTypes screenType)
    {
        CloseAllScreens();

        foreach (var screen in _screens)
        {
            if(screen.ScreenType == screenType)
            {
                screen.Show();
            }
        }
    }

    public void ShowPopup(PopupTypes popupType)
    {

        foreach (var popup in _popups)
        {
            if (popup.PopupType == popupType)
            {
                popup.Show();
            }
        }
    }

    private void CloseAllScreens()
    {
        foreach (var screen in _screens)
        {
            screen.Hide();
        }
    }
}