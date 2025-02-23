using Business.Factories;
using Business.Helpers;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController(IProjectService projectService, ICustomerService customerService, IStatusTypeService statusTypeService, IProductService productService, IUserService userService, ProjectFactory projectFactory, CalculatePrice calculatePrice) : ControllerBase
{
    private readonly IProjectService _projectService = projectService;
    private readonly ICustomerService _customerService = customerService;
    private readonly IStatusTypeService _statusTypeService = statusTypeService;
    private readonly IProductService _productService = productService;
    private readonly IUserService _userService = userService;
    private readonly ProjectFactory _projectFactory = projectFactory;
    private readonly CalculatePrice _calculatePrice = calculatePrice;
    //GET 
    #region HTTPGET
    [HttpGet("projects")]
    public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        return Ok(projects);
    }

    [HttpGet("customers")]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    [HttpGet("status")]
    public async Task<ActionResult<IEnumerable<StatusType>>> GetStatuses()
    {
        var statuses = await _statusTypeService.GetAllStatusTypesAsync();
        return Ok(statuses);
    }

    [HttpGet("services")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetProject(int id)
    {
        var project = await _projectService.GetProjectAsync(p => p.Id == id);

        if (project == null)
        {
            return NotFound();
        }

        return Ok(project);
    }

    #endregion

    //POST
    #region HTTPPOSTS
    [HttpPost("projects")]
    public async Task<IActionResult> CreateProject([FromBody] Project projectModel) //Tog hjälp av ChatGPT för att skapa denna.
    {
        //Kontrollerar om det finns ett projekt med samma titel, om det finns så skickas 409(conflict)
        var existingProject = await _projectService.CheckIfProjectExists(x => x.Title == projectModel.Title);
        if (existingProject)
        {
            return Conflict(new { message = "A project with the same title already exists." });
        }

        try
        {
            await _projectService.CreateProjectAsync(projectModel);

            return CreatedAtAction(nameof(GetProject), new { id = projectModel.Id }, projectModel); //Skapar ett svar med statuskod 201 (objektet har skapats), refererar till metoden getproject för att hämta objektet baserat på id, skapar ett nytt objekt med id, och projectModel är själva objektet som skapas.
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }


    [HttpPost("users")]
    public async Task<IActionResult> CreateUser([FromBody] User userModel) 
    {
        var existingUser = await _userService.CheckIfUserExists(u => u.Email == userModel.Email);
        if (existingUser)
        {
            return Conflict(new { message = "A User with the same Email already exists." });
        }

        try
        {
            await _userService.CreateUserAsync(userModel);

            return CreatedAtAction(nameof(GetUsers), new { id = userModel.Id }, userModel);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpPost("customers")]
    public async Task<IActionResult> CreateCustomer([FromBody] Customer customerModel)
    {
        var existingCustomer = await _customerService.CheckIfCustomerExistsAsync(c => c.CustomerName == customerModel.CustomerName);
        if (existingCustomer)
        {
            return Conflict(new { message = "This customer does already exists." });
        }

        try
        {
            await _customerService.CreateCustomerAsync(customerModel);

            return CreatedAtAction(nameof(GetCustomers), new { id = customerModel.Id }, customerModel);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpPost("services")]
    public async Task<IActionResult> CreateProduct([FromBody] Product productModel)
    {
        var existingProduct = await _productService.CheckIfProductExists(p => p.ProductName == productModel.ProductName);
        if (existingProduct)
        {
            return Conflict(new { message = "This service/product does already exists." });
        }

        try
        {
            await _productService.CreateProductAsync(productModel);

            return CreatedAtAction(nameof(GetProducts), new { id = productModel.Id }, productModel);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpPost("status")]
    public async Task<IActionResult> CreateStatus([FromBody] StatusType statusModel)
    {
        var existingStatus = await _statusTypeService.CheckIfStatusTypeExists(s => s.StatusName == statusModel.StatusName);
        if (existingStatus)
        {
            return Conflict(new { message = "This status does already exists." });
        }

        try
        {
            await _statusTypeService.CreateStatusAsync(statusModel);

            return CreatedAtAction(nameof(GetStatuses), new { id = statusModel.Id }, statusModel);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    #endregion

    //PUT
    #region HTTPPUT
    //Tog hjälp utav ChatGPT för att skapa denna PUT metod.
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, [FromBody] Project updatedProject)
    {
        if (updatedProject == null) 
        {
            return BadRequest();
        }

        var existingProject = await _projectService.GetProjectAsync(p => p.Id == id);
        if (existingProject == null)
        {
            return NotFound();
        }

        //Här uppdateras enbart de fält som skickats med.
        existingProject.Title = updatedProject.Title ?? existingProject.Title;
        existingProject.Description = updatedProject.Description ?? existingProject.Description;
        existingProject.TotalHours = updatedProject.TotalHours;
        existingProject.StartDate = updatedProject.StartDate != DateTime.MinValue ? updatedProject.StartDate : existingProject.StartDate;
        existingProject.EndDate = updatedProject.EndDate != DateTime.MinValue ? updatedProject.EndDate : existingProject.EndDate;
        existingProject.CustomerId = updatedProject.CustomerId != 0 ? updatedProject.CustomerId : existingProject.CustomerId;
        existingProject.StatusId = updatedProject.StatusId != 0 ? updatedProject.StatusId : existingProject.StatusId;
        existingProject.UserId = updatedProject.UserId != 0 ? updatedProject.UserId : existingProject.UserId;
        existingProject.ProductId = updatedProject.ProductId != 0 ? updatedProject.ProductId : existingProject.ProductId;
        
        //Försöker uppdatera projektet med min service.
        try
        {
            await _projectService.UpdateProjectAsync(existingProject);
            return Ok(existingProject);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }


    #endregion

    //DELETE
    #region HTTPDELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        try
        {
            var success = await _projectService.DeleteProjectAsync(id);

            if (!success)
            {
                return NotFound(new { message = "Project was not deleted." });
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
    #endregion
}
