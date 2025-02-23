using Business.Factories;
using Business.Helpers;
using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("WebApi")));
//Tog hjälp utav ChatGPT för att sätta upp databasconnection. Den hämtar connection från appsettings.json där jag kodat en default connection.


builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<ProjectRepository>();
builder.Services.AddScoped<StatusTypeRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStatusTypeService, StatusTypeService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ProjectFactory>();
builder.Services.AddScoped<ProjectNumberGenerator>();
var app = builder.Build();
app.MapOpenApi();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.Run();
