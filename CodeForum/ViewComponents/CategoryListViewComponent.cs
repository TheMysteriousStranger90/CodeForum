using CodeForum.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodeForum.ViewComponents;

public class CategoryListViewComponent : ViewComponent
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryListViewComponent(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await _categoryRepository.ListAllAsync();
        return View(categories);
    }
}