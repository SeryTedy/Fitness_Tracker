namespace Fitness_Tracker.Activities;

/// <summary>
/// Base class for all fitness activities. Every activity has a name exactly,
/// Three metrics, its own calorie formula.Subclasses only supply the 
/// metric definintions and CalculateCore(Polymorphism).
/// </summary>
/// 
public abstract class Activity
{
    /// Asummed body wieght because the assignment did not gives no weight metrics
    protected const double BodyWeightKg = 70.0;
    protected const double KcalPerMeterClimbed = BodyWeightKg * 9.81 / (0.25 * 4184.0);
    private readonly MetricDefinition[] _metrics;
    public string Name { get; }

    public IReadOnlyList<MetricDefinition> Metrics => _metrics;

    protected Activity(string name, MetricDefinition metric1, MetricDefinition metric2, MetricDefinition metric3 )
    {
        Name = name;
        _metrics = new[] { metric1, metric2, metric3 };
    }

    ///Checks the three values.Returns an error message, or null if all metrics are valid
    public string? ValidateMetrics(double[] values)
    {
        if(values.Length != _metrics.Length)
        {
            return $"{Name} nedds exactly {_metrics.Length} values.";

        }

        for(int i = 0; i < _metrics.Length; i++)
        {
            string? error = _metrics[i].Validate(values[i]);
            if(error != null)
            {
                return error;
            }
        }
        return null;
    }

    /// Calculates the calories burned. 
    public double CalculateCalories(double[] values)
    {
        string? error = ValidateMetrics(values);
        if (error != null)
        {
            throw new ArgumentException(error);

        }

        return CalculateCore(values[0], values[1], values[2]);
    }


    /// The activity specific formula. The values are already validated.
    protected abstract double CalculateCore(double metric1, double metric2, double metric3);

    /// MET formula : kcal = MET * bpdy weight *  times
    protected static double MetCalories(double met, double minutes) =>
        met * BodyWeightKg * (minutes / 60.0);

    /// Extra calories for climbing a given height. 
    protected static double ClimbingCalories(double metresClimbed) =>
        metresClimbed * KcalPerMeterClimbed;

    ///comboBox display the activity name; 
    public override string ToString() => Name;
    

}