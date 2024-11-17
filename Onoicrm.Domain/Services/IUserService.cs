using Onoicrm.Domain.Models;

namespace Onoicrm.Domain.Services;

public interface IUserService
{
    
    Task<UserManagerResponse> RegisterUserAsync(RegisterModel model);
    Task<UserManagerResponse> LoginUserAsync(LoginModel model);
    Task<bool> CheckClinicAsync(string userName,CheckClinicModel model);
    Task<bool> ResetPasswordAsync(ResetPasswordModel model);
    Task<bool> SetRoleAsync(SetRoleModel model);
    Task<bool> DeleteRoleAsync(SetRoleModel model);
}