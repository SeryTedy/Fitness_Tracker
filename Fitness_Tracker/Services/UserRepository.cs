

using Fitness_Tracker.Models;
using System.Text.Json;

namespace Fitness_Tracker.Services;

/// <summary> Stores users (and their saved goals) in a JSON file in the user's AppData folder.</summary> 

public class UserRepository
{
    private readonly string _filePath;
    private readonly Dictionary<string, User> _users = new(StringComparer.OrdinalIgnoreCase);

    public UserRepository(string? filePath = null)
    {
        _filePath = filePath ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Fitness_Tracker", "users.json");
        Load();
    }

    public bool Exists(string username) => _users.ContainsKey(username);
    public User? Find(string username) =>
        _users.TryGetValue(username, out User? user) ? user : null;
     

    /// Adds a user  and save the file.Returns falso if saving failed.
    public bool Add(User user)
    {
        _users[user.Username] = user;
        return Save();
    }

    /// Writes all users to disk. Returns false if the file could not be written
    public bool Save()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
            string json = JsonSerializer.Serialize(_users.Values.ToList(),
                new JsonSerializerOptions { WriteIndented = true});
            File.WriteAllText(_filePath, json);
            return true;

        }
        catch(Exception ex) when (ex is IOException or UnauthorizedAccessException){
            return false;
        }

    }

    private void Load()
    {
        try
        {
            if(!File.Exists(_filePath))
                    {
                return;


                }
            var users = JsonSerializer.Deserialize<List<User>>(File.ReadAllText(_filePath));
            foreach(User user in users ?? new List<User>())
            {
                _users[user.Username] = user;
            }
                    
        }

        catch (Exception ex ) when(ex is IOException or UnauthorizedAccessException or JsonException)
        {
            _users.Clear();
        }
    }

}
