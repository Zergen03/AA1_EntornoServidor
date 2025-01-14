using AA1.Models;
using AA1.Data;
using AA1.Services;
using Microsoft.Extensions.DependencyInjection;


var services = new ServiceCollection();
services.AddSingleton<IUsersRepository, UsersRepository>();
services.AddTransient<IUsersService, UsersService>();
services.AddSingleton<Menu>();

var serviceProvider = services.BuildServiceProvider();
var UsersRepository = serviceProvider.GetService<IUsersRepository>();
var UsersService = serviceProvider.GetService<IUsersService>();
var Menu = serviceProvider.GetService<Menu>();

Menu.ShowMenu();