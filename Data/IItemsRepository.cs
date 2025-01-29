using AA1.Models;

namespace AA1.Data
{
    public interface IItemsRepository
    {
        List<Items> GetItems();
        Items GetItemById(int id);
        Items CreateItem(Items item);
        Items UpdateItem(int id, Items item);
        Items DeleteItem(int id);
        void LoadItems();
        void SaveChanges();
    }
}