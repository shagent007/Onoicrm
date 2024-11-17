using Microsoft.AspNetCore.Identity;

namespace Onoicrm.Domain.Exceptions;

[Serializable]
public class UserRegistrationException : Exception
{
    public IEnumerable<IdentityError> Errors { get; set; } 
    public UserRegistrationException(IEnumerable<IdentityError> errors) : base($"Есть ошибки не возможно зарегистрироваться")
    {
        Errors = errors;
    }
}