using AA1.Models;

namespace AA1.Data
{
    public interface IItemsRepository
    {
        List<Items> GetItems();
        Items GetItemById(int id);
        Items CreateItem(string _name, int _value, Dictionary<string, int> _stats, string _type);
        Items UpdateItem(int id, Items item);
        Items DeleteItem(int id);
        List<Items> ListItems();
        void LoadItems();
        void SaveChanges();
    }
}