using AA1.Models;
using System.Text.Json;

namespace AA1.Data;

public class ItemsRepository : IItemsRepository
{
    private Dictionary<int, Items> _items = new Dictionary<int, Items>();
    private readonly string _filePath = Environment.GetEnvironmentVariable("USERS_JSON_PATH") ?? "ddbb/Items.json";

    public ItemsRepository()
    {
        LoadItems();
    }

    public List<Items> GetItems()
    {
        return _items.Values.ToList();
    }

    public Items GetItemById(int id)
    {
        if (!_items.ContainsKey(id))
        {
            throw new System.Exception("Item not found");
        }
            return _items[id];
    }

    public Items CreateItem(string _name, int _value, Dictionary<string, int> _stats, string _type)
    {
        Items item = new Items(_name, _value, _stats, _type);
        _items.Add(item.Id, item);
        return item;
    }

    public Items UpdateItem(int id, Items item)
    {
        _items[id] = item;
        return item;
    }

    public Items DeleteItem(int id)
    {
        Items item = _items[id];
        _items.Remove(id);
        return item;
    }

    public List<Items> ListItems()
    {
        return _items.Values.ToList();
    }

    public void LoadItems()
    {
        if (!File.Exists(_filePath))
        {
            return;
        }
        string itemsJson = File.ReadAllText(_filePath);
        var ItemsToDeserialize = JsonSerializer.Deserialize<List<Items>>(itemsJson);
        _items = ItemsToDeserialize.ToDictionary(i => i.Id);
    }

    public void SaveChanges()
    {
        var ItemsToSerialize = _items.Values.ToList();
        var options = new JsonSerializerOptions { WriteIndented = true };
        string itemsJson = JsonSerializer.Serialize(ItemsToSerialize, options);
        File.WriteAllText(_filePath, itemsJson);
    }
}