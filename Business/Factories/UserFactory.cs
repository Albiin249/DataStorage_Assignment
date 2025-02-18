using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class UserFactory
{
    //Från UserModel till Entity
    public static UserEntity? Create(User model) => model == null ? null : new()
    {
        Id = model.Id,
        FirstName = model.FirstName,
        LastName = model.LastName,
        Email = model.Email
    };

    //Från UserEntity till Model
    public static User? Create(UserEntity entity) => entity == null ? null : new()
    {
        Id = entity.Id,
        FirstName = entity.FirstName,
        LastName = entity.LastName,
        Email = entity.Email
    };
}
