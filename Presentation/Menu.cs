using AA1.Models;
using AA1.Data;
using AA1.Services;

namespace AA1.Menu;
public class Menu
{

    private readonly IUsersRepository _usersRepository;
    private readonly IUsersService _usersService;
    private readonly IItemsService _itemsService;
    private readonly ITaskService _taskService;

    public Menu(IUsersRepository usersRepository, IUsersService usersService, IItemsService itemsService, ITaskService taskService)
    {
        _usersRepository = usersRepository;
        _usersService = usersService;
        _itemsService = itemsService;
        _taskService = taskService;
    }


    private void ShowMainMenu()
    {
        Console.WriteLine("1) Login");
        Console.WriteLine("2) Register");
        Console.WriteLine("0) Exit");
    }

    private void ShowUserMenu()
    {
        Console.WriteLine("1) Inventory");
        Console.WriteLine("2) Tasks");
        Console.WriteLine("3) Stats");
        Console.WriteLine("0) Exit");
    }

    private void ShowInventoryMenu()
    {
        Console.WriteLine("1) Show inventory");
        Console.WriteLine("2) Equip item");
        Console.WriteLine("3) Unequip item");
        Console.WriteLine("0) Exit");
    }

    private void ShowTaskMenu()
    {
        Console.WriteLine("1) Show tasks");
        Console.WriteLine("2) Add task");
        Console.WriteLine("3) Delete task");
        Console.WriteLine("4) Complete task");
        Console.WriteLine("0) Exit");
    }

    public void MainMenu()
    {
        int option;
        do
        {
            ShowMainMenu();
            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Invalid option");
            }

            switch (option)
            {
                case 1:
                    Login();
                    break;
                case 2:
                    Register();
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        } while (int.TryParse(Console.ReadLine(), out option) && option != 0);
    }

    private void Login()
    {
        Console.WriteLine("Enter your name");
        string? name = Console.ReadLine();
        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Invalid name");
            return;
        }
        Console.WriteLine("Enter your password");
        string? password = Console.ReadLine();
        if (string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Invalid password");
            return;
        }

        try
        {
            Users user = _usersService.Login(name, password);
            MenuUser(user);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            MainMenu();
        }
    }

    private void Register()
    {
        Console.WriteLine("Enter your name");
        string? name = Console.ReadLine();
        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Invalid name");
            return;
        }
        Console.WriteLine("Enter your password");
        string? password = Console.ReadLine();
        if (string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Invalid password");
            return;
        }

        try
        {
            Users user = _usersService.Register(name, password);
            MenuUser(user);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            MainMenu();
        }
    }

    private void MenuUser(Users user)
    {
        int option;
        do
        {
            ShowUserMenu();
            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Invalid option");
            }

            switch (option)
            {
                case 1:
                    InventoryMenu(user);
                    break;
                case 2:
                    TaskMenu(user);
                    break;
                case 3:
                    user.ToString();
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        } while (int.TryParse(Console.ReadLine(), out option) && option != 0);
    }

    private void InventoryMenu(Users user)
    {
        int option;
        int itemId;
        do
        {
            ShowInventoryMenu();
            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Invalid option");
            }
            switch (option)
            {
                case 1:
                    Console.WriteLine("Unequiped items:\n-------------------");
                    Dictionary<int, string> inventory = _usersService.GetInventory(user.Id);
                    foreach (var item in inventory)
                    {
                        Console.WriteLine($"{item.Key} - {item.Value}");
                    }
                    Console.WriteLine("Equiped items:\n-------------------");
                    Dictionary<int, string> equippedItems = _usersService.GetEquippedItems(user.Id);
                    foreach (var item in equippedItems)
                    {
                        Console.WriteLine($"{item.Key} - {item.Value}");
                    }
                    break;
                case 2:
                    Console.WriteLine("Select item to equip");
                    if (!int.TryParse(Console.ReadLine(), out itemId))
                    {
                        Console.WriteLine("Invalid option");
                    }
                    _usersService.EquipItem(user.Id, itemId);
                    break;
                case 3:
                    Console.WriteLine("Select item to unequip");
                    if (!int.TryParse(Console.ReadLine(), out itemId))
                    {
                        Console.WriteLine("Invalid option");
                    }
                    _usersService.UnEquipItem(user.Id, itemId);
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        } while (int.TryParse(Console.ReadLine(), out option) && option != 0);
    }

    private void TaskMenu(Users user)
    {
        int option;
        int taskId;
        do
        {
            ShowTaskMenu();
            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Invalid option");
            }

            switch (option)
            {
                case 1:
                    Console.WriteLine("Tasks:\n-------------------");
                    Dictionary<int, string> tasks = _usersService.GetTasks(user.Id);
                    foreach (var task in tasks)
                    {
                        Console.WriteLine($"{task.Key} - {task.Value}");
                    }
                    break;
                case 2:
                    Console.WriteLine("Enter task name");
                    

                    if (!int.TryParse(Console.ReadLine(), out taskId))
                    {
                        Console.WriteLine("Invalid option");
                    }

                    break;
                case 3:
                    Console.WriteLine("Delete task");
                    // _usersService.DeleteTask(user.Id, 1);
                    break;
                case 4:
                    Console.WriteLine("Complete task");
                    // _usersService.CompleteTask(user.Id, 1);
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        } while (int.TryParse(Console.ReadLine(), out option) && option != 0);
    }

}