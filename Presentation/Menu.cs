using AA1.Models;
using AA1.Data;
using AA1.Services;
using AA1.DTOs;

namespace AA1.Presentation;
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
                option = -1;
                continue;
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
        } while (option != 0);
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
            LoginResultDTO userDTO = _usersService.Login(name, password);
            MenuUser(userDTO);
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
            LoginResultDTO userDTO = _usersService.Register(name, password);
            MenuUser(userDTO);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            MainMenu();
        }
    }

    private void MenuUser(LoginResultDTO userDTO)
    {
        int option;
        do
        {
            ShowUserMenu();
            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Invalid option");
                option = -1;
                continue;
            }

            switch (option)
            {
                case 1:
                    InventoryMenu(userDTO);
                    break;
                case 2:
                    TaskMenu(userDTO);
                    break;
                case 3:
                    userDTO.ToString();
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        } while (option != 0);
    }

    private void InventoryMenu(LoginResultDTO userDTO)
    {
        int option;
        int itemId;
        do
        {
            ShowInventoryMenu();
            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Invalid option");
                option = -1;
                continue;
            }
            switch (option)
            {
                case 1:
                    try
                    {
                        Console.WriteLine("Unequiped items:\n-------------------");
                        Dictionary<int, string> inventory = _usersService.GetInventory(userDTO.User.Id);
                        foreach (var item in inventory)
                        {
                            Console.WriteLine($"{item.Key} - {item.Value}");
                        }
                        Console.WriteLine("Equiped items:\n-------------------");
                        Dictionary<int, string> equippedItems = _usersService.GetEquippedItems(userDTO.User.Id);
                        foreach (var item in equippedItems)
                        {
                            Console.WriteLine($"{item.Key} - {item.Value}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error retrieving inventory: {ex.Message}");
                    }
                    break;
                case 2:
                    try
                    {
                        Console.WriteLine("Select item to equip");
                        if (!int.TryParse(Console.ReadLine(), out itemId))
                        {
                            Console.WriteLine("Invalid option");
                            continue;
                        }
                        _usersService.EquipItem(userDTO.User.Id, itemId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error equipping item: {ex.Message}");
                    }
                    break;
                case 3:
                    try
                    {
                        Console.WriteLine("Select item to unequip");
                        if (!int.TryParse(Console.ReadLine(), out itemId))
                        {
                            Console.WriteLine("Invalid option");
                        }
                        _usersService.UnEquipItem(userDTO.User.Id, itemId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error unequipping item: {ex.Message}");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        } while (option != 0);
    }

    private void TaskMenu(LoginResultDTO userDTO)
    {
        int option;
        int taskId;
        Dictionary<int, int> taskMapping = new Dictionary<int, int>();
        int index = 1;
        foreach (var task in userDTO.User.Tasks)
        {
            taskMapping.Add(index, task.Key);
            index++;
        }
        do
        {
            ShowTaskMenu();
            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Invalid option");
                option = -1;
                continue;
            }
            Console.WriteLine("_________________________");

            switch (option)
            {
                case 1: // Show tasks
                    try
                    {
                        Console.WriteLine("Tasks:\n-------------------");
                        Dictionary<int, string> tasks = _usersService.GetTasks(userDTO.User.Id);
                        index = 1;
                        foreach (var task in tasks)
                        {
                            Console.WriteLine($"{index} - {task.Value}");
                            index++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error retrieving tasks: {ex.Message}");
                        continue;
                    }
                    break;
                case 2: // View task
                    try
                    {
                        Console.WriteLine("Select task to view");
                        if (!int.TryParse(Console.ReadLine(), out taskId))
                        {
                            Console.WriteLine("Invalid option");
                        }
                        Dictionary<int, string> tasks = _usersService.GetTasks(userDTO.User.Id);
                        int mappedTaskId = taskMapping[taskId];
                        AA1.Models.Task task = _taskService.GetTaskById(mappedTaskId);
                        Console.WriteLine(task.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error viewing task: {ex.Message}");
                    }
                    break;
                case 3: // Add task
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
                        _usersService.AddTask(userDTO.User.Id, task);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating task: {ex.Message}");
                    }
                    break;
                case 4: // Delete task
                    try
                    {
                        Console.WriteLine("Select task to delete");
                        if (!int.TryParse(Console.ReadLine(), out taskId))
                        {
                            Console.WriteLine("Invalid option");
                        }
                        _usersService.DeleteTask(userDTO.User.Id, taskId);
                        _taskService.DeleteTask(taskId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting task: {ex.Message}");
                    }
                    break;
                case 5: // Complete task
                    try
                    {
                        Console.WriteLine("Select task to complete");
                        if (!int.TryParse(Console.ReadLine(), out taskId))
                        {
                            Console.WriteLine("Invalid option");
                        }
                        AA1.Models.Task task = _taskService.CompleteTask(taskId);
                        _usersService.GainXp(userDTO.User.Id, task.Xp);
                        Users fasjfa = _usersService.GainGold(userDTO.User.Id, task.Gold);
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
        } while (option != 0);
    }

}