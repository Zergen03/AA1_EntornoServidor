using AA1.Models;
using AA1.Data;
using AA1.Services;

namespace AA1.Presentation;

public class AdminMenu
{
    private readonly IUsersRepository _usersRepository;
    private readonly IUsersService _usersService;
    private readonly IItemsService _itemsService;
    private readonly ITaskService _taskService;

    public AdminMenu(IUsersRepository usersRepository, IUsersService usersService, IItemsService itemsService, ITaskService taskService)
    {
        _usersRepository = usersRepository;
        _usersService = usersService;
        _itemsService = itemsService;
        _taskService = taskService;
    }

    private void ManinMenu()
    {
        Console.WriteLine("1) Create item");
        Console.WriteLine("2) Delete item");
        Console.WriteLine("3) Show items");
        Console.WriteLine("4) Delete user");
        Console.WriteLine("0) Exit");
    }

    private void ShowMenu()
    {
        int option;
        do
        {
            ManinMenu();
            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Invalid option");
            }
            List<string> types = new List<string> { "Weapon", "Helmet", "Chestplate", "Boots", "Gloves", "Leggins" };
            List<string> modifiers = new List<string> { "MaxHP", "XPreward", "GoldReward", "Heal" };

            switch (option)
            {
                case 1: // Create item
                    try
                    {
                        Console.WriteLine("Name: ");
                        string name = Console.ReadLine() ?? throw new ArgumentException("Name is required");
                        Console.WriteLine("Value: ");
                        if (!int.TryParse(Console.ReadLine(), out int value) || value <= 0)
                        {
                            throw new ArgumentException("Value must be a positive number");
                        }
                        Console.WriteLine("Type: \n(Weapon, Helmet, Chestplate, Boots, Gloves, Leggins)");
                        string type = Console.ReadLine();
                        if (!types.Contains(type))
                        {
                            throw new ArgumentException("Type not found");
                        }
                        Console.WriteLine("Modifiers: \n(MaxHP, XPreward, GoldReward, Heal)\n 0 to finish");
                        Dictionary<string, int> stats = new Dictionary<string, int>();
                        do
                        {
                            Console.WriteLine("Modifier: ");
                            string modifier = Console.ReadLine();
                            if (modifier == "0")
                            {
                                break;
                            }
                            if (!modifiers.Contains(modifier))
                            {
                                throw new ArgumentException("Modifier not found");
                            }
                            Console.WriteLine("Value: ");
                            int.TryParse(Console.ReadLine(), out int modifierValue);
                        } while (true);
                        _itemsService.CreateItem(name, value, type, stats);
                    }
                    catch (System.Exception e)
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }
                    break;
                case 2: // Delete item
                    try
                    {
                        Console.WriteLine("Id: ");
                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            throw new ArgumentException("Invalid id");
                        }
                        _itemsService.DeleteItem(id);
                    }
                    catch (System.Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
                case 3: // Show items
                    Dictionary<int, Items> items = _itemsService.GetItems();
                    foreach (Items item in items.Values)
                    {
                        Console.WriteLine(item.ToString());
                    }
                    break;
                case 4: // Delete user
                    try
                    {
                        Console.WriteLine("Id: ");
                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            throw new ArgumentException("Invalid id");
                        }
                        _usersService.DeleteUser(id);
                    }
                    catch (System.Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
                case 0:
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        } while (option != 0);

    }
}