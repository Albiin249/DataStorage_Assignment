using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using System.Linq.Expressions;

namespace Business.Services;

public class UserService(UserRepository userRepository) : IUserService
{
    private readonly UserRepository _userRepository = userRepository;

    //CREATE
    public async Task CreateUserAsync(User userModel)
    {
        var existingUser = await _userRepository.GetAsync(x => x.Email == userModel.Email);
        if (existingUser != null)
            throw new Exception("User with this Email does already exist.");

        var userEntity = UserFactory.Create(userModel);
        await _userRepository.CreateASync(userEntity!);
    }

    //READ
    public async Task<IEnumerable<User?>> GetAllUsersAsync()
    {
        var userEntities = await _userRepository.GetAllAsync();
        return userEntities.Select(UserFactory.Create);
    }

    public async Task<User?> GetUserAsync(Expression<Func<UserEntity, bool>> expression)
    {
        var userEntity = await _userRepository.GetAsync(expression);
        return UserFactory.Create(userEntity);
    }

    //UPDATE
    public async Task<User?> UpdateUserAsync(User user)
    {
        var updatedEntity = await _userRepository.UpdateAsync(
            u => u.Id == user.Id,
            UserFactory.Create(user)!
        );

        return updatedEntity != null ? UserFactory.Create(updatedEntity) : null;
    }

    //DELETE
    public async Task<bool> DeleteUserAsync(int id)
    {
        var result = await _userRepository.DeleteAsync(x => x.Id == id);
        return result;
    }

    //EXISTS
    public async Task<bool> CheckIfUserExists(Expression<Func<UserEntity, bool>> expression)
    {
        return await _userRepository.AlreadyExistsAsync(expression);
    }
}
