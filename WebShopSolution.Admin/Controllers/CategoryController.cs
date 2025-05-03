using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebShopSolution.ViewModels.Catalog.Category;

namespace WebShopSolution.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly HttpClient _httpClient;

        public CategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7236"); // Cập nhật đúng base URL nếu khác
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
                return RedirectToAction("Login", "AdminAuth");

            var categories = await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>("/api/category");
            return View(categories);
        }

        public async Task<IActionResult> Create()
        {
            var categoryTree = await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>("/api/category");

            var flatCategories = new List<CategoryViewModel>();

            void Flatten(List<CategoryViewModel> categories, int level)
            {
                foreach (var cate in categories)
                {
                    cate.CateName = new string('-', level * 4) + cate.CateName;
                    flatCategories.Add(cate);
                    if (cate.Children != null && cate.Children.Any())
                        Flatten(cate.Children, level + 1);
                }
            }

            Flatten(categoryTree, 0);
            ViewBag.Categories = flatCategories;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateRequest request)
        {
            var res = await _httpClient.PostAsJsonAsync("/api/category", request);
            if (res.IsSuccessStatusCode) return RedirectToAction(nameof(Index));

            // load lại danh mục cha nếu lỗi
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>("/api/category");
            ViewBag.Categories = categories;
            return View(request);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var category = await _httpClient.GetFromJsonAsync<CategoryViewModel>($"/api/category/{id}");
            var allCategories = await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>("/api/category");

            // Lọc danh mục cha hợp lệ (không cho chọn chính nó hoặc các danh mục con của nó)
            var availableParents = new List<CategoryViewModel>();

            void Flatten(List<CategoryViewModel> categories, int level)
            {
                foreach (var cate in categories)
                {
                    cate.CateName = new string('-', level * 4) + cate.CateName;

                    // Đảm bảo không chọn chính nó làm Parent
                    if (cate.IdCate != id)
                    {
                        availableParents.Add(cate);
                    }
                    if (cate.Children != null && cate.Children.Any())
                        Flatten(cate.Children, level + 1);
                }
            }

            Flatten(allCategories, 0);

            ViewBag.ParentCategories = availableParents;

            return View(new CategoryUpdateRequest
            {
                IdCate = category.IdCate,
                CateName = category.CateName,
                ParentId = category.ParentId
            });
        }


        private bool IsDescendant(CategoryViewModel potentialParent, int categoryId)
        {
            while (potentialParent.ParentId.HasValue)
            {
                if (potentialParent.ParentId == categoryId)
                {
                    return true;
                }
                // Lấy danh mục cha của danh mục hiện tại để tiếp tục kiểm tra
                potentialParent = _httpClient.GetFromJsonAsync<CategoryViewModel>($"/api/category/{potentialParent.ParentId}").Result;
            }
            return false;
        }



        [HttpPost]
        public async Task<IActionResult> Edit(CategoryUpdateRequest request)
        {
            var res = await _httpClient.PutAsJsonAsync($"/api/category/{request.IdCate}", request);
            if (res.IsSuccessStatusCode) return RedirectToAction(nameof(Index));

            // reload lại danh sách danh mục cha nếu có lỗi
            var allCategories = await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>("/api/category");
            ViewBag.ParentCategories = allCategories.Where(c => c.IdCate != request.IdCate).ToList();

            return View(request);
        }


        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync($"/api/category/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
