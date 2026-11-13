namespace Fitness_Tracker.Services;

/// <summary> 
/// Checks usernames and passwords against the rules in the brief.
/// Each method returns an error message, or null when the value is valid. 
///  </summary>


public static class CredentialValidator
{

    public const int PasswordLength = 12;

    public static string? ValidateUsername(string? username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return "Please enter a username.";
        }

        if (!username.All(char.IsAsciiLetterOrDigit))
        {
            return "The username can only contain letters and numbers (no spaces or symbols).";

        }

        return null;
    }

    public static string? ValidatePassword(string? password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return "Please enter a password.";
        }

        if(password.Length != PasswordLength)
        {
            return $"The password must be exactly {PasswordLength} characters long yours has {password.Length}).";


        }

        if (!password.Any(char.IsLower))
        {

            return "The password must contain at least one lowercase letter.";

        }

        return null;
    }

}
