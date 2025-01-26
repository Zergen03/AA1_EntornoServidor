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
        Console.WriteLine("2) View task");
        Console.WriteLine("3) Add task");
        Console.WriteLine("4) Delete task");
        Console.WriteLine("5) Complete task");
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
                    try
                    {
                        Console.WriteLine("Tasks:\n-------------------");
                        Dictionary<int, string> tasks = _usersService.GetTasks(user.Id);
                        foreach (var _task in tasks)
                        {
                            Console.WriteLine($"{_task.Key} - {_task.Value}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error retrieving tasks: {ex.Message}");
                    }
                    break;
                case 2:
                //wip - view task
                    // try
                    // {
                    //     Console.WriteLine("Select task to view");
                    //     if (!int.TryParse(Console.ReadLine(), out taskId))
                    //     {
                    //         Console.WriteLine("Invalid option");
                    //     }
                    //     AA1.Models.Task task = _taskService.GetTask(taskId);
                    //     Console.WriteLine($"Task: {task.Title}");
                    //     Console.WriteLine($"Description: {task.Description}");
                    //     Console.WriteLine($"Difficulty: {task.Difficulty}");
                    //     Console.WriteLine($"Expiration date: {task.ExpirationDate}");
                    // }
                    // catch (Exception ex)
                    // {
                    //     Console.WriteLine($"Error viewing task: {ex.Message}");
                    // }
                    break;
                case 3:
                    try
                    {
                        Console.WriteLine("Enter task title");
                        string? title = Console.ReadLine();
                        if (string.IsNullOrEmpty(title))
                        {
                            Console.WriteLine("Invalid name");
                        }
                        Console.WriteLine("Enter task description");
                        string? description = Console.ReadLine() ?? "";
                        Console.WriteLine("Enter task difficulty (1-5)");
                        int difficulty;
                        if (!int.TryParse(Console.ReadLine(), out difficulty))
                        {
                            Console.WriteLine("Invalid difficulty");
                        }
                        Console.WriteLine("Enter task expiration date (yyyy-MM-dd)");
                        string? expirationDateString = Console.ReadLine();
                        DateTime? expirationDate = null;
                        if (!string.IsNullOrEmpty(expirationDateString))
                        {
                            if (DateTime.TryParse(expirationDateString, out DateTime parsedDate))
                            {
                                expirationDate = parsedDate;
                            }
                            else
                            {
                                throw new Exception("Invalid date");
                            }
                        }
                        AA1.Models.Task task = _taskService.CreateTask(title, description, difficulty, expirationDate);
                        _usersService.AddTask(user.Id, task);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating task: {ex.Message}");
                    }
                    break;
                case 4:
                    try
                    {
                        Console.WriteLine("Select task to delete");
                        if (!int.TryParse(Console.ReadLine(), out taskId))
                        {
                            Console.WriteLine("Invalid option");
                        }
                        _usersService.DeleteTask(user.Id, taskId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting task: {ex.Message}");
                    }
                    break;
                case 5:
                    try
                    {
                        Console.WriteLine("Select task to complete");
                        if (!int.TryParse(Console.ReadLine(), out taskId))
                        {
                            Console.WriteLine("Invalid option");
                        }
                        AA1.Models.Task task = _taskService.CompleteTask(taskId);
                        _usersService.GainXp(user.Id, task.Xp);
                        //wip - add gold
                        Console.WriteLine($"Task completed: {task.Title}");
                        Console.WriteLine($"XP gained: {task.Xp}");
                        Console.WriteLine($"Gold gained: {task.Gold}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error completing task: {ex.Message}");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        } while (int.TryParse(Console.ReadLine(), out option) && option != 0);
    }

}