using AA1.Models;

namespace AA1.Services;

public interface IItemsService
{
    void ShowShop();
    void filterItemsByType(string type);
    void filterItemsByStat(string stat);
}
