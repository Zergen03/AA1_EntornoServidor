using AA1.Models;
using AA1.Data;
using AA1.Services;
using AA1.DTOs;
using System;
using System.Collections;
using System.IO;
using Data;
using NLog;


namespace AA1.Presentation;
public class Menu
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private readonly Dictionary<string, string> admin = new Dictionary<string, string>();
    private readonly IUsersRepository _usersRepository;
    private readonly IUsersService _usersService;
    private readonly IItemsService _itemsService;
    private readonly ITaskService _taskService;
    private readonly AdminMenu _adminMenu;

    public Menu(IUsersRepository usersRepository, IUsersService usersService, IItemsService itemsService, ITaskService taskService, AdminMenu adminMenu)
    {
        _usersRepository = usersRepository;
        _usersService = usersService;
        _itemsService = itemsService;
        _taskService = taskService;
        _adminMenu = adminMenu;
        admin.Add(Config.Get("ADMIN_NAME"), Config.Get("ADMIN_PASSWORD"));
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
        Console.WriteLine("4) Go to Shop");
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
    private void showShopMenu()
    {
        Console.WriteLine("1) Show Shop");
        Console.WriteLine("2) View item");
        Console.WriteLine("3) Buy item");
        Console.WriteLine("0) Exit");
    }
    private void filterMenu()
    {
        Console.WriteLine("1) Show All");
        Console.WriteLine("2) Filter by type");
        Console.WriteLine("3) Filter by stat");
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
            if (admin.ContainsKey(name) && admin[name] == password)
            {
                _adminMenu.ShowAdminMenu();
                return;
            }
            else
            {
                LoginResultDTO userDTO = _usersService.Login(name, password);
                MenuUser(userDTO);
            }
        }
        catch (System.Exception ex)
        {
            logger.Warn(ex.Message);
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
            if (admin.ContainsKey(name))
            {
                throw new Exception("Not allowed to register as 'admin'");
            }
            LoginResultDTO userDTO = _usersService.Register(name, password);
            MenuUser(userDTO);
        }
        catch (System.Exception ex)
        {
            logger.Warn(ex.Message);
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
                case 4:
                    ShopMenu(userDTO);
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
                        logger.Warn(ex.Message);
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
                        logger.Warn(ex.Message);
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
                        logger.Warn(ex.Message);
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
                        logger.Warn(ex.Message);
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
                        logger.Warn(ex.Message);
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
                        logger.Warn(ex.Message);
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
                        logger.Warn(ex.Message);
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
                        logger.Warn(ex.Message);
                        Console.WriteLine($"Error completing task: {ex.Message}");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        } while (option != 0);
    }

    public void ShopMenu(LoginResultDTO userDTO)
    {
        int option;
        Dictionary<int, int> itemMapping = new Dictionary<int, int>();
        Dictionary<int, Items> itemsToMap = _itemsService.GetItems();
        int index = 1;
        foreach (var item in itemsToMap)
        {
            itemMapping.Add(index, item.Key);
            index++;
        }
        do
        {
            showShopMenu();
            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Invalid option");
                option = -1;
                continue;
            }
            switch (option)
            {
                case 1: // Show shop
                    try
                    {
                        filterMenu();
                        if (!int.TryParse(Console.ReadLine(), out int filterOption))
                        {
                            Console.WriteLine("Invalid option");
                            filterOption = -1;
                            continue;
                        }
                        List<string> types = new List<string> { "Weapon", "Helmet", "Chestplate", "Boots", "Gloves", "Leggins" };
                        List<string> modifiers = new List<string> { "MaxHP", "XPreward", "GoldReward", "Heal" };
                        Console.WriteLine("Items:\n-------------------");
                        switch (filterOption)
                        {
                            case 1: // Show all
                                try
                                {
                                    Dictionary<int, Items> shopItems = _itemsService.GetItems();
                                    index = 1;
                                    foreach (var item in shopItems)
                                    {
                                        Console.WriteLine($"{index} - {item.Value.Name}");
                                        index++;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.Warn(ex.Message);
                                    Console.WriteLine($"Error retrieving items: {ex.Message}");
                                    continue;
                                }
                                break;
                            case 2: // Filter by type
                                try
                                {
                                    Console.WriteLine("Enter type to filter\n(Weapon, Helmet, Chestplate, Boots, Gloves, Leggins)");
                                    string type = Console.ReadLine();
                                    if (!types.Contains(type))
                                    {
                                        throw new ArgumentException("Type not found");
                                    }
                                    Dictionary<int, Items> filterItems = _itemsService.filterItemsByType(type);
                                    foreach (var item in filterItems)
                                    {
                                        if (itemMapping.ContainsValue(item.Key))
                                        {
                                            int key = itemMapping.FirstOrDefault(i => i.Value == item.Key).Key;
                                            Console.WriteLine($"{key} - {item.Value.Name}");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.Warn(ex.Message);
                                    Console.WriteLine($"Error retrieving items: {ex.Message}");
                                    continue;
                                }
                                break;
                            case 3: // Filter by stat
                                try
                                {
                                    Console.WriteLine("Enter stat to filter\n(MaxHP, XpReward, GoldReward, Heal)");
                                    string stat = Console.ReadLine();
                                    if (!modifiers.Contains(stat))
                                    {
                                        throw new ArgumentException("Type not found");
                                    }
                                    Dictionary<int, Items> filterItems = _itemsService.filterItemsByStat(stat);
                                    foreach (var item in filterItems)
                                    {
                                        if (itemMapping.ContainsValue(item.Key))
                                        {
                                            int key = itemMapping.FirstOrDefault(i => i.Value == item.Key).Key;
                                            Console.WriteLine($"{key} - {item.Value.Name}");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.Warn(ex.Message);
                                    Console.WriteLine($"Error retrieving items: {ex.Message}");
                                    continue;
                                }
                                break;
                            default:
                                Console.WriteLine("Invalid option");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex.Message);
                        Console.WriteLine($"Error retrieving items: {ex.Message}");
                        continue;
                    }
                    break;
                case 2: // View item
                    try
                    {
                        Console.WriteLine("Select item to view");
                        if (!int.TryParse(Console.ReadLine(), out int itemId))
                        {
                            Console.WriteLine("Invalid option");
                        }
                        int mappedItemId = itemMapping[itemId];
                        Items item = _itemsService.GetItemById(mappedItemId);
                        Console.WriteLine(item.ToString());
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex.Message);
                        Console.WriteLine($"Error viewing item: {ex.Message}");
                        continue;
                    }
                    break;
                case 3: // Buy item
                    try
                    {
                        Console.WriteLine("Select item to buy");
                        if (!int.TryParse(Console.ReadLine(), out int itemId))
                        {
                            Console.WriteLine("Invalid option");
                        }
                        int mappedItemId = itemMapping[itemId];
                        Items item = _itemsService.GetItemById(mappedItemId);
                        _usersService.BuyItem(userDTO.User.Id, item);
                        Console.WriteLine($"Item bought: {item.Name}");
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex.Message);
                        Console.WriteLine($"Error buying item: {ex.Message}");
                        continue;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        } while (option != 0);
    }

}