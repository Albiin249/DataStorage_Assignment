using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class StatusTypeFactory
{
    //Från StatusModel till Entity
    public static StatusTypeEntity? Create(StatusType model) => model == null ? null : new()
    {
        Id = model.Id,
        StatusName = model.StatusName,
    };

    //Från StatusEntity till Model
    public static StatusType? Create(StatusTypeEntity entity) => entity == null ? null : new()
    {
        Id = entity.Id,
        StatusName = entity.StatusName
    };
}
