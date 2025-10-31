using Backend.Application.DTOs;

namespace Backend.Domain.Interfaces.Repositories
{
  public interface IUserRepository
  {
    Task<UserDto> Create(UserCreateDto user);
    Task<IEnumerable<UserDto>> GetAll();
    Task<UserDto?> GetById(Guid id);
    Task<UserDto?> Update(UserUpdateDto user);
    Task<bool> Delete(Guid id);
    Task<UserDto?> GetByEmail(string email);
  }
}