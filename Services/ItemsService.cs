using AA1.Models;
using AA1.Data;

namespace AA1.Services;

public class ItemsService : IItemsService
{
    private readonly IItemsRepository _itemsRepository;

    public ItemsService(IItemsRepository itemsRepository)
    {
        _itemsRepository = itemsRepository;
    }

    public void CreateItem(string _name, int _value, string _type, Dictionary<string, int> _stats)
    {
        try
        {
            Items item = new Items(_name, _value, _stats, _type);
            _itemsRepository.CreateItem(item);
        }
        catch (System.Exception e)
        {
            throw new System.Exception($"There was an error creating the item: {e.Message}");
        }
    }

    public Dictionary<int, Items> GetItems()
    {
        List<Items> items = _itemsRepository.GetItems();
        if (items == null || items.Count == 0)
        {
            throw new System.Exception("No items found");
        }
        Dictionary<int, Items> itemsDict = new Dictionary<int, Items>();
        foreach (Items item in items)
        {
            itemsDict.Add(item.Id, item);
        }
        return itemsDict;
    }

    public Items GetItemById(int id)
    {
        if (id < 0)
        {
            throw new System.Exception("Invalid id");
        }
        Items item = _itemsRepository.GetItemById(id);
        if (item == null)
        {
            throw new System.Exception("Item not found");
        }
        return item;
    }

    public void DeleteItem(int id)
    {
        if (_itemsRepository.GetItemById(id) == null)
        {
            throw new System.Exception("Item not found");
        }
        _itemsRepository.DeleteItem(id);
    }

    public void filterItemsByType(string type)
    {
        foreach (Items item in _itemsRepository.GetItems())
        {
            if (item.Type == type)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }

    public void filterItemsByStat(string stat)
    {
        foreach (Items item in _itemsRepository.GetItems())
        {
            if (item.Stats.ContainsKey(stat))
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}