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

    public void ShowShop()
    {
        foreach (Items item in _itemsRepository.GetItems())
        {
            Console.WriteLine(item.ToString());
        }
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