using Business.Models;
using Data.Entities;
using System.Linq.Expressions;

namespace Business.Interfaces
{
    public interface IUserService
    {
        Task<bool> CheckIfUserExists(Expression<Func<UserEntity, bool>> expression);
        Task CreateUserAsync(User userModel);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<User?>> GetAllUsersAsync();
        Task<User?> GetUserAsync(Expression<Func<UserEntity, bool>> expression);
        Task<User?> UpdateUserAsync(User user);
    }
}