namespace Fitness_Tracker.Activities;

/// Running : distance, time taken , elevation gain


public class Running : Activity
{
    public Running() : base("Running",
        new MetricDefinition("Distance", "km", 0.1, 100),
        new MetricDefinition("Time taken", "minutes", 1, 1440),
        new MetricDefinition("Elevation gain", "m", 0, 5000))
    {

    }

    /// MET chosen from the running speed + extra energy for the climb.
   
    protected override double CalculateCore(double distancekm, double minutes, double ElevationGain)
    {
        double speedkmh = distancekm / (minutes /60.0);
        double met; 
        if (speedkmh <= 6.4) met = 6.0;
        else if (speedkmh <= 8.0) met = 8.3;
        else if (speedkmh <= 9.7) met = 9.8;
        else if (speedkmh <= 11.3) met = 11.0;
        else if (speedkmh <= 12.9) met = 12.3;
        else if (speedkmh <= 14.5) met = 14.5;

        else met = 16.0;

        
        return MetCalories(met, minutes) + ClimbingCalories(ElevationGain);

    }
}

