using AA1.Models;
using System.Text.Json;

namespace AA1.Data;

class UsersRepository : IUsersRepository
{

    private Dictionary<int, Users> _users = new Dictionary<int, Users>();
    private readonly string _filePath = Environment.GetEnvironmentVariable("USERS_JSON_PATH") ?? "../../../ddbb/Users.json";


    public List<Users> GetUsers()
    {
        return _users.Values.ToList();
    }

    public Users GetUserById(int id)
    {
        return _users[id];
    }

    public Users CreateUser(string name, string password)
    {
        Users user = new Users(name, password);
        _users.Add(user.Id, user);
        return user;
    }

    public Users UpdateUser(int id, Users user)
    {
        _users[id] = user;
        return user;
    }

    public Users DeleteUser(int id)
    {
        Users user = _users[id];
        _users.Remove(id);
        return user;
    }
    public void LoadUsers()
    {
        throw new System.NotImplementedException();
    }
    public void SaveChanges()
    {
        var UsersToSerialize = _users.Values.ToList();
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(UsersToSerialize, options);
        File.WriteAllText(_filePath, jsonString);

    }
}