using TMPro;

public class TextManager
{
    public void SetText(object message, TMP_Text textRow, bool formatKNumber = false)
    {
        textRow.text = GetFormattedText(message, formatKNumber);
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