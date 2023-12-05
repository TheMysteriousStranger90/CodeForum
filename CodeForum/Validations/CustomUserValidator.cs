using CodeForum.Models;
using Microsoft.AspNetCore.Identity;

namespace CodeForum.Validations;

public class CustomUserValidator : IUserValidator<ApplicationUser>
{
    public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
    {
        List<IdentityError> errors = new List<IdentityError>();
 
        if (user.Email.ToLower().EndsWith("@spam.com"))
        {
            errors.Add(new IdentityError
            {
                Description = "Choose another mail service"
            });
        }
        if (user.UserName.Contains("admin"))
        {
            errors.Add(new IdentityError
            {
                Description = "Not contain the word 'admin'!!!"
            });
        }
        return Task.FromResult(errors.Count == 0 ?
            IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
    }
}