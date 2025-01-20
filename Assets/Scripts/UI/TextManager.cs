using TMPro;
using UnityEngine;

public class TextManager
{
    public void SetText(object message, TMP_Text textRow, bool formatKNumber = false, string frontAddedMessage = "", string endAddedMessage = "", bool addToPrevious = false)
    {
        string formattedText = GetFormattedText(message, formatKNumber);

        if (addToPrevious)
        {
            textRow.text += frontAddedMessage + formattedText + endAddedMessage;
        }
        else 
        {
            textRow.text = frontAddedMessage + formattedText + endAddedMessage;
        }
    }

    public void SetTimerText(TMP_Text textRow, float seconds, bool showHoursAndMinutes = false, string frontAddedMessage = "", string endAddedMessage = "")
    {
        textRow.text = $"{frontAddedMessage}{FormatTime(seconds, showHoursAndMinutes)}{endAddedMessage}";
    }

    private string FormatTime(float seconds, bool showHoursAndMinutes)
    {
        if (showHoursAndMinutes)
        {
            int hours = Mathf.FloorToInt(seconds / 3600);
            int minutes = Mathf.FloorToInt((seconds % 3600) / 60);
            int secs = Mathf.FloorToInt(seconds % 60);

            return hours > 0
                ? $"{hours:D2}:{minutes:D2}:{secs:D2}"
                : $"{minutes:D2}:{secs:D2}";
        }

        int secsOnly = Mathf.FloorToInt(seconds);
        return $"{secsOnly}";
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