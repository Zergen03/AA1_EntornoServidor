using AA1.Models;
using AA1.Data;
using AA1.Services;
using AA1.Presentation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;


var services = new ServiceCollection();
services.AddSingleton<IUsersRepository, UsersRepository>();
services.AddTransient<IUsersService, UsersService>();
services.AddSingleton<IItemsRepository, ItemsRepository>();
services.AddTransient<IItemsService, ItemsService>();
services.AddSingleton<ITaskRepository, TaskRepository>();
services.AddTransient<ITaskService, TaskService>();
services.AddSingleton<Menu>();
services.AddSingleton<AdminMenu>();

services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
    loggingBuilder.AddNLog();
});


var serviceProvider = services.BuildServiceProvider();
var UsersRepository = serviceProvider.GetService<IUsersRepository>();
var UsersService = serviceProvider.GetService<IUsersService>();
var Menu = serviceProvider.GetService<Menu>();
var AdminMenu = serviceProvider.GetService<AdminMenu>();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();


logger.LogTrace("Este es un mensaje de Trace.");
logger.LogDebug("Este es un mensaje de Debug.");
logger.LogInformation("Este es un mensaje de Información.");
logger.LogWarning("Este es un mensaje de Advertencia.");
logger.LogError("Este es un mensaje de Error.");
logger.LogCritical("Este es un mensaje de Error Crítico.");


Menu.MainMenu();

// try{
//     if(UsersService != null){
//         UsersService.Register("Alex", "1234");
//         Console.WriteLine("User registered");
//     }else{
//         Console.WriteLine("Service is null");
//     }
// }catch(Exception e){
//     Console.WriteLine(e.Message);
// }