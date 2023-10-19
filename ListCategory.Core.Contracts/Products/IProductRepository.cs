using ListCategory.Core.Contracts.Common;
using ListCategory.Core.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListCategory.Core.Contracts.Products
{
    public interface IProductRepository: IBaseRepository<Product>
    {
        void Update(Product product);
    }
}
