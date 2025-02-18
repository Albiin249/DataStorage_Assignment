using Business.Models;
using Data.Entities;
using System.Linq.Expressions;

namespace Business.Interfaces
{
    public interface IProjectService
    {
        Task<bool> CheckIfProjectExists(Expression<Func<ProjectEntity, bool>> expression);
        Task CreateProjectAsync(Project projectModel);
        Task<bool> DeleteProjectAsync(int id);
        Task<IEnumerable<Project?>> GetAllProjectsAsync();
        Task<Project?> GetProjectAsync(Expression<Func<ProjectEntity, bool>> expression);
        Task<Project?> UpdateProjectAsync(Project project);
    }
}