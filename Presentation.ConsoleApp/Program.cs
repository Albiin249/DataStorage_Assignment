using Business.Factories;
using Business.Helpers;
using Business.Services;
using Data.Contexts;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation.ConsoleApp;

var connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Projects\\DataStorage_Assignment\\Data\\Databases\\local_database.mdf;Integrated Security=True;Connect Timeout=30";

var services = new ServiceCollection()
    .AddDbContext<DataContext>(x => x.UseSqlServer(connectionString))
    .AddScoped<CustomerRepository>()
    .AddScoped<ProductRepository>()
    .AddScoped<ProjectRepository>()
    .AddScoped<StatusTypeRepository>()
    .AddScoped<UserRepository>()

    .AddScoped<CustomerService>()
    .AddScoped<ProductService>()
    .AddScoped<ProjectService>()
    .AddScoped<StatusTypeService>()
    .AddScoped<UserService>()
    .AddScoped<MenuDialogs>()

    .AddScoped<ProjectFactory>()
    .AddScoped<ProjectNumberGenerator>()
    .AddScoped<CalculatePrice>()

    .BuildServiceProvider();

var menuDialogs = services.GetRequiredService<MenuDialogs>();
await menuDialogs.MainMenu();