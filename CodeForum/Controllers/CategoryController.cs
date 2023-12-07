using CodeForum.Interfaces;
using CodeForum.Models;
using CodeForum.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodeForum.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public CategoryController(ICategoryRepository categoryRepository, UserManager<ApplicationUser> userManager)
    {
        _categoryRepository = categoryRepository;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var categories = await _categoryRepository.ListAllAsync();
            return View((IEnumerable<Category>)categories);
        }
        catch (Exception ex)
        {
            return BadRequest("Failed to get topics");
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryAddViewModel model)
    {
        if (ModelState.IsValid)
        {
            var category = new Category { Name = model.Name };
            _categoryRepository.Add(category);
            await _categoryRepository.SaveChangesAsync();

            return RedirectToAction("Index", "Category");
        }

        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        CategoryEditViewModel model = new CategoryEditViewModel
            { Id = category.Id, Name = category.Name };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CategoryEditViewModel model)
    {
        if (ModelState.IsValid)
        {
            var category = await _categoryRepository.GetByIdAsync(model.Id);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = model.Name;

            _categoryRepository.Update(category);

            if (await _categoryRepository.SaveChangesAsync())
            {
                return RedirectToAction("Index", "Category");
            }
            else
            {
                return BadRequest($"Failed!!!");
            }
        }

        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync();

        return RedirectToAction("Index", "Category");
    }
}