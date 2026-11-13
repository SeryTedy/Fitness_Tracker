namespace Fitness_Tracker.Activities;

/// Running : distance, time taken , stroke per minute. 


public class Rowing : Activity
{
    public Rowing() : base("Rowing",
        new MetricDefinition("Distance", "m", 50, 50000),
        new MetricDefinition("Time taken", "minutes", 1, 600),
        new MetricDefinition("Strokes per minute", "spm", 10, 6000))
    {

    }

    /// MET  when the pace is faster then 2: 00 per 500m.

    protected override double CalculateCore(double distanceMeters, double minutes, double strokesPerMinute)
    {
       
        double met;
        if (strokesPerMinute < 20) met = 4.8;
        else if (strokesPerMinute <= 28) met = 7.0;
        
       

        else met = 8.5;

        double secondsPer500m = minutes * 60.0 / (distanceMeters / 500.0);
        if(secondsPer500m < 120.0)
        {
            met += 1.0;
        }
        return MetCalories(met, minutes);

    }
}

