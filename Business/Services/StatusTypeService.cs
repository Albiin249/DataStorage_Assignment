using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System.Linq.Expressions;

namespace Business.Services;

public class StatusTypeService(StatusTypeRepository repository) : IStatusTypeService
{
    private readonly StatusTypeRepository _statusRepository = repository;


    //CREATE
    public async Task CreateStatusAsync(StatusType statusModel)
    {
        var existingStatus = await _statusRepository.GetAsync(x => x.StatusName == statusModel.StatusName);
        if (existingStatus != null)
            throw new Exception("This status already exists.");

        var statusEntity = StatusTypeFactory.Create(statusModel);
        await _statusRepository.CreateASync(statusEntity!);
    }

    //READ
    public async Task<IEnumerable<StatusType?>> GetAllStatusTypesAsync()
    {
        var statusEntities = await _statusRepository.GetAllAsync();
        return statusEntities.Select(StatusTypeFactory.Create);
    }

    public async Task<StatusType?> GetStatusTypeAsync(Expression<Func<StatusTypeEntity, bool>> expression)
    {
        var statusEntity = await _statusRepository.GetAsync(expression);
        return StatusTypeFactory.Create(statusEntity);
    }

    //UPDATE
    public async Task<StatusType?> UpdateStatusTypeAsync(StatusType status)
    {
        var updatedEntity = await _statusRepository.UpdateAsync(
            s => s.Id == status.Id,
            StatusTypeFactory.Create(status)!
        );

        return updatedEntity != null ? StatusTypeFactory.Create(updatedEntity) : null;
    }

    //DELETE
    public async Task<bool> DeleteStatusTypeAsync(int id)
    {
        var result = await _statusRepository.DeleteAsync(x => x.Id == id);
        return result;
    }

    //EXISTS
    public async Task<bool> CheckIfStatusTypeExists(Expression<Func<StatusTypeEntity, bool>> expression)
    {
        return await _statusRepository.AlreadyExistsAsync(expression);
    }
}
