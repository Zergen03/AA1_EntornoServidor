using AA1.Models;
using Data;
using System.Text.Json;

namespace AA1.Data;

public class ItemsRepository : IItemsRepository
{
    private Dictionary<int, Items> _items = new Dictionary<int, Items>();
    private readonly string _filePath;

    public ItemsRepository()
    {
        _filePath = Config.Get("ITEMS_JSON_PATH");
        if (string.IsNullOrEmpty(_filePath))
        {
            throw new Exception("Environment variable 'ITEMS_JSON_PATH' not found or empty.");
        }
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

    public Items CreateItem(Items item)
    {
        _items.Add(item.Id, item);
        SaveChanges();
        return item;
    }

    public Items UpdateItem(int id, Items item)
    {
        _items[id] = item;
        SaveChanges();
        return item;
    }

    public Items DeleteItem(int id)
    {
        Items item = _items[id];
        _items.Remove(id);
        SaveChanges();
        return item;
    }

    public void LoadItems()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                throw new Exception("File not found");
            }
            string jsonString = File.ReadAllText(_filePath);
            if (string.IsNullOrEmpty(jsonString))
            {
                return;
            }
            var itemsToDeserialize = JsonSerializer.Deserialize<List<AA1.Models.Items>>(jsonString);
            if (itemsToDeserialize != null)
            {
                _items = itemsToDeserialize.ToDictionary(i => i.Id);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading items: {ex.Message}");
        }
    }

    public void SaveChanges()
    {
        var ItemsToSerialize = _items.Values.ToList();
        var options = new JsonSerializerOptions { WriteIndented = true };
        string itemsJson = JsonSerializer.Serialize(ItemsToSerialize, options);
        File.WriteAllText(_filePath, itemsJson);
    }
}