namespace Fitness_Tracker.Activities;

/// Swimming : number of laps, time taken, average heart. 
public class SwimmingActivity : Activity
{
    public SwimmingActivity() : base("Swimming",
        new MetricDefinition ("Number of laps", "laps", 1, 500, wholeNumber: true),
        new MetricDefinition("Time taken", "minutes", 1, 600),
        new MetricDefinition("Average heart rate", "bpm", 60, 220))
        {

        }

    /// MET chosen from the heart rate(light / Moderate / vigorous),
    /// +1 MET when the swimmer is fast
    protected override double CalculateCore(double laps, double minutes, double heartRate)
    {
        double met;
        if (heartRate < 110) met = 5.8;
        else if (heartRate < 140) met = 8.3;
        else met = 9.8;

        if(laps/ minutes >= 0.8)
        {
            met += 1.0;
        }
        return MetCalories(met, minutes);

    }
}
