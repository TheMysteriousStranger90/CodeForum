using CodeForum.Interfaces;
using CodeForum.Models;
using CodeForum.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodeForum.Controllers;

[Authorize(Roles = "Administrator")]
public class UsersController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFileUploadService _fileUploadService;
    private readonly IWebHostEnvironment _env;

    public UsersController(UserManager<ApplicationUser> userManager, IFileUploadService fileUploadService,
        IWebHostEnvironment env)
    {
        _userManager = userManager;
        _fileUploadService = fileUploadService;
        _env = env;
    }

    public IActionResult Index() => View(_userManager.Users.ToList());

    public async Task<IActionResult> Edit(string id)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        EditUserViewModel model = new EditUserViewModel
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Bio = user.Bio,
            CurrentProfilePicture = user.ProfilePicture
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditUserViewModel model, IFormFile ProfilePicture)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(model.Id);
        if (user != null)
        {
            if (ProfilePicture != null)
            {
                var fileName = Path.GetFileName(ProfilePicture.FileName);
                var path = Path.Combine(_env.WebRootPath, "images", "profiles");
                await _fileUploadService.UploadAsync(path, fileName, ProfilePicture);

                var relativePath = "/images/profiles/" + fileName;
                model.CurrentProfilePicture = relativePath;
                user.ProfilePicture = relativePath;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.Bio = model.Bio;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, "User not found");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            IdentityResult result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, "User not found");
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> ChangePassword(string id)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                IdentityResult result =
                    await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found");
            }
        }

        return View(model);
    }
}