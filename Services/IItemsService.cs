using AA1.Models;

namespace AA1.Services;

public interface IItemsService
{
    void CreateItem(string _name, int _value, string _type, Dictionary<string, int> _stats);
    Dictionary<int, Items> GetItems();
    Items GetItemById(int id);
    void DeleteItem(int id);
    Dictionary<int, Items> filterItemsByType(string type);
    Dictionary<int, Items> filterItemsByStat(string stat);
}
