using Fitness_Tracker.Models;


namespace Fitness_Tracker.Services; 

public enum LoginStatus { Success, Failed, LockedOut}

public record LoginResult(LoginStatus Status, User? User, string Message);

/// <summary>
/// Handles registration and login.After MaxFailedAttempts wrongs passwords
/// in a row, the username if loxked for LochoutDuration. 
/// </summary>


public class AuthService
{
    public const int MaxFailedAttempts = 3;
    public static readonly TimeSpan LockoutDuration = TimeSpan.FromSeconds(60);

    private class AttemptInfo
    {
        public int Failures;
        public DateTime? LockedUntil;
    }

    private readonly UserRepository _repository;
    private readonly Dictionary<string, AttemptInfo> _attempts = new();
    public AuthService(UserRepository repository)
    {
        _repository = repository;
    }


    /// Creates an account.Returns an error message, or null on success.
    
    public string? Register(string username, string password, string confirmPassword)
    {
        string? error = CredentialValidator.ValidateUsername(username) ?? CredentialValidator.ValidatePassword(password);
        if(error != null)
        {
            return error;
        }

        if(password != confirmPassword)
        {
            return "The passwords do not match.";
        }

        if (_repository.Exists(username))
        {
            return "This username is already taken. Please choose another one ";
        }

        string salt = PasswordHasher.GenerateSalt();
        var user = new User(username, PasswordHasher.Hash(password, salt), salt);


        return _repository.Add(user)
            ? null
            : "The account could not be saved to disk. Please check your folder permission.";
                        

    }

    public LoginResult Login(string username, string password)
    {
        string? error = CredentialValidator.ValidateUsername(username);
        if(error != null)
        {
            return new LoginResult(LoginStatus.Failed, null, error);


        }

        if (string.IsNullOrEmpty(password))
        {
            return new LoginResult(LoginStatus.Failed, null, "Please enter your password");

        }

        string key = username.ToLowerInvariant();
        if(!_attempts.TryGetValue(key, out AttemptInfo? info))
        {
            info = new AttemptInfo();
            _attempts[key] = info;
        }

        if (info.LockedUntil.HasValue)
        {
            if(info.LockedUntil.Value > DateTime.Now)
            {
                return LockedMessage(info.LockedUntil.Value);
            }

            info.Failures = 0; // lock has expired
            info.LockedUntil = null;
        }

        User? user = _repository.Find(username);
        bool passwordOk = user != null && PasswordHasher.Verify(password, user.Salt, user.PasswordHash);

        if (passwordOk)
        {
            info.Failures = 0;
            return new LoginResult(LoginStatus.Success, user, "Login Sucessful.");



        }

        info.Failures++;
        if(info.Failures >= MaxFailedAttempts)
        {
            info.LockedUntil = DateTime.Now + LockoutDuration;
            return LockedMessage(info.LockedUntil.Value);
        }

        // same message for "Unknown user" and "wrong password", on purpose. 

        int left = MaxFailedAttempts - info.Failures;
        return new LoginResult(LoginStatus.Failed, null,
            $"Incorrect username or password.{left} attempt(s) left before a temporary lock.");

        
    }

    /// Saves the calories goal for a user. Returns an error message, or null on success. 
    public string? SaveGoal(User user, int goal)
    {
        string? error = user.SetCalorieGoal(goal);
        if(error != null)
        {
            return error;

        }
        return _repository.Save() ? null : "The goal could not be saved to disk.";
    }
   
        

        private static LoginResult LockedMessage(DateTime lockedUntil)
    {
        int seconds = (int)Math.Ceiling((lockedUntil - DateTime.Now).TotalSeconds);
        return new LoginResult(LoginStatus.LockedOut, null,
            $"Too many failed attempts.This account is locked. Try again in {seconds} second(s). ");

    }
}

