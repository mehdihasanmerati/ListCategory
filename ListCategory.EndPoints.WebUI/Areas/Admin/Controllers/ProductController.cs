using ListCategory.Core.Contracts.Categories;
using ListCategory.Core.Contracts.Products;
using ListCategory.Core.Domain.Categories;
using ListCategory.Core.Domain.Products;
using ListCategory.DataAccess.Common;
using ListCategory.DataAccess.Products;
using ListCategory.EndPoints.WebUI.Models.SelectDetail;
using ListCategory.Models.ProductVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AspTest.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IProductRepository ProductRepository;
        private readonly ICategoryRepository category;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(IProductRepository ProductRepository,
                                 ICategoryRepository category,
                                 IWebHostEnvironment webHostEnvironment)
        {
            this.ProductRepository = ProductRepository;
            this.category = category;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> products  = ProductRepository.GetAll(includeProperties:"Category").ToList();
            return View(products);
        }

        public IActionResult UpdateInsert(int? id)
        {
            ProductViewMoel productView = new()
            {
                CategoryList = category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString(),
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //create
                return View(productView);
            }
            else
            {
                //update
                productView.Product = ProductRepository.Get(u => u.ProductId == id);
                return View(productView);
            }
            
        }
        [HttpPost]
        public IActionResult UpdateInsert(ProductViewMoel productMV, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    // delete the old image
                    if (!string.IsNullOrEmpty(productMV.Product.Image))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productMV.Product.Image.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productMV.Product.Image = @"\images\product\" + fileName;
                }

                if (productMV.Product.ProductId == 0)
                {
                    ProductRepository.Add(productMV.Product);
                }
                else
                {
                    ProductRepository.Update(productMV.Product);
                }

                TempData["success"] = "create successed";
                return RedirectToAction("Index");

            }
            else
            {
                productMV.CategoryList = category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString()
                });
                return View(productMV);
            }  
        }

        // Product? ProductFromDb1 = context.Categories.FirstOrDefault(u => u.ProductId == id);
        // Product? ProductFromDb2 = context.Categories.Where(d => d.ProductId == id).FirstOrDefault();

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? ProductFromDb = ProductRepository.Get(x => x.ProductId == id);
        //    if (ProductFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(ProductFromDb);
        //}
        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePost(int? id)
        //{
        //    Product? obj = ProductRepository.Get(x => x.ProductId == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    ProductRepository.Remove(obj);
        //    TempData["success"] = "Delete successed";
        //    return RedirectToAction(nameof(Index));
        //}

        #region API Call

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProducts = ProductRepository.GetAll(includeProperties:"Category").ToList();
            return Json(new {data = objProducts});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
            {
            var productToDeleted = ProductRepository.Get(u => u.ProductId == id);

            if (productToDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, productToDeleted.Image.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            ProductRepository.Remove(productToDeleted);
            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
