using AA1.Models;
using AA1.Data;
using AA1.Services;
using AA1.DTOs;
using System;
using System.Collections;
using System.IO;
using Data;
using NLog;
using Spectre.Console;


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
        AnsiConsole.Clear();

        AnsiConsole.Write(
            new FigletText("Main Menu")
                .Centered()
                .Color(Color.Green));

        AnsiConsole.MarkupLine("[bold]Use arrow keys to navigate and [green]ENTER[/] to select.[/]");
    }

    private void ShowUserMenu()
    {
        Console.Clear();

        AnsiConsole.Write(
            new FigletText("User Menu")
                .Centered()
                .Color(Color.Blue));

        AnsiConsole.MarkupLine("[bold]Use arrow keys to navigate and [green]ENTER[/] to select.[/]");
    }


    private void ShowInventoryMenu()
    {
        Console.Clear();

        AnsiConsole.Write(
            new FigletText("Inventory")
                .Centered()
                .Color(Color.Blue));

        AnsiConsole.MarkupLine("[bold]Use arrow keys to navigate and [green]ENTER[/] to select.[/]");
    }

    private void ShowTaskMenu()
    {
        Console.Clear();

        AnsiConsole.Write(
            new FigletText("Tasks")
                .Centered()
                .Color(Color.Blue));

        AnsiConsole.MarkupLine("[bold]Use arrow keys to navigate and [green]ENTER[/] to select.[/]");

    }
    private void showShopMenu()
    {
        Console.Clear();

        AnsiConsole.Write(
            new FigletText("Shop")
                .Centered()
                .Color(Color.Blue));

        AnsiConsole.MarkupLine("[bold]Use arrow keys to navigate and [green]ENTER[/] to select.[/]");

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

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Select an option:[/]")
                    .AddChoices("1) Login", "2) Register", "0) Exit"));

            option = int.Parse(choice[0].ToString());

            switch (option)
            {
                case 1:
                    Login();
                    break;
                case 2:
                    Register();
                    break;
                case 0:
                    AnsiConsole.MarkupLine("[bold green]Exiting...[/]");
                    break;
                default:
                    AnsiConsole.MarkupLine("[bold red]Invalid option[/]");
                    break;
            }

        } while (option != 0);
    }
    private void Login()
    {
        AnsiConsole.Clear();

        AnsiConsole.MarkupLine("[bold yellow]--- Login ---[/]");

        string name = AnsiConsole.Prompt(
            new TextPrompt<string>("[bold]Enter your name:[/]"));

        string password = AnsiConsole.Prompt(
            new TextPrompt<string>("[bold]Enter your password:[/]")
                .Secret());

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

        AnsiConsole.MarkupLine("\n[bold]Press any key to return to the main menu...[/]");
        Console.ReadKey(true);

        MainMenu();
    }


    private void Register()
    {
        AnsiConsole.Clear();

        AnsiConsole.MarkupLine("[bold yellow]--- Register ---[/]");

        string name = AnsiConsole.Prompt(
            new TextPrompt<string>("[bold]Enter your name:[/]"));

        string password = AnsiConsole.Prompt(
            new TextPrompt<string>("[bold]Enter your password:[/]")
                .Secret());

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
            AnsiConsole.MarkupLine($"[bold red]{ex.Message}[/]");

            AnsiConsole.MarkupLine("\n[bold]Press any key to return to the main menu...[/]");
            Console.ReadKey(true);

            MainMenu();
        }
    }
    private void MenuUser(LoginResultDTO userDTO)
    {
        int option;
        do
        {
            ShowUserMenu();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]User Menu[/]")
                    .AddChoices("1) Inventory", "2) Tasks", "3) Profile", "4) Shop", "0) Logout"));

            option = int.Parse(choice[0].ToString());

            switch (option)
            {
                case 1:
                    InventoryMenu(userDTO);
                    break;
                case 2:
                    TaskMenu(userDTO);
                    break;
                case 3:
                    Table profileTable = new Table();
                    profileTable.AddColumn("Name");
                    profileTable.AddColumn("HP");
                    profileTable.AddColumn("XP");
                    profileTable.AddColumn("Level");
                    profileTable.AddColumn("Gold");

                    profileTable.AddRow(userDTO.User.Name, userDTO.User.life.ToString(), userDTO.User.xp.ToString(), userDTO.User.level.ToString(), userDTO.User.gold.ToString());
                    AnsiConsole.Write(new Panel(profileTable)
                        .Header("[bold cyan]Profile[/]")
                        .Border(BoxBorder.Rounded)
                        .Expand());

                    AnsiConsole.MarkupLine("\n[bold]Press any key to return...[/]");
                    Console.ReadKey(true);


                    break;
                case 4:
                    ShopMenu(userDTO);
                    break;
                case 0:
                    AnsiConsole.MarkupLine("[bold green]Logging out...[/]");
                    break;
                default:
                    AnsiConsole.MarkupLine("[bold red]Invalid option[/]");
                    break;
            }

        } while (option != 0);
    }
    private void InventoryMenu(LoginResultDTO userDTO)
    {
        while (true)
        {
            try
            {
                ShowInventoryMenu();
                Dictionary<int, string> inventory;
                try
                {
                    inventory = _usersService.GetInventory(userDTO.User.Id) ?? new Dictionary<int, string>();
                }
                catch (Exception ex)
                {
                    logger.Warn(ex.Message);
                    inventory = new Dictionary<int, string>();
                }

                Dictionary<int, string> equippedItems;
                try
                {
                    equippedItems = _usersService.GetEquippedItems(userDTO.User.Id);
                }
                catch (Exception ex)
                {
                    logger.Warn(ex.Message);
                    equippedItems = new Dictionary<int, string>();
                }

                if (inventory.Count == 0 && equippedItems.Count == 0)
                {
                    AnsiConsole.MarkupLine("[bold red]Your inventory is empty.[/]");
                    AnsiConsole.MarkupLine("[bold]Press any key to return...[/]");
                    Console.ReadKey(true);
                    break;
                }

                var menuItems = new List<string>();

                if (equippedItems.Count > 0)
                {
                    menuItems.Add("[bold green]--- Equipped Items ---[/]");
                    foreach (var item in equippedItems)
                    {
                        menuItems.Add($"[green]{item.Key + 1} - {item.Value}[/]");
                    }
                }

                if (inventory.Count > 0)
                {
                    menuItems.Add("[bold red]--- Unequipped Items ---[/]");
                    foreach (var item in inventory)
                    {
                        menuItems.Add($"[red]{item.Key + 1} - {item.Value}[/]");
                    }
                }

                menuItems.Add("[yellow]0 - Exit[/]");

                var selectedItem = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold yellow]Select an item to equip/unequip or view details:[/]")
                        .PageSize(10)
                        .AddChoices(menuItems));

                string cleanSelection = selectedItem.Split(" - ")[0]
                    .Replace("[red]", "")
                    .Replace("[green]", "")
                    .Replace("[yellow]", "")
                    .Replace("[/]", "")
                    .Trim();

                if (cleanSelection == "0" || !int.TryParse(cleanSelection, out int itemId))
                    break;

                itemId -= 1;

                Items itemDetails = _itemsService.GetItemById(itemId);

                var statsTable = new Table();
                statsTable.AddColumn("Attribute");
                statsTable.AddColumn("Value");

                statsTable.AddRow("Type", itemDetails.Type);

                foreach (var stat in itemDetails.Stats)
                {
                    statsTable.AddRow(stat.Key, stat.Value.ToString());
                }

                AnsiConsole.Write(new Panel(statsTable)
                    .Header($"[bold cyan]{itemDetails.Name}[/]")
                    .Border(BoxBorder.Rounded)
                    .Expand());

                var action = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold yellow]What do you want to do?[/]")
                        .AddChoices("Equip/Unequip", "Cancel"));

                if (action == "Equip/Unequip")
                {
                    if (equippedItems.ContainsKey(itemId))
                    {
                        _usersService.UnEquipItem(userDTO.User.Id, itemId);
                    }
                    else if (inventory.ContainsKey(itemId))
                    {
                        _usersService.EquipItem(userDTO.User.Id, itemId);
                    }
                }

                AnsiConsole.Clear();
            }
            catch (Exception ex)
            {
                logger.Warn(ex.Message);
                AnsiConsole.MarkupLine($"[bold red]{ex.Message}[/]");
                Console.ReadKey(true);
                continue;
            }
        }
    }
    private void TaskMenu(LoginResultDTO userDTO)
    {
        int option;
        do
        {
            ShowTaskMenu();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Task Menu[/]")
                    .AddChoices("1) Show tasks", "2) Add task", "0) Exit"));

            option = int.Parse(choice[0].ToString());

            switch (option)
            {
                case 1: // Show tasks
                    try
                    {
                        Dictionary<int, string> tasksIds = _usersService.GetTasks(userDTO.User.Id);
                        Dictionary<int, AA1.Models.Task> tasks = new Dictionary<int, AA1.Models.Task>();
                        foreach (var _task in tasksIds)
                        {
                            tasks.Add(_task.Key, _taskService.GetTaskById(_task.Key));
                        }

                        if (tasks.Count == 0)
                        {
                            AnsiConsole.MarkupLine("[bold red]No tasks available.[/]");
                            return;
                        }

                        var choices = tasks.Select(t => $"{t.Key + 1} - {t.Value.Title}").ToList();
                        choices.Add("[yellow]0 - Cancel[/]");

                        var selectedTaskString = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[bold yellow]Select a task to complete or delete:[/]")
                                .PageSize(10)
                                .AddChoices(choices));

                        if (selectedTaskString.Contains("0 - Cancel"))
                            return;

                        int taskId = int.Parse(selectedTaskString.Split(" - ")[0].Trim()) - 1;

                        var task = tasks[taskId];

                        var taskTable = new Table();
                        taskTable.AddColumn("Attribute");
                        taskTable.AddColumn("Value");

                        taskTable.AddRow("Title", task.Title);
                        taskTable.AddRow("Description", task.Description);
                        taskTable.AddRow("Difficulty", task.difficulty.ToString());
                        taskTable.AddRow("Expiration", task.ExpirationDate?.ToString("yyyy-MM-dd") ?? "No expiration");

                        AnsiConsole.Write(new Panel(taskTable)
                            .Header($"[bold cyan]{task.Title}[/]")
                            .Border(BoxBorder.Rounded)
                            .Expand());

                        var action = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[bold yellow]What do you want to do?[/]")
                                .AddChoices("Complete", "Delete", "Cancel"));

                        if (action == "Complete")
                        {
                            _usersService.GainXp(userDTO.User.Id, task.Xp);
                            _usersService.GainGold(userDTO.User.Id, task.Gold);
                            _taskService.CompleteTask(taskId);
                            _usersService.DeleteTask(userDTO.User.Id, taskId);
                            AnsiConsole.MarkupLine($"[bold green]Task completed![/]");
                            AnsiConsole.MarkupLine($"[bold blue]XP Gained:[/] {task.Xp}");
                            AnsiConsole.MarkupLine($"[bold yellow]Gold Gained:[/] {task.Gold}");
                            Console.ReadKey(true);

                        }
                        else if (action == "Delete")
                        {
                            bool confirm = AnsiConsole.Confirm($"[bold red]Are you sure you want to delete task '{task.Title}'?[/]");

                            if (confirm)
                            {
                                _usersService.DeleteTask(userDTO.User.Id, taskId);
                                _taskService.DeleteTask(taskId);
                                AnsiConsole.MarkupLine("[bold red]Task deleted successfully.[/]");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex.Message);
                        AnsiConsole.MarkupLine($"[bold red]Error managing task: {ex.Message}[/]");
                    }
                    break;

                case 2: // Add task
                    try
                    {
                        string title = AnsiConsole.Ask<string>("[bold]Enter task title:[/]");

                        string description = AnsiConsole.Ask<string>("[bold]Enter task description:[/]", "");

                        int difficulty = AnsiConsole.Prompt(
                            new SelectionPrompt<int>()
                                .Title("[bold]Select task difficulty (1-5):[/]")
                                .AddChoices(1, 2, 3, 4, 5));

                        string expirationInput = AnsiConsole.Ask<string>("[bold]Enter task expiration date (yyyy-MM-dd) or leave empty:[/]", "");
                        DateTime? expirationDate = null;

                        if (!string.IsNullOrEmpty(expirationInput) && DateTime.TryParse(expirationInput, out DateTime parsedDate))
                        {
                            expirationDate = parsedDate;
                        }

                        AA1.Models.Task task = _taskService.CreateTask(title, description, difficulty, expirationDate);
                        _usersService.AddTask(userDTO.User.Id, task);

                        AnsiConsole.MarkupLine("[bold green]Task created successfully![/]");
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex.Message);
                        AnsiConsole.MarkupLine($"[bold red]Error creating task: {ex.Message}[/]");
                    }
                    break;
            }
        } while (option != 0);
    }
    public void ShopMenu(LoginResultDTO userDTO)
    {
        int option;

        showShopMenu();
        Dictionary<int, int> itemMapping = new Dictionary<int, int>();
        Dictionary<int, Items> itemsToMap = _itemsService.GetItems();
        int index = 2;

        foreach (var _item in itemsToMap)
        {
            itemMapping.Add(index, _item.Key);
            index++;
        }

        while (true)
        {
            List<string> menuOptions = new List<string> { "1 - Filter", "0 - Exit" };

            foreach (var mapping in itemMapping)
            {
                Items currentItem = _itemsService.GetItemById(mapping.Value);
                menuOptions.Add($"{mapping.Key} - {currentItem.Name}");
            }

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Shop Menu[/]")
                    .AddChoices(menuOptions));

            if (choice.Contains("0 - Exit"))
            {
                return;
            }
            if (choice.Contains("1 - Filter"))
            {
                itemMapping = FilterMenu();
                continue;
            }

            int itemId = int.Parse(choice.Split(" - ")[0]);
            Items item = _itemsService.GetItemById(itemMapping[itemId]);

            var itemTable = new Table();
            itemTable.AddColumn("Attribute");
            itemTable.AddColumn("Value");

            itemTable.AddRow("Name", item.Name);
            itemTable.AddRow("Type", item.Type);
            itemTable.AddRow("Value", item.Value.ToString());
            foreach (var stat in item.Stats)
            {
                itemTable.AddRow(stat.Key, stat.Value.ToString());
            }

            AnsiConsole.Write(new Panel(itemTable)
                .Header($"[bold cyan]{item.Name}[/]")
                .Border(BoxBorder.Rounded)
                .Expand());

            var action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]What do you want to do?[/]")
                    .AddChoices("Buy", "Cancel"));

            if (action == "Buy")
            {
                try
                {
                    _usersService.BuyItem(userDTO.User.Id, item);
                    AnsiConsole.MarkupLine($"[bold green]Item bought: {item.Name}[/]");
                    Console.ReadKey(true);
                }
                catch (Exception ex)
                {
                    logger.Warn(ex.Message);
                    AnsiConsole.MarkupLine($"[bold red]Error buying item: {ex.Message}[/]");
                    Console.ReadKey(true);
                }
            }
        }
    }

    private Dictionary<int, int> FilterMenu()
    {
        var filterType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold yellow]Filter by:[/]")
                .AddChoices("Type", "Stat", "Cancel"));

        Dictionary<int, Items> filteredItems = new Dictionary<int, Items>();

        if (filterType == "Type")
        {
            try
            {
                var type = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold]Select item type:[/]")
                        .AddChoices("Weapon", "Helmet", "Chestplate", "Boots", "Gloves", "Leggins"));

                filteredItems = _itemsService.filterItemsByType(type);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]{ex.Message}[/]");
                Console.ReadKey(true);

            }
        }
        else if (filterType == "Stat")
        {
            try
            {
                var stat = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold]Select stat to filter:[/]")
                        .AddChoices("MaxHP", "XpReward", "GoldReward", "Heal"));

                filteredItems = _itemsService.filterItemsByStat(stat);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]{ex.Message}[/]");
                Console.ReadKey(true);
            }
        }
        else
        {
            return _itemsService.GetItems().ToDictionary(i => i.Key + 2, i => i.Key);
        }

        Dictionary<int, int> itemMapping = new Dictionary<int, int>();
        int index = 2;
        foreach (var item in filteredItems)
        {
            itemMapping.Add(index, item.Key);
            index++;
        }

        return itemMapping;
    }
}