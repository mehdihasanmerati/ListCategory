using ListCategory.Core.Contracts.Products;
using ListCategory.Core.Domain.Categories;
using ListCategory.Core.Domain.Products;
using ListCategory.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListCategory.DataAccess.Products
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public void Update(Product obj)
        {
            var objFromDb = context.Products.FirstOrDefault(p => p.ProductId == obj.ProductId);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price100 = obj.Price100;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Author = obj.Author;
                if(obj.Image != null)
                {
                    objFromDb.Image = obj.Image;
                }
            }
            context.SaveChanges();
        }
    }
}
