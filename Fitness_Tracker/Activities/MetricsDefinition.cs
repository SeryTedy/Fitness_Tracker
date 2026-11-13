namespace Fitness_Tracker.Activities;

/// Describes one emtric of an activity: its name, unit and accepted range

public class MetricDefinition
{
    public string Name { get; }
    public string Unit { get; }
    public double Min { get; }
    public double Max { get; }
    public bool WholeNumber { get; }

    public MetricDefinition(string name, string unit, double min, double max, bool wholeNumber = false)
    {
        Name = name;
        Unit = unit;
        Min = min;
        Max = max;
        WholeNumber = wholeNumber;
    }


    /// Text shown next to the input box
    public string Label => $"{Name} ({Unit}) [{Min:0.##} - {Max:N0}]";

    // Return an error message , or null if the value is acceptable.

    public string? Validate(double value)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            return $"{Name} must be a valid number.";
        }

        if (value < Min || value > Max)
        {
            return $"{Name} must be between {Min:0.##} and {Max:N0} {Unit}.";


        }

        if (WholeNumber && value != Math.Floor(value))
        {
            return $"{Name} must be a whole number not decimals.";
        }

        return null;
    }
}


