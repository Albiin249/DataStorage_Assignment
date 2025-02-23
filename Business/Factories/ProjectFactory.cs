using Business.Helpers;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public class ProjectFactory(ProjectNumberGenerator projectNumberGenerator)
{
    private readonly ProjectNumberGenerator _projectNumberGenerator = projectNumberGenerator;

   
    //Från ProjectModel till Entity
    public ProjectEntity? Create(Project model) => model == null ? null : new()
    {
        Id = model.Id,
        Title = model.Title,
        ProjectNumber = _projectNumberGenerator.GenerateProjectNumber(),
        Description = model.Description,
        StartDate = model.StartDate,
        EndDate = model.EndDate,
        CustomerId = model.CustomerId,
        StatusId = model.StatusId,
        UserId = model.UserId,
        ProductId = model.ProductId,
        TotalHours = model.TotalHours,
        TotalPrice = model.TotalPrice
    };

    //Från ProjectEntity till Model
    public Project? Create(ProjectEntity entity) => entity == null ? null : new()
    {
        Id = entity.Id,
        Title = entity.Title,
        ProjectNumber = entity.ProjectNumber,
        Description = entity.Description,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        CustomerId = entity.CustomerId,
        StatusId = entity.StatusId,
        UserId = entity.UserId,
        ProductId = entity.ProductId,
        TotalHours = entity.TotalHours,
        TotalPrice = entity.TotalPrice
    };
}
