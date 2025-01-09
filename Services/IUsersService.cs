using AA1.Models;

namespace AA1.Services;

public interface IUsersService
{
    Users AddTask(int id, AA1.Models.Task task);
    Users DeleteTask(int id, int taskId);
    Users CompleteTask(int userId, int taskId);
    Users AddItem(int id, Items item);
    Users DeleteItem(int id, int itemId);
    List<Items> GetInventory(int id);
    List<Items> GetEquippedItems(int id);
    Users EquipItem(int id, int itemId);
    Users UnEquipItem(int id, int itemId);
    Users TakeDamage(int id, int damage);
    Users Heal(int id, int heal);
    Users levelUp(int id);
}