using AA1.Models;
using AA1.Data;
using AA1.Services;
using NLog;
using Spectre.Console;


namespace AA1.Presentation;

public class AdminMenu
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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

    private void MainMenu()
    {
        Console.WriteLine("1) Create item");
        Console.WriteLine("2) Delete item");
        Console.WriteLine("3) Show items");
        Console.WriteLine("4) Delete user");
        Console.WriteLine("0) Exit");
    }

    public void ShowAdminMenu()
    {
        int option;
        do
        {
            AnsiConsole.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Admin Menu[/]")
                    .AddChoices("1) Create item", "2) Delete item", "3) Show items", "4) Delete user", "0) Exit"));

            option = int.Parse(choice[0].ToString());

            List<string> types = new List<string> { "Weapon", "Helmet", "Chestplate", "Boots", "Gloves", "Leggins" };
            List<string> modifiers = new List<string> { "MaxHP", "Xpreward", "GoldReward", "Heal" };

            switch (option)
            {
                case 1: // Create item
                    try
                    {
                        string name = AnsiConsole.Ask<string>("[bold]Name:[/]");

                        int value = AnsiConsole.Ask<int>("[bold]Value:[/]");

                        string type = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[bold]Select item type:[/]")
                                .AddChoices(types));

                        Dictionary<string, int> stats = modifiers.ToDictionary(mod => mod, mod => 0);

                        while (true)
                        {
                            string modifier = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .Title("[bold]Select modifier (or choose 'Finish'):[/]")
                                    .AddChoices(modifiers.Append("Finish")));

                            if (modifier == "Finish")
                                break;

                            int modifierValue = AnsiConsole.Ask<int>($"[bold]Value for {modifier}:[/]");
                            stats[modifier] = modifierValue;
                        }

                        _itemsService.CreateItem(name, value, type, stats);
                    }
                    catch (Exception e)
                    {
                        logger.Warn(e.Message);
                        AnsiConsole.MarkupLine($"[bold red]{e.Message}[/]");
                        Console.ReadKey(true);
                    }
                    break;

                case 2: // Delete item
                    try
                    {
                        Dictionary<int, Items> itemsToDelete = _itemsService.GetItems();

                        if (itemsToDelete.Count == 0)
                        {
                            AnsiConsole.MarkupLine("[bold red]No items available to delete.[/]");
                            AnsiConsole.MarkupLine("[bold]Press any key to return...[/]");
                            Console.ReadKey(true);
                            return;
                        }

                        var choices = itemsToDelete.Select(item => $"[red]{item.Key}[/] - {item.Value.Name}").ToList();
                        choices.Add("[yellow]0[/] - Cancel");

                        var selectedItem = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[bold yellow]Select an item to delete:[/]")
                                .PageSize(10)
                                .AddChoices(choices));

                        string cleanSelection = selectedItem.Split(" - ")[0]
                            .Replace("[red]", "")
                            .Replace("[yellow]", "")
                            .Replace("[/]", "")
                            .Trim();

                        if (cleanSelection == "0")
                            return;

                        int itemId = int.Parse(cleanSelection);

                        var confirm = AnsiConsole.Confirm($"[bold red]Are you sure you want to delete item ID {itemId}?[/]");
                        if (confirm)
                        {
                            _itemsService.DeleteItem(itemId);
                            AnsiConsole.MarkupLine("[bold green]Item deleted successfully.[/]");
                        }
                    }
                    catch (Exception e)
                    {
                        logger.Warn(e.Message);
                        AnsiConsole.MarkupLine($"[bold red]{e.Message}[/]");
                        Console.ReadKey(true);
                    }
                    break;

                case 3: // Show items
                    Dictionary<int, Items> items = _itemsService.GetItems();

                    var table = new Table();
                    table.AddColumn("ID");
                    table.AddColumn("Name");
                    table.AddColumn("Type");
                    table.AddColumn("Value");

                    foreach (var stat in modifiers)
                    {
                        table.AddColumn(stat);
                    }

                    foreach (var item in items.Values)
                    {
                        var row = new List<string>
                    {
                        item.Id.ToString(),
                        item.Name,
                        item.Type,
                        item.Value.ToString()
                    };

                        foreach (var stat in modifiers)
                        {
                            row.Add(item.Stats.TryGetValue(stat, out var statValue) ? statValue.ToString() : "0");
                        }

                        table.AddRow(row.ToArray());
                    }

                    AnsiConsole.Write(new Panel(table)
                        .Header("[bold cyan]Items List[/]")
                        .Border(BoxBorder.Rounded)
                        .Expand());

                    AnsiConsole.MarkupLine("\n[bold]Press any key to return...[/]");
                    Console.ReadKey(true);
                    break;

                case 4: // Delete user
                    try
                    {
                        int id = AnsiConsole.Ask<int>("[bold]Enter user ID to delete:[/]");

                        _usersService.DeleteUser(id);
                    }
                    catch (Exception e)
                    {
                        logger.Warn(e.Message);
                        AnsiConsole.MarkupLine($"[bold red]{e.Message}[/]");
                        Console.ReadKey(true);
                    }
                    break;

                case 0:
                    break;
            }
        } while (option != 0);
    }

}