using AA1.Models;

namespace AA1.Data;

public interface IUsersRepository
{
    List<Users> GetUsers();
    Users GetUserById(int id);
    Users CreateUser(string name, string password);
    Users UpdateUser(int id, Users user);
    Users DeleteUser(int id);
    void LoadUsers();
    void SaveChanges();
}