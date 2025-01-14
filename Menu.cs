using AA1.Models;
using AA1.Data;
using AA1.Services;

public class Menu
{

    private readonly IUsersRepository _usersRepository;
    private readonly IUsersService _usersService;
    // private readonly ITaskRepository _taskRepository;
    // private readonly ITaskService _taskService;    

    public Menu(IUsersRepository usersRepository, IUsersService usersService)
    {
        _usersRepository = usersRepository;
        _usersService = usersService;
    }


    private void ManinMenu()
    {
        Console.WriteLine("1) Login");
        Console.WriteLine("2) Register");
        Console.WriteLine("0) Exit");
    }

        private void UserMenu()
    {
        Console.WriteLine("1) Inventory");
        Console.WriteLine("2) Tasks");
        Console.WriteLine("3) Stats");
        Console.WriteLine("0) Exit");
    }

        private void InventoryMenu()
    {
        Console.WriteLine("1) Add item");
        Console.WriteLine("2) Delete item");
        Console.WriteLine("3) Equip item");
        Console.WriteLine("4) Unequip item");
        Console.WriteLine("0) Exit");
    }

    public void ShowMenu()
    {
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
                    Login();
                    break;
                case 2:
                    Register();
                    break;
                case 0:
                    Console.WriteLine("Exit");
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
        if(string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Invalid name");
            return;
        }
        Console.WriteLine("Enter your password");
        string? password = Console.ReadLine();
        if(string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Invalid password");
            return;
        }

        try{
            Users user = _usersService.Login(name, password);
            MenuUser(user);
        }catch(System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            Login();
        }
    }

    private void Register()
    {
        Console.WriteLine("Enter your name");
        string? name = Console.ReadLine();
        if(string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Invalid name");
            return;
        }
        Console.WriteLine("Enter your password");
        string? password = Console.ReadLine();
        if(string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Invalid password");
            return;
        }

        try{
            Users user = _usersRepository.CreateUser(name, password);
            _usersRepository.SaveChanges();
        }catch(System.Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void MenuUser(Users user)
    {
        int option;
        do
        {
            UserMenu();
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
                    Console.WriteLine("Tasks");
                    break;
                case 3:
                    Console.WriteLine("Stats");
                    break;
                case 0:
                    Console.WriteLine("Exit");
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
        do
        {
            InventoryMenu();
            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Invalid option");
            }

            switch (option)
            {
                case 1:
                    // Console.WriteLine("Add item");
                    break;
                case 2:
                    // Console.WriteLine("Delete item");
                    break;
                case 3:
                    Console.WriteLine("Equip item");
                    break;
                case 4:
                    Console.WriteLine("Unequip item");
                    break;
                case 0:
                    Console.WriteLine("Exit");
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        } while (int.TryParse(Console.ReadLine(), out option) && option != 0);
    }

}