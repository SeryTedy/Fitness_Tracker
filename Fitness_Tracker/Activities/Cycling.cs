namespace Fitness_Tracker.Activities;

/// Cycling : distance, time taken , elevation gain


public class Cycling : Activity
{
    public Cycling() : base("Cycling",
        new MetricDefinition("Distance", "km", 0.1, 300),
        new MetricDefinition("Time taken", "minutes", 1, 1440),
        new MetricDefinition("Elevation gain", "m", 0, 8000))
    {

    }

    /// MET chosen from the Cycling speed + extra energy for the climb.

    protected override double CalculateCore(double distancekm, double minutes, double ElevationGain)
    {
        double speedkmh = distancekm / (minutes / 60.0);
        double met;
        if (speedkmh <= 16.0) met = 4.0;
        else if (speedkmh <= 19.2) met = 6.8;
        else if (speedkmh <= 22.4) met = 8.0;
        else if (speedkmh <= 25.6) met = 10.0;
        else if (speedkmh <= 30.6) met = 12.0;
        

        else met = 15.8;


        return MetCalories(met, minutes) + ClimbingCalories(ElevationGain);

    }
}


