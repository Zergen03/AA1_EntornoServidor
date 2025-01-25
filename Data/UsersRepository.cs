using AA1.Models;

namespace AA1.Data;

class UsersRepository : IUsersRepository
{

    private Dictionary<int, Users> _users = new Dictionary<int, Users>();

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
        throw new System.NotImplementedException();
    }
    public void LoadUsers()
    {
        throw new System.NotImplementedException();
    }
    public void SaveChanges()
    {
        throw new System.NotImplementedException();
    }
}