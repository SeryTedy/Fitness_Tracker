using System.Text.Json.Serialization; 
namespace Fitness_Tracker.Models;

///<summary>
///A registered user. Only a PBKDF2 hash of the password is kept. 
///The calorie goal is stored with the user so it is remebered between sessions. 
///</summary>


public class User
{
    public const int MinGoal = 1; 
    public const int MaxGoal = 100000; 

    public string Username { get; }
    public string PasswordHash { get; }
    public string Salt { get; }


    /// Calories the user wants to burn. 0 means " no goal set yet"
    public int CalorieGoal { get; private set; }

    [JsonConstructor]
    public User (string username, string passwordHash, string salt, int calorieGoal = 0)
    {
        Username = username;
        PasswordHash = passwordHash;
        Salt = salt;
        CalorieGoal = calorieGoal;
    }


    /// sets the goal. Return an error message, or null if the goal is valid.
    
    public string? SetCalorieGoal(int goal)
    {
        if(goal < MinGoal || goal > MaxGoal)
        {
            return $"The goal must be between {MinGoal} and {MaxGoal:NO} calories.";


        }

        CalorieGoal = goal;
        return null;
    }
}
