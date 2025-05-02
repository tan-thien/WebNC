using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSolution.ViewModels.Catalog.Category
{
    public class CategoryViewModel
    {
        public int IdCate { get; set; }
        public string CateName { get; set; }

        public int? ParentId { get; set; }

        // ✅ Nếu bạn muốn trả về cây danh mục
        public List<CategoryViewModel> Children { get; set; } = new();
    }

}
