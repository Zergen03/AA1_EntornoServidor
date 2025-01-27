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
        Console.WriteLine("2) Update item");
        Console.WriteLine("3) Delete item");
        Console.WriteLine("4) Show items");
        Console.WriteLine("5) Delete user");
        Console.WriteLine("0) Exit");
    }

    private void ShowMenu(){
        int option;
        do
        {
            ManinMenu();
            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Invalid option");
            }

            switch (option)
            {
                case 1:
                    // _itemsService.CreateItem();
                    break;
                case 2:
                    // _itemsService.UpdateItem();
                    break;
                case 3:
                    // _itemsService.DeleteItem();
                    break;
                case 4:
                    _itemsService.ShowShop();
                    break;
                case 5:
                    // _usersService.DeleteUser();
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