using AA1.Models;
using AA1.Data;
using AA1.Services;

public class Menu
{

    private readonly IUsersRepository _usersRepository;
    private readonly IUsersService _usersService;
    // private readonly ITaskRepository _taskRepository;
    // private readonly ITaskService _taskService;    

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
                    Console.WriteLine("Login");
                    break;
                case 2:
                    Console.WriteLine("Register");
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

    private void ManinMenu()
    {
        Console.WriteLine("1) Login");
        Console.WriteLine("2) Register");
        Console.WriteLine("0) Exit");
    }

    private void Login()
    {
        Console.WriteLine("Enter your name");
        string name = Console.ReadLine();
        if(string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Invalid name");
            return;
        }
        Console.WriteLine("Enter your password");
        string password = Console.ReadLine();
        if(string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Invalid password");
            return;
        }

        try{
            Users user = _usersService.Login(name, password);
        }catch(System.Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}