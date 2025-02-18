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
        
        var existingProject = await _projectRepository.GetAsync(x => x.Title == projectModel.Title);
        if (existingProject != null)
            throw new Exception("Project with this title already exists.");

        var projectEntity = _projectFactory.Create(projectModel);
        await _projectRepository.CreateASync(projectEntity!);
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
        var updatedEntity = await _projectRepository.UpdateAsync(
            p => p.Id == project.Id,
            _projectFactory.Create(project)!
        );

        return updatedEntity != null ? _projectFactory.Create(updatedEntity) : null;
    }

    //DELETE
    public async Task<bool> DeleteProjectAsync(int id)
    {
        var result = await _projectRepository.DeleteAsync(x => x.Id == id);
        return result;
    }

    //ALREADY EXISTS
    public async Task<bool> CheckIfProjectExists(Expression<Func<ProjectEntity, bool>> expression)
    {
        return await _projectRepository.AlreadyExistsAsync(expression);
    }
}
