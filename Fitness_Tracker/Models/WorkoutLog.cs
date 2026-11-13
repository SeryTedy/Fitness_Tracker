namespace Fitness_Tracker.Models;

///  <summary> Result of comparing the total calories burned with the user's goal. </summary>
public record GoalReport(bool HasGoal, bool Achieved, double Percentage, string Message);

////// <summary> 
///keeps the activitie in the current session and calculates
///the total calories burned and the goal status.
////// </summary>

public class WorkoutLog
{
    private readonly List<ActivityRecord> _records = new();
    public IReadOnlyList<ActivityRecord> Records => _records;
    public double TotalCalories => _records.Sum(r => r.Calories);
    public void Add(ActivityRecord record) => _records.Add(record);
    public void Remove(ActivityRecord record) => _records.Remove(record);
    public void Clear() => _records.Clear();

    /// Report whether the goals has been achieved
    /// 
    public GoalReport Evaluate(int goal)
    {
        if(goal <= 0)
        {
            return new GoalReport(false, false, 0, "No goal set yet.Enter a calorie goal above and press 'Save goal'. ");


        }

        double total = TotalCalories;
        double percentage = total / goal * 100.0;

        if(total >= goal)
        {
            return new GoalReport(true, true, percentage,
                $"Goal achieved ! You bruned {total:F1} kcal (goal:{goal} kcal.");

        }

        return new GoalReport(true, false, percentage,
                $"Goal not achieved yet. { goal- total:F1} kcal left to burn ({percentage: F0}% done.");



    }

}

