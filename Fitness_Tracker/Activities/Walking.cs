namespace Fitness_Tracker.Activities;

/// Walking : steps, distance, time taken
public class WalkingActivity : Activity
{
    public WalkingActivity() : base("Walking",
        new MetricDefinition ("Steps", "steps", 1, 200000, wholeNumber: true),
        new MetricDefinition("Distance", "km", 0.01, 100),
        new MetricDefinition("Time taken", "minutes", 1, 1440))
    {
    }

    /// Average of two estimates : a MET value Chosen from the walking speed
    /// (Compeduim of Physical Activities) and 0.04 kcal per step.
    protected override double CalculateCore(double steps, double distanceKm, double minutes)
    {
        double speedKmh = distanceKm / (minutes /60.0);
        double met;
        if (speedKmh <= 3.2) met = 2.8;
        else if (speedKmh <= 4.0) met = 3.0;
        else if (speedKmh <= 4.8) met = 3.5;
        else if (speedKmh <= 5.6) met = 4.3;
        else if (speedKmh <= 6.4) met = 5.0;
        else met = 7.0;

        double metEstimated = MetCalories(met, minutes);
        double stepEstimate = steps * 0.04;
        return (metEstimated + stepEstimate) / 2.0;

    }
}
