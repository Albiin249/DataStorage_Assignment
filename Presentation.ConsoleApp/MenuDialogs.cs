using Business.Helpers;
using Business.Models;
using Business.Services;
using Data.Migrations;
using System.Text.RegularExpressions;

namespace Presentation.ConsoleApp;

public class MenuDialogs(CustomerService customerService, ProductService productService, UserService userService, ProjectService projectService, StatusTypeService statusTypeService, ProjectNumberGenerator projectNumberGenerator)
{
    private readonly CustomerService _customerService = customerService;
    private readonly ProductService _productService = productService; 
    private readonly UserService _userService = userService;
    private readonly ProjectService _projectService = projectService;
    private readonly StatusTypeService _statusTypeService = statusTypeService;
    private readonly ProjectNumberGenerator _projectNumberGenerator = projectNumberGenerator;

    //MAIN MENU
    #region MainMenu
    public async Task MainMenu()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("---------MAIN MENU---------");
            Console.WriteLine("1. Create Objects");
            Console.WriteLine("2. Update Objects");
            Console.WriteLine("3. Show Objects");
            Console.WriteLine("4. Delete Objects");
            Console.WriteLine("5. Exit");
            Console.WriteLine("---------------------------");
            Console.WriteLine();
            Console.Write("Please choose an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await CreateMenu(); 
                    break;
                case "2":
                    await UpdateMenu();
                    break;
                case "3":
                    await ShowMenu();
                    break;
                case "4":
                    await DeleteMenu();
                    break;
                case "5":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    #endregion

    //CREATE MENU
    #region CreateMenu

    private async Task CreateMenu()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("---------CREATE MENU---------");
            Console.WriteLine("1. Create New Project");
            Console.WriteLine("2. Create New Customer");
            Console.WriteLine("3. Create New User");
            Console.WriteLine("4. Create New Product");
            Console.WriteLine("5. Create New Status Type");
            Console.WriteLine("6. Back to Main Menu");
            Console.WriteLine("-----------------------------");
            Console.WriteLine();
            Console.Write("Please choose an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await CreateProject();
                    break;
                case "2":
                    await CreateCustomer();
                    break;
                case "3":
                    await CreateUser();
                    break;
                case "4":
                    await CreateProduct();
                    break;
                case "5":
                    await CreateStatusType();
                    break;
                case "6":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
    #endregion

    //METHODS FOR CREATEMENU
    #region MethodsForCreateMenu
    
    private async Task CreateProject()
    {
        {
            string title;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Create New Project ---");

                Console.Write("Enter project title: ");
                title = Console.ReadLine()!;

                if (string.IsNullOrWhiteSpace(title))
                {
                    Console.WriteLine("Invalid. Title cannot be empty, please enter a title or enter 'X' to go back to the main menu.");
                    if (title!.ToUpper() == "X")
                        return;
                }
                else
                {
                    break;
                }
            }

            Console.Write("\nEnter project description (Optional): ");
            string description = Console.ReadLine()!;

            DateTime startDate;
            while (true) 
            { 
                Console.Write("\nEnter project start date (yyyy-mm-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out startDate)) //Tog hjälp utav ChatGPT här. Läser in användarens svar, försöker konvertera strängen till ett DateTime-objekt.
                {                                                           //Ifall det lyckas lagras datumet i startDate och metoden forsätter, om det misslyckas returnerar TryParse false.
                    Console.WriteLine("Invalid date format. Please try again.");
                    continue;
                }
                break;
            }

            DateTime endDate;
            while (true)
            {
                Console.Write("\nEnter project end date (yyyy-mm-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out endDate))
                {
                    Console.WriteLine("Invalid date format. Please try again.");
                    continue;
                }
                break ;
            }

            int userId;
            while (true)
            {
                List<User?> users = (await _userService.GetAllUsersAsync()).ToList();

                Console.WriteLine("\nUsers: ");
                foreach (var user in users)
                {
                    Console.WriteLine($"ID: {user.Id} - {user.FirstName} {user.LastName}");
                }


                Console.Write("Enter project manager (user ID): ");
                if (!int.TryParse(Console.ReadLine(), out userId))
                {
                    Console.WriteLine("Invalid user ID. Please enter a valid ID or enter 'X' to go back to the main menu.");
                    if (Console.ReadLine()!.ToUpper() == "X")
                        return; 
                }

                if (!users.Any(u => u!.Id == userId))
                {
                    Console.WriteLine($"Could not found a user with ID: {userId}. Please try again or enter 'X' to go back.");
                    continue; 
                }
                else
                {
                    break;
                }
            }

            
            int customerId;
            while (true)
            {
                List<Customer?> customers = (await _customerService.GetAllCustomersAsync()).ToList();

                Console.WriteLine("\nCustomers: ");
                foreach (var customer in customers)
                {
                    Console.WriteLine($"ID: {customer.Id} - {customer.CustomerName}");
                }

                Console.Write("Enter customer ID: ");
                if (!int.TryParse(Console.ReadLine(), out customerId))
                {
                    Console.WriteLine("Invalid Customer ID. Please enter a valid ID or enter 'X' to go back to the main menu.");
                    if (Console.ReadLine()!.ToUpper() == "X")
                        return; 
                }

                if (!customers.Any(c => c!.Id == customerId))
                {
                    Console.WriteLine($"Could not found a customer with ID: {customerId}. Please try again or enter 'X' to go back.");
                    continue;
                }
                else
                {
                    break;
                }
            }

            int productId;
            while (true)
            {
                List<Product?> products = (await _productService.GetAllProductsAsync()).ToList();

                Console.WriteLine("\nServices: ");
                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.Id} - {product.ProductName}");
                }
                Console.Write("Enter the ID of which service fits: ");
                if (!int.TryParse(Console.ReadLine(), out productId))
                {
                    Console.WriteLine("Invalid Product ID. Please enter a valid ID or enter 'X' to go back to the main menu.");
                    if (Console.ReadLine()!.ToUpper() == "X")
                        return; 
                }

                if (!products.Any(p => p!.Id == productId))
                {
                    Console.WriteLine($"Could not found a service with ID: {productId}. Please try again or enter 'X' to go back.");
                    continue;
                }
                else
                {
                    break;
                }
            } 

            int statusId;
            while (true)
            {
                List<StatusType?> statusTypes = (await _statusTypeService.GetAllStatusTypesAsync()).ToList();

                Console.WriteLine("\nStatuses: ");
                foreach (var status in statusTypes)
                {
                    Console.WriteLine($"ID: {status.Id} - {status.StatusName}");
                }


                Console.Write("Select project status: ");
                if (!int.TryParse(Console.ReadLine(), out statusId))
                {
                    Console.WriteLine("Invalid Status ID. Please enter a valid ID or enter 'X' to go back.");
                    if (Console.ReadLine()!.ToUpper() == "X")
                        return; 
                }

                if (!statusTypes.Any(p => p!.Id == statusId))
                {
                    Console.WriteLine($"Could not found a status with ID: {statusId}. Please try again or enter 'X' to go back.");
                    continue;
                }
                else
                {
                    break;
                }
            }


            Project project = new()
            {
                Title = title,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
                UserId = userId, 
                CustomerId = customerId, 
                ProductId = productId,
                StatusId = statusId, 
                ProjectNumber = _projectNumberGenerator.GenerateProjectNumber()

            };
            var newProject = project;

            try
            {
                await _projectService.CreateProjectAsync(newProject);
                Console.WriteLine("Project successfully created.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating project. \nError: {ex.Message}");
            }

            Console.ReadKey();
        }
    }

    private async Task CreateCustomer()
    {
        Console.Clear();
        Console.WriteLine("--- Create New Customer ---");
        Console.Write("Enter customer name: ");
        var customerName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(customerName))
        {
            Console.WriteLine("Customer name cannot be empty. Please try again.");
            return;
        }

        try
        {
            var newCustomer = new Customer
            {
                CustomerName = customerName,
            };
            await _customerService.CreateCustomerAsync(newCustomer);
            Console.WriteLine($"Customer {customerName} created successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating customer. \nError: {ex.Message}");
        }
    }

