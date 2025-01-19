using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class TextManager
{
    public void SetText(object message, TMP_Text textRow, bool formatKNumber = false, string frontAddedMessage = "", string endAddedMessage = "")
    {
        string formattedText = GetFormattedText(message, formatKNumber);
        textRow.text = frontAddedMessage + formattedText + endAddedMessage; 
    }
    public void SetTimerText(TMP_Text textRow, float seconds, bool showHoursAndMinutes = false, string frontAddedMessage = "", string endAddedMessage = "")
    {
        string formattedText;

        if (showHoursAndMinutes)
        {
            int hours = Mathf.FloorToInt(seconds / 3600);
            int minutes = Mathf.FloorToInt((seconds % 3600) / 60);
            int secs = Mathf.FloorToInt(seconds % 60);

            if (hours > 0)
            {
                formattedText = $"{hours:D2}:{minutes:D2}:{secs:D2}";
            }
            else
            {
                formattedText = $"{minutes:D2}:{secs:D2}";
            }
        }
        else
        {
            int secs = Mathf.FloorToInt(seconds);
            formattedText = $"{secs}";
        }
        textRow.text = frontAddedMessage + formattedText + endAddedMessage;
    }

    private string GetFormattedText(object message, bool formatKNumber =false)
    {
        if (formatKNumber && message is int number)
        {
            return FormatKNumber(number);
        }

        return message.ToString();
    }

    private string FormatKNumber(int number)
    {
        return number >= 1000
            ? (number / 1000f).ToString("0.#") + "K"
            : number.ToString();
    }
}