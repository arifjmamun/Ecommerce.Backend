using System.Threading.Tasks;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.Services.Abstractions
{
  public interface IUserService : IBaseService<User>
  {
    Task<User> AddUser(User user);
    Task<string> UpdateAvatar(string userId, string avatarUrl);
  }
}