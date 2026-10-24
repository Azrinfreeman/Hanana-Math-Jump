using System.Text;
using TMPro;

public static class TextUtil
{
    // Drops characters the label's font (including its fallbacks) cannot draw, so no empty boxes show up.
    public static string Printable(TMP_Text label, string text)
    {
        if (string.IsNullOrEmpty(text) || !label || !label.font)
            return text;

        var sb = new StringBuilder(text.Length);
        foreach (char c in text)
        {
            if (label.font.HasCharacter(c, true))
                sb.Append(c);
        }
        string result = sb.ToString().Trim();
        return result.Length > 0 ? result : "-";
    }
}
