using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using System.Linq.Expressions;

namespace Business.Services;

public class ProjectService(ProjectRepository repository, ProjectFactory _projectFactory) : IProjectService
{
    private readonly ProjectRepository _projectRepository = repository;
    private readonly ProjectFactory _projectFactory = _projectFactory;

    //CREATE
    public async Task CreateProjectAsync(Project projectModel)
    {

        await _projectRepository.BeginTransactionAsync();

        try
        {
            var existingProject = await _projectRepository.GetAsync(x => x.Title == projectModel.Title);
            if (existingProject != null)
                throw new Exception("Project with this title already exists.");

            var projectEntity = _projectFactory.Create(projectModel);
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
            var updatedEntity = await _projectRepository.UpdateAsync(
                p => p.Id == project.Id,
                _projectFactory.Create(project)!
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
