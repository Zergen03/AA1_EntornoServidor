using AA1.Models;

namespace AA1.Data;

public interface IUsersRepository
{
    List<Users> GetUsers();
    Users GetUserById(int id);
    Users GetUserByName(string name);
    Users CreateUser(Users user);
    Users UpdateUser(int id, Users user);
    Users DeleteUser(int id);
    void LoadUsers();
    void SaveChanges();
}