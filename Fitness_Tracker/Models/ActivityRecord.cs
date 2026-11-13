namespace Fitness_Tracker.Models;

///<summary> One recorded activity </summary>

public class ActivityRecord
{
    public DateTime Time { get;  }
    public string ActivityName { get; }
    public string MetricsSummary{ get; }

    public double Calories { get; }

    public ActivityRecord(string activityName, string metricsSummary, double calories)
    {
        Time = DateTime.Now;
        ActivityName = activityName;
        MetricsSummary = metricsSummary;
        Calories = calories;
    }

}