    private async Task CreateUser()
    {
        Console.Clear();
        Console.WriteLine("--- Create New User ---");
        
        string userFirstName;
        while (true)
        {
            Console.Write("Enter user first name (or enter 'X' to go back): ");
            userFirstName = Console.ReadLine()!;

            if(userFirstName.ToUpper() == "X")
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(userFirstName))
            {
                break;
            }

            Console.WriteLine("First name cannot be empty. Please try again.");
        }

        string userLastName;
        while(true)
        {
            Console.Write("Enter user last name (or enter 'X' to go back): ");
            userLastName = Console.ReadLine()!;
            if (userLastName.ToUpper() == "X")
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(userLastName))
            {
                break;
            }

            Console.WriteLine("Last name cannot be empty. Please try again.");
        }
        
        string userEmail;
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; //Tog hjälp av ChatGPT för att skapa denna Regex för email-validering.

        while (true)
        {
            Console.Write("Enter user email (or enter 'X' to go back): ");
            userEmail = Console.ReadLine()!;
            if (userEmail.ToUpper() == "X")
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(userEmail) && Regex.IsMatch(userEmail, emailPattern)) //Tog hjälp av ChatGPT här. Koden kollar så att emailen inte är tom och uppfyller regexens krav.
            {
                break; 
            }

            Console.WriteLine("Invalid email format. Please try again.");
        }

        try
        {
            var newUser = new User
            {
                FirstName = userFirstName,
                LastName = userLastName,
                Email = userEmail
            };

            await _userService.CreateUserAsync(newUser);
            Console.WriteLine($"User {userFirstName} created successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating User. \nError: {ex.Message}");
        }

    }

    private async Task CreateProduct()
    {
        Console.Clear();
        Console.WriteLine("--- Create New Product ---");
        Console.Write("Enter product name: ");
        var productName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(productName))
        {
            Console.WriteLine("Product name cannot be empty. Please try again.");
            return;
        }

        Console.Write("Enter product price: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0) //Tog hjälp av ChatGPT här. Koden tar in inmatningen från användaren som skapar produkten.
        {                                                                          //Försöker konvertera från sträng till decimal, om de misslyckas returneras false. Och priset får inte vara ett negativt tal.
            Console.WriteLine("Invalid price. Please enter a positive number.");
            return;
        }

        try
        {
            var newProduct = new Product
            {
                ProductName = productName,
                Price = price
            };

            await _productService.CreateProductAsync(newProduct);
            Console.WriteLine($"Product {productName} created successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating product. \nError: {ex.Message}");
        }

    }

    private async Task CreateStatusType()
    {
        Console.Clear();
        Console.WriteLine("--- Create New Status ---");
        Console.Write("Enter status type name: ");
        var statusName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(statusName))
        {
            Console.WriteLine("Status name cannot be empty. Please try again.");
            return;
        }
        try
        {
            var newStatus = new StatusType
            {
                StatusName = statusName
            };

            await _statusTypeService.CreateStatusAsync(newStatus);
            Console.WriteLine($"Status {statusName} created successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating statustype. \nError: {ex.Message}");
        }


    }


    #endregion

    //UPDATE MENU
    #region UpdateMenu
    private async Task UpdateMenu()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("---------UPDATE MENU---------");
            Console.WriteLine("1. Update Project");
            Console.WriteLine("2. Update Customer");
            Console.WriteLine("3. Update User");
            Console.WriteLine("4. Update Product");
            Console.WriteLine("5. Update Status Type");
            Console.WriteLine("6. Back to Main Menu");
            Console.WriteLine("-----------------------------");
            Console.WriteLine();
            Console.Write("Please choose an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await UpdateProject();
                    break;
                case "2":
                    await UpdateCustomer();
                    break;
                case "3":
                    await UpdateUser();
                    break;
                case "4":
                    await UpdateProduct();
                    break;
                case "5":
                    await UpdateStatusType();
                    break;
                case "6":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
    #endregion

    //METHODS FOR UPDATEMENU
    #region MethodsForUpdateMenu
    private async Task UpdateProject()
    {
        Console.Clear();
        Console.WriteLine("----UPDATE PROJECT----");
        var projects = await _projectService.GetAllProjectsAsync();
        foreach (var eproject in projects)
        {
            Console.WriteLine($"ID: {eproject.Id}, Title: {eproject.Title}");
        }

        Project project = null; //Tog hjälp av ChatGPT här för att kunna använda project efter while loopen också.

        while (true)
        {
            Console.Write("Enter the ID of the project you want to update (or enter 'X' to exit): ");
            var input = Console.ReadLine();
            if (input.ToUpper() == "X")
            {
                return;
            }

            if (!int.TryParse(input, out int projectId))
            {
                Console.WriteLine("Invalid input. Please enter a valid Project ID.");
                continue;
            }

            project = await _projectService.GetProjectAsync(p => p.Id == projectId);
            if (project == null)
            {
                Console.WriteLine($"No Project with ID {projectId}. Please try again.");
                continue;
            }

            break;
        }

        

        Console.Write("\nEnter new project title (leave empty to keep current title): ");
        var newProjectTitle = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newProjectTitle))
        {
            project.Title = newProjectTitle;
        }

        Console.Write("\nEnter new description (optional): ");
        project.Description = Console.ReadLine();

        Console.WriteLine($"\nCurrent start date: {project.StartDate}");
        DateTime startDate;
        while (true)
        {
            Console.Write("Enter new start date (yyyy-mm-dd) (or enter 'X' to exit): ");
            var input = Console.ReadLine();

            if (input.ToUpper() == "X")
            {
                return;
            }
            if (DateTime.TryParse(input, out startDate))
            {
                break;
            }
            Console.WriteLine("Invalid date format. Please try again.");

        }
        project.StartDate = startDate;


        Console.WriteLine($"\nCurrent end date: {project.EndDate}");
        DateTime endDate;
        while (true)
        {
            Console.Write("Enter new end date (yyyy-mm-dd) (or enter 'X' to exit): ");
            var input = Console.ReadLine();

            if (input.ToUpper() == "X")
            {
                return;
            }
            if (DateTime.TryParse(input, out endDate))
            {
                break;
            }
            Console.WriteLine("Invalid date format. Please try again.");

        }
        project.EndDate = endDate;

        Console.WriteLine($"\nCurrent Customer ID: {project.CustomerId}");
        while (true)
        {
            Console.Write("Enter new Customer ID (or enter 'X' to exit): ");
            var input = Console.ReadLine();
            if (input.ToUpper() == "X")
            {
                return;
            }

            if (!int.TryParse(input, out int customerId))
            {
                Console.WriteLine("Invalid input. Please enter a valid Customer ID.");
                continue;
            }

            var customer = await _customerService.GetCustomerAsync(c => c.Id == customerId);
            if (customer == null)
            {
                Console.WriteLine($"No customer with ID {customerId}. Please try again.");
                continue;
            }

            project.CustomerId = customerId;
            break;
        }

        Console.WriteLine($"\nCurrent Status ID: {project.StatusId}");
        while (true)
        {
            Console.Write("Enter new Status ID (or enter 'X' to exit): ");
            var input = Console.ReadLine();
            if (input.ToUpper() == "X")
            {
                return;
            }

            if (!int.TryParse(input, out int statusId))
            {
                Console.WriteLine("Invalid input. Please enter a valid Status ID.");
                continue;
            }

            var status = await _statusTypeService.GetStatusTypeAsync(s => s.Id == statusId);
            if (status == null)
            {
                Console.WriteLine($"No Status with ID {statusId}. Please try again.");
                continue;
            }

            project.StatusId = statusId;
            break;
        }

        Console.WriteLine($"\nCurrent User ID: {project.UserId}");
        while (true)
        {
            Console.Write("Enter new User ID (or enter 'X' to exit): ");
            var input = Console.ReadLine();
            if (input.ToUpper() == "X")
            {
                return;
            }

            if (!int.TryParse(input, out int userId))
            {
                Console.WriteLine("Invalid input. Please enter a valid User ID.");
                continue;
            }

            var user = await _userService.GetUserAsync(u => u.Id == userId);
            if (user == null)
            {
                Console.WriteLine($"No User with ID {userId}. Please try again.");
                continue;
            }

            project.UserId = userId;
            break;
        }

        Console.WriteLine($"\nCurrent Service ID: {project.ProductId}");
        while (true)
        {
            Console.Write("Enter new Service ID (or enter 'X' to exit): ");
            var input = Console.ReadLine();
            if (input.ToUpper() == "X")
            {
                return;
            }

            if (!int.TryParse(input, out int productId))
            {
                Console.WriteLine("Invalid input. Please enter a valid Service ID.");
                continue;
            }

            var product = await _productService.GetProductAsync(p => p.Id == productId);
            if (product == null)
            {
                Console.WriteLine($"No Service with ID {productId}. Please try again.");
                continue;
            }

            project.ProductId = productId;
            break;
        }


        try
        {
            var updatedProject = await _projectService.UpdateProjectAsync(project);

            if (updatedProject != null)
                Console.WriteLine("Project updated successfully.");
            else
                Console.WriteLine("Failed to update project.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating project. \nError: {ex.Message}");
        }


        Console.ReadKey();


    }

    private async Task UpdateCustomer()
    {
        Console.Clear();
        Console.WriteLine("----UPDATE CUSTOMER----");
        Console.Write("Enter the ID of the customer you want to update: ");
        if (!int.TryParse(Console.ReadLine(), out int customerId) || customerId <= 0) //Tog hjälp av ChatGPT här. Koden läser in användarens inmatning.
        {                                                                           //Om användaren ej skriver ett heltal, kommer det bli true och felmeddelandet skrivs ut.
            Console.WriteLine("Invalid Customer ID. Please enter a valid positive number.");
            return;
        }

        var customer = await _customerService.GetCustomerAsync(c => c.Id == customerId);
        if (customer == null)
        {
            Console.WriteLine($"Customer with {customerId} not found.");
            return;
        }

        Console.Write("Enter new name of customer: ");
        var newCustomerName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newCustomerName))
        {
            Console.WriteLine("Customer name cannot be empty, please try again.");
            return;
        }

        customer.CustomerName = newCustomerName;

        try
        {
            var updatedCustomer = await _customerService.UpdateCustomerAsync(customer);

            if (updatedCustomer != null)
                Console.WriteLine("Customer updated successfully.");
            else
                Console.WriteLine("Failed to update customer.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating customer. \nError: {ex.Message}");
        }
    }
    
    private async Task UpdateUser()
    {
        Console.Clear();
        Console.WriteLine("----UPDATE USER----");
        Console.Write("Enter the ID of the user you want to update: ");

        if (!int.TryParse(Console.ReadLine(), out int userId) || userId <= 0) //Tog hjälp av ChatGPT här. Koden läser in användarens inmatning.
        {                                                                           //Om användaren ej skriver ett heltal, kommer det bli true och felmeddelandet skrivs ut.
            Console.WriteLine("Invalid User ID. Please enter a valid positive number.");
            return;
        }
        var user = await _userService.GetUserAsync(u => u.Id == userId);
        if (user == null)
        {
            Console.WriteLine($"User with {userId} not found.");
            return;
        }

        Console.Write("Enter user firstname: (leave this field empty to keep current firstname)");
        var newUserFirstName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newUserFirstName))
        {
            user.FirstName = newUserFirstName;
        }

        Console.Write("Enter user lastname: (leave this field empty to keep current lastname)");
        var newUserLastName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newUserLastName))
        {
            user.LastName = newUserLastName;
        }

        Console.Write("Enter user Email: (leave this field empty to keep current Email)");
        var newUserEmail = Console.ReadLine();
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; //Tog hjälp utav ChatGPT här för att skapa Regex för Email validering.
        if (!string.IsNullOrWhiteSpace(newUserEmail) && !Regex.IsMatch(newUserEmail, emailPattern)) //Om strängen inte är tom och den matchar inte min regex så skrivs felmeddelande ut.
        {
            Console.WriteLine("Invalid email format. Please try again.");
            return;
        }
        user.Email = newUserEmail!;

        try
        {
            var updatedUser = await _userService.UpdateUserAsync(user);

            if (updatedUser != null)
                Console.WriteLine("User updated successfully.");
            else
                Console.WriteLine("Failed to update user.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user. \nError: {ex.Message}");
        }



    }

    private async Task UpdateProduct()
    {
        Console.Clear();
        Console.WriteLine("----UPDATE PRODUCT----");
        Console.Write("Enter the ID of the product you want to update: ");

        if (!int.TryParse(Console.ReadLine(), out int productId) || productId <= 0) //Tog hjälp av ChatGPT här. Koden läser in användarens inmatning.
        {                                                                           //Om användaren ej skriver ett heltal, kommer det bli true och felmeddelandet skrivs ut.
            Console.WriteLine("Invalid Product ID. Please enter a valid positive number.");
            return;
        }
        var product = await _productService.GetProductAsync(u => u.Id == productId);
        if (product == null)
        {
            Console.WriteLine($"Product with ID: {productId} not found.");
            return;
        }

        Console.Write("Enter product name: (leave this field empty to keep current product name)");
        var newProductName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newProductName))
        {
            product.ProductName = newProductName;
        }

        Console.Write("Enter new product price: (leave this field empty to keep current product price)");
        var newProductPrice = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newProductPrice))
        {
            if (decimal.TryParse(newProductPrice, out decimal parsedPrice) && parsedPrice >= 0) //Tog hjälp av ChatGPT för att skapa detta. 
            {                                                                                   //Den konverterar strängen till ett decimal värde och skriver ut felmeddelande ifall det misslyckas.
                product.Price = parsedPrice;
            }
            else
            {
                Console.WriteLine("Invalid price format. Please enter a valid positive number.");
                return;
            }
        }

        try
        {
            var updatedProduct = await _productService.UpdateProductAsync(product);

            if (updatedProduct != null)
                Console.WriteLine("Product updated successfully.");
            else
                Console.WriteLine("Failed to update product.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating product. \nError: {ex.Message}");
        }


    }
   
    private async Task UpdateStatusType()
    {
        Console.Clear();
        Console.WriteLine("----UPDATE STATUS----");
        Console.Write("Enter the ID of the status type you want to update: ");


        if (!int.TryParse(Console.ReadLine(), out int statusId) || statusId <= 0) //Tog hjälp av ChatGPT här. Koden läser in användarens inmatning.
        {                                                                           //Om användaren ej skriver ett heltal, kommer det bli true och felmeddelandet skrivs ut.
            Console.WriteLine("Invalid Status ID. Please enter a valid positive number.");
            return;
        }
        var status = await _statusTypeService.GetStatusTypeAsync(s => s.Id == statusId);
        if (status == null)
        {
            Console.WriteLine($"Product with ID: {statusId} not found.");
            return;
        }

        Console.Write("Enter status type name: (leave this field empty to keep current status type name)");
        var newStatusTypeName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newStatusTypeName))
        {
            status.StatusName = newStatusTypeName;
        }

        try
        {
            var updatedStatus = await _statusTypeService.UpdateStatusTypeAsync(status);

            if (updatedStatus != null)
                Console.WriteLine("Status updated successfully.");
            else
                Console.WriteLine("Failed to update status.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating status. \nError: {ex.Message}");
        }

    }

    #endregion

    //SHOW MENU
    #region ShowMenu
    private async Task ShowMenu()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("---------SHOW MENU---------");
            Console.WriteLine("1. Show All Customers");
            Console.WriteLine("2. Show All Projects");
            Console.WriteLine("3. Show Project Details");
            Console.WriteLine("4. Back to Main Menu");
            Console.WriteLine("---------------------------");
            Console.WriteLine();
            Console.Write("Please choose an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ViewAllCustomers();
                    break;
                case "2":
                    await ViewAllProjects();
                    break;
                case "3":
                    await ViewProject();
                    break;
                case "4":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    #endregion

    //METODS FOR SHOWMENU
    #region MethodsForShowMenu

    private async Task ViewAllCustomers()
    {
        Console.Clear();
        Console.WriteLine("---- CUSTOMER LIST ----");
        
        var customers = await _customerService.GetAllCustomersAsync();
        if (customers == null) 
        {
            Console.WriteLine("No customers found.");
            return;
        }

        foreach (var customer in customers)
        {
            Console.WriteLine($"ID: {customer.Id} - Name: {customer.CustomerName}");
            Console.WriteLine("-----------------------");
        }

        Console.WriteLine("Press any key to return to main menu.");
        Console.ReadKey();
    }

    private async Task ViewAllProjects()
    {
        Console.Clear();
        Console.WriteLine("---- PROJECT LIST ----");

        var projects = await _projectService.GetAllProjectsAsync();
        if (projects == null)
        {
            Console.WriteLine("No projects found.");
            return;
        }

        //Tog hjälp av ChatGPT för att hämta och få ut statusnamnet.
        //Kodsträngen hämtar alla statustype objekt, omvandlar listan till dictionary, där varje nyckel är s.Id och värdet är namnet.
        var statusTypes = (await _statusTypeService.GetAllStatusTypesAsync()).ToDictionary(s => s.Id, s => s.StatusName);

        foreach (var project in projects)
        {
            //Hämtar ut värdet från statustypes med trygetvalue, och statusId som nyckel. Om den inte lyckas hitta statusen, så returneras Unknown Status. 
            string statusName = statusTypes.TryGetValue(project.StatusId, out var name) ? name : "Unknown Status";
            Console.WriteLine($"Project Number: {project.ProjectNumber}\nName: {project.Title}");
            Console.WriteLine($"Description: {project.Description}");
            Console.WriteLine($"Startdate: {project.StartDate}\nEnddate: {project.EndDate}");
            Console.WriteLine($"Status: {statusName}");
            Console.WriteLine("----------------------");
        }

        Console.WriteLine("Press any key to return to main menu.");
        Console.ReadKey();
    }
  
    private async Task ViewProject()
    {
        Console.Clear();
        Console.WriteLine("---- VIEW PROJECT ----");

        var projects = await _projectService.GetAllProjectsAsync();
        foreach (var eproject in projects)
        {
            Console.WriteLine($"ID: {eproject.Id}, Title: {eproject.Title}");
        }

        Console.Write("\nPlease enter Project ID: ");
        if (!int.TryParse(Console.ReadLine(), out int projectId) || projectId <= 0)
        {
            Console.WriteLine("Invalid Project ID. Please enter a valid positive number.");
            return;
        }

        var project = await _projectService.GetProjectAsync(p => p.Id == projectId);

        if (project == null)
        {
            Console.WriteLine($"Could not find the project with ID: {projectId}");
            return;
        }

        var user = await _userService.GetUserAsync(u => u.Id == project.UserId);
        var product = await _productService.GetProductAsync(p => p.Id == project.ProductId);
        var status = await _statusTypeService.GetStatusTypeAsync(s => s.Id == project.StatusId);
        var customer = await _customerService.GetCustomerAsync(c => c.Id == project.CustomerId);

        Console.WriteLine($"\nProject ID: {project.Id}");
        Console.WriteLine($"Project Number: {project.ProjectNumber}");
        Console.WriteLine($"Project Title: {project.Title}");
        Console.WriteLine($"Project Description: {project.Description}");
        Console.WriteLine($"Project Customer: {customer.CustomerName}");
        Console.WriteLine($"Project Service: {product.ProductName}");
        Console.WriteLine($"Project Manager: {user.FirstName} {user.LastName}");
        Console.WriteLine($"Project Status: {status.StatusName}");
        Console.WriteLine($"Project Start date: {project.StartDate}");
        Console.WriteLine($"Project End date: {project.EndDate}");



        Console.WriteLine("\nPlease press any key to return to the menu.");
        Console.ReadKey();
    }

    #endregion

    //DELETE MENU
    #region DeleteMenu
    private async Task DeleteMenu()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("---------DELETE MENU---------");
            Console.WriteLine("1. Delete Project");
            Console.WriteLine("2. Delete Customer");
            Console.WriteLine("3. Delete Service");
            Console.WriteLine("4. Delete User");
            Console.WriteLine("5. Delete Status");
            Console.WriteLine("6. Back to Main Menu");
            Console.WriteLine("---------------------------");
            Console.WriteLine();
            Console.Write("Please choose an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await DeleteProject();
                    break;
                case "2":
                    await DeleteCustomer();
                    break;
                case "3":
                    await DeleteService();
                    break;
                case "4":
                    await DeleteUser();
                    break;
                case "5":
                    await DeleteStatus();
                    break;
                case "6":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
        
    }

    #endregion

    //METHODS FOR DELETEMENU
    #region MethodsForDeleteMenu

    private async Task DeleteProject()
    {
        while (true)
        {
            Console.WriteLine("---- DELETE PROJECT ----");
            var projects = await _projectService.GetAllProjectsAsync();

            foreach (var project in projects)
            {
                Console.WriteLine($"ID: {project.Id} - Name: {project.Title}");
            }

            Console.Write("\nPlease enter the ID of which project you want to delete (or enter 'X' to go back): ");

            var input = Console.ReadLine();
            if (input?.ToUpper() == "X")
                return;

            if (!int.TryParse(input, out var projectId)) //Tog hjälp utav ChatGPT med detta. Kodsnippen försöker konvertera input(strängen) till en INT.
            {
                Console.WriteLine("Invalid input. Please enter a valid project ID.");
                continue;
            }

            Project? projectToDelete;
            try
            {
                projectToDelete = await _projectService.GetProjectAsync(p => p.Id == projectId);
                if (projectToDelete == null)
                {
                    Console.WriteLine($"Project with ID {input} not found. Please try again.");
                    continue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                continue;
            }

            Console.WriteLine($"Are you sure you want to delete project {projectToDelete.Title}? (Y/N))");
            if (Console.ReadLine()?.ToUpper() != "Y")
                {
                    Console.WriteLine("Project not deleted.");
                }
            else
            {
                try
                {
                    var success = await _projectService.DeleteProjectAsync(projectId);
                    Console.WriteLine(success ? "Project deleted successfully." : "Failed to delete project.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("Press any key to return to main menu..");
                Console.ReadKey();
                return;
            }
        }

    }

    private async Task DeleteCustomer()
    {
        while (true)
        {
            Console.WriteLine("---- DELETE CUSTOMER ----");
            var customers = await _customerService.GetAllCustomersAsync();

            foreach (var customer in customers)
            {
                Console.WriteLine($"ID: {customer.Id} - Name: {customer.CustomerName}");
            }

            Console.Write("\nPlease enter the ID of which customer you want to delete (or enter 'X' to go back): ");
            var input = Console.ReadLine();
            if (input?.ToUpper() == "X")
                return;

            if (!int.TryParse(input, out var customerId)) //Tog hjälp utav ChatGPT med detta. Kodsnippen försöker konvertera input(strängen) till en INT.
            {
                Console.WriteLine("Invalid input. Please enter a valid project ID.");
                continue;
            }

            Customer? customerToDelete;
            try
            {
                customerToDelete = await _customerService.GetCustomerAsync(c => c.Id == customerId);
                if (customerToDelete == null)
                {
                    Console.WriteLine($"Customer with ID {input} not found. Please try again.");
                    continue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                continue;
            }

            Console.WriteLine($"Are you sure you want to delete customer {customerToDelete.CustomerName}? (Y/N))");
            if (Console.ReadLine()?.ToUpper() != "Y")
            {
                Console.WriteLine("Customer not deleted.");
            }
            else
            {
                try
                {
                    var success = await _customerService.DeleteCustomerAsync(customerId);
                    Console.WriteLine(success ? "Customer deleted successfully." : "Failed to delete customer.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("Press any key to return to main menu..");
                Console.ReadKey();
                return;
            }
            
        }
    }

    private async Task DeleteService()
    {
        while (true)
        {
            Console.WriteLine("---- DELETE SERVICE ----");
            var products = await _productService.GetAllProductsAsync();

            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.Id} - Name: {product.ProductName}");
            }

            Console.Write("\nPlease enter the ID of which service you want to delete (or enter 'X' to go back): ");

            var input = Console.ReadLine();
            if (input?.ToUpper() == "X")
                return;

            if (!int.TryParse(input, out var productId)) //Tog hjälp utav ChatGPT med detta. Kodsnippen försöker konvertera input(strängen) till en INT.
            {
                Console.WriteLine("Invalid input. Please enter a valid product ID.");
                continue;
            }

            Product? productToDelete;
            try
            {
                productToDelete = await _productService.GetProductAsync(p => p.Id == productId);
                if (productToDelete == null)
                {
                    Console.WriteLine($"Product with ID {input} not found. Please try again.");
                    continue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                continue;
            }

            Console.WriteLine($"Are you sure you want to delete product {productToDelete.ProductName}? (Y/N))");
            if (Console.ReadLine()?.ToUpper() != "Y")
            {
                Console.WriteLine("Product not deleted.");
            }
            else
            {
                try
                {
                    var success = await _productService.DeleteProductAsync(productId);
                    Console.WriteLine(success ? "Product deleted successfully." : "Failed to delete product.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("Press any key to return to main menu..");
                Console.ReadKey();
                return;
            }
            
        }
    }

    private async Task DeleteUser()
    {
        while (true)
        {
            Console.WriteLine("---- DELETE USER ----");
            var users = await _userService.GetAllUsersAsync();

            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id} - Name: {user.FirstName} {user.LastName}");
            }

            Console.Write("\nPlease enter the ID of which user you want to delete (or enter 'X' to go back): ");

            var input = Console.ReadLine();
            if (input?.ToUpper() == "X")
                return;

            if (!int.TryParse(input, out var userId)) //Tog hjälp utav ChatGPT med detta. Kodsnippen försöker konvertera input(strängen) till en INT.
            {
                Console.WriteLine("Invalid input. Please enter a valid product ID.");
                continue;
            }

            User? userToDelete;
            try
            {
                userToDelete = await _userService.GetUserAsync(u => u.Id == userId);
                if (userToDelete == null)
                {
                    Console.WriteLine($"User with ID {input} not found. Please try again.");
                    continue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                continue;
            }

            Console.WriteLine($"Are you sure you want to delete User {userToDelete.FirstName} {userToDelete.LastName}? (Y/N))");
            if (Console.ReadLine()?.ToUpper() != "Y")
            {
                Console.WriteLine("User not deleted.");
            }
            else
            {
                try
                {
                    var success = await _userService.DeleteUserAsync(userId);
                    Console.WriteLine(success ? "User deleted successfully." : "Failed to delete user.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("Press any key to return to main menu..");
                Console.ReadKey();
                return;
            }

        }
    }

    private async Task DeleteStatus()
    {
        while (true)
        {
            Console.WriteLine("---- DELETE STATUS ----");
            var statuses = await _statusTypeService.GetAllStatusTypesAsync();

            foreach (var status in statuses)
            {
                Console.WriteLine($"ID: {status.Id} - Name: {status.StatusName}");
            }

            Console.Write("\nPlease enter the ID of which status you want to delete (or enter 'X' to go back): ");

            var input = Console.ReadLine();
            if (input?.ToUpper() == "X")
                return;

            if (!int.TryParse(input, out var statusId)) //Tog hjälp utav ChatGPT med detta. Kodsnippen försöker konvertera input(strängen) till en INT. Lyckas inte tryparse, så skrivs felmeddelande ut.
            {
                Console.WriteLine("Invalid input. Please enter a valid product ID.");
                continue;
            }

            StatusType? statusToDelete;
            try
            {
                statusToDelete = await _statusTypeService.GetStatusTypeAsync(s => s.Id == statusId);
                if (statusToDelete == null)
                {
                    Console.WriteLine($"Status with ID {input} not found. Please try again.");
                    continue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                continue;
            }

            Console.WriteLine($"Are you sure you want to delete status {statusToDelete.StatusName}? (Y/N))");
            if (Console.ReadLine()?.ToUpper() != "Y")
            {
                Console.WriteLine("Status not deleted.");
            }
            else 
            { 
                try
                {
                    var success = await _statusTypeService.DeleteStatusTypeAsync(statusId);
                    Console.WriteLine(success ? "Status deleted successfully." : "Failed to delete status.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

            Console.WriteLine("Press any key to return to main menu..");
            Console.ReadKey();
            return;
            }
        }
    }


    #endregion
}
