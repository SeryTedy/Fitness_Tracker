namespace Fitness_Tracker.Activities;

/// Jump rope : nulber of jumps, time taken , average heart rate. 


public class JumpRope : Activity
{
    public JumpRope() : base("Jump rope",
        new MetricDefinition("Number of jumps", "jumps", 10, 50000, wholeNumber :true),
        new MetricDefinition("Time taken", "minutes", 1, 300),
        new MetricDefinition("Average heart rate", "bpm", 60, 220))
    {

    }

    /// MET  chosen from the jump rate, +1 MET when the average heart rate is 160 bm or more.

    protected override double CalculateCore(double jumps, double minutes, double heartRate)
    {
        double jumpsPerMinute = jumps / minutes; 
        double met;
        if (jumpsPerMinute < 100) met = 8.8;
        else if (jumpsPerMinute <= 160) met = 11.8;



        else met = 12.3;

        
        if (heartRate < 160)
        {
            met += 1.0;
        }
        return MetCalories(met, minutes);

    }
}

