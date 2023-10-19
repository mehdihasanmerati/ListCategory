using ListCategory.Core.Contracts.Common;
using ListCategory.Core.Domain.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListCategory.Core.Contracts.Categories
{
    public interface ICategoryRepository: IBaseRepository<Category>
    {
        void Update(Category category);
    }
}
