using ListCategory.Core.Contracts.Categories;
using ListCategory.Core.Domain.Categories;
using ListCategory.DataAccess.Common;
using ListCategory.EndPoints.WebUI.Models.SelectDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ListCategory.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            List<Category> categorries = categoryRepository.GetAll().ToList();
            return View(categorries);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The name must not be same as displayorder");
            }
            if (ModelState.IsValid)
            {
                categoryRepository.Add(category);
                TempData["success"] = "create successed";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = categoryRepository.Get(x => x.CategoryId == id);
            // Categorry? categoryFromDb1 = context.Categories.FirstOrDefault(u => u.CategoryId == id);
            // Categorry? categoryFromDb2 = context.Categories.Where(d => d.CategoryId == id).FirstOrDefault();
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category opj)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Update(opj);
                TempData["success"] = "Edit successed";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = categoryRepository.Get(x => x.CategoryId == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = categoryRepository.Get(x => x.CategoryId == id);
            if (obj == null)
            {
                return NotFound();
            }
            categoryRepository.Remove(obj);
            TempData["success"] = "Delete successed";
            return RedirectToAction(nameof(Index));

        }
    }
}
