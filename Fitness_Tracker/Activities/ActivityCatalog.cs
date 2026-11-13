using Fitness_Tracker.Activities;

/// The list of Six activties supported by the program.
public static class ActivityCatalog
{
    public static IReadOnlyList<Activity> All { get; } = new Activity[]

    {
        new WalkingActivity(),
        new SwimmingActivity(),
        new Running(),
        new Cycling(),
        new Rowing(),
        new JumpRope()
    };
}