using System.Globalization;

namespace Fitness_Tracker.Services;

/// <summary> Converts text typed by the user into number </summary>


public static class InputParser
{
    /// Accepts both "1.5" and "1,5". Return falso for empty text, letters , "NaN" or "Infinity".
    

    public static bool TryParse(string? text, out double value)
    {
        value = 0;
        if (string.IsNullOrWhiteSpace(text))    
        {
            return false;
        }

        string cleaned = text.Trim().Replace(',', '.');
        return double.TryParse(cleaned, NumberStyles.Float, CultureInfo.InvariantCulture, out value)
            && double.IsFinite(value);
    }
}
