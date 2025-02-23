using Business.Factories;
using Business.Helpers;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System.Linq.Expressions;

namespace Business.Services;

public class ProjectService(ProjectRepository repository, ProjectFactory _projectFactory, CalculatePrice calculatePrice, ProductRepository productRepository) : IProjectService
{
    private readonly ProjectRepository _projectRepository = repository;
    private readonly ProjectFactory _projectFactory = _projectFactory;
    private readonly CalculatePrice _calculatePrice = calculatePrice;
    private readonly ProductRepository _productRepository = productRepository;

    //CREATE
    public async Task CreateProjectAsync(Project projectModel)
    {
        var projectEntity = _projectFactory.Create(projectModel);

        var product = await _productRepository.GetAsync(p => p.Id == projectEntity.ProductId);
        if (product == null)
        {
            throw new Exception("Product not found.");
        }

        projectEntity.TotalPrice = _calculatePrice.CalculateTotalPriceAndHours(projectEntity.TotalHours, product.Price);


        await _projectRepository.BeginTransactionAsync();

        try
        {
            var existingProject = await _projectRepository.GetAsync(x => x.Title == projectModel.Title);
            if (existingProject != null)
                throw new Exception("Project with this title already exists.");
           
            await _projectRepository.CreateASync(projectEntity!);
            await _projectRepository.CommitTransactionAsync();
        }
        catch
        {
            await _projectRepository.RollBackTransactionAsync();
        }
    }


    //READ
    public async Task<IEnumerable<Project?>> GetAllProjectsAsync()
    {
        var projectsEntities = await _projectRepository.GetAllAsync();
        return projectsEntities.Select(_projectFactory.Create);
    }

    public async Task<Project?> GetProjectAsync(Expression<Func<ProjectEntity, bool>> expression)
    {
        var projectEntity = await _projectRepository.GetAsync(expression);
        return _projectFactory.Create(projectEntity);
    }

    //UPDATE
    public async Task<Project?> UpdateProjectAsync(Project project)
    {
        await _projectRepository.BeginTransactionAsync();

        try
        {
            // Tog hjälp av ChatGPT för att hämta produktens pris
            var product = await _productRepository.GetAsync(p => p.Id == project.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            //Här beräknas nytt TotalPrice baserat på uppdaterade timmar och produktens pris
            var totalPrice = _calculatePrice.CalculateTotalPriceAndHours(project.TotalHours, product.Price);

      
            var projectEntity = _projectFactory.Create(project);
            projectEntity.TotalPrice = totalPrice;

            
            var updatedEntity = await _projectRepository.UpdateAsync(
                p => p.Id == project.Id,
                projectEntity
            );

            if (updatedEntity == null)
                return null;

            await _projectRepository.CommitTransactionAsync();
            return _projectFactory.Create(updatedEntity); 
        }
        catch
        {
            await _projectRepository.RollBackTransactionAsync();
            throw;
        }
    }


    //DELETE
    public async Task<bool> DeleteProjectAsync(int id)
    {
        await _projectRepository.BeginTransactionAsync();
        try
        {
            var result = await _projectRepository.DeleteAsync(x => x.Id == id);
            if (!result) 
                return false;

            await _projectRepository.CommitTransactionAsync();
            return result;
        }
        catch
        {
            await _projectRepository.RollBackTransactionAsync();
            throw;
        }
        
    }

    //ALREADY EXISTS
    public async Task<bool> CheckIfProjectExists(Expression<Func<ProjectEntity, bool>> expression)
    {
        return await _projectRepository.AlreadyExistsAsync(expression);
    }
}
