using AA1.Models;
using AA1.Data;
using AA1.Services;
using AA1.Presentation;
using Microsoft.Extensions.DependencyInjection;


var services = new ServiceCollection();
services.AddSingleton<IUsersRepository, UsersRepository>();
services.AddTransient<IUsersService, UsersService>();
services.AddSingleton<IItemsRepository, ItemsRepository>();
services.AddTransient<IItemsService, ItemsService>();
services.AddSingleton<ITaskRepository, TaskRepository>();
services.AddTransient<ITaskService, TaskService>();
services.AddSingleton<Menu>();

var serviceProvider = services.BuildServiceProvider();
var UsersRepository = serviceProvider.GetService<IUsersRepository>();
var UsersService = serviceProvider.GetService<IUsersService>();
var Menu = serviceProvider.GetService<Menu>();

// Menu.ShowMenu();


try{
    if(UsersService != null){
        UsersService.Register("Alex", "1234");
        Console.WriteLine("User registered");
    }else{
        Console.WriteLine("Service is null");
    }
}catch(Exception e){
    Console.WriteLine(e.Message);
}