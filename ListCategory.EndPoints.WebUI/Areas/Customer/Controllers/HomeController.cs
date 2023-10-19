
using ListCategory.Core.Contracts.Products;
using ListCategory.Core.Domain.Products;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspTest.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IProductRepository productRepository;

        public HomeController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> products = productRepository.GetAll(includeProperties:"Category");
            return View(products);
        }

        public IActionResult Details(int id)
        {
            Product product = productRepository.Get(u => u.ProductId == id, includeProperties: "Category");
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }


    }
}