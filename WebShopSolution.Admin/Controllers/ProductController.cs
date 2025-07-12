using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using WebShopSolution.ViewModels.Catalog.Product;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebShopSolution.ViewModels.Catalog.Category;

namespace WebShopSolution.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment)
        {
            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<List<ProductViewModel>>("https://localhost:7236/api/product");
            return View(response);
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var product = await client.GetFromJsonAsync<ProductViewModel>($"https://localhost:7236/api/product/{id}");
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadCategoriesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model, IFormCollection form)
        {
            // RÀNG BUỘC THỦ CÔNG
            if (!model.IsVariantProduct)
            {
                if (model.BasePrice == null || model.BasePrice <= 0)
                {
                    ModelState.AddModelError(nameof(model.BasePrice), "Base price is required for non-variant products.");
                }

                if (model.Quantity == null || model.Quantity < 0)
                {
                    ModelState.AddModelError(nameof(model.Quantity), "Quantity is required for non-variant products.");
                }
            }
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(model);
            }

            var client = _httpClientFactory.CreateClient();

            // 1. Tạo Product
            var productCreate = new ProductCreateRequest
            {
                ProductName = model.ProductName,
                Description = model.Description,
                BasePrice = model.IsVariantProduct ? null : model.BasePrice,
                Quantity = model.IsVariantProduct ? null : model.Quantity,
                Status = model.Status,
                IdCate = model.IdCate
            };

            var response = await client.PostAsJsonAsync("https://localhost:7236/api/product", productCreate);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Tạo sản phẩm thất bại.");
                await LoadCategoriesAsync();
                return View(model);
            }

            var productId = await GetProductIdByNameAsync(client, model.ProductName);

            // 2. Nếu có biến thể thì tạo variant
            if (model.IsVariantProduct)
            {
                int i = 0;
                while (true)
                {
                    var skuKey = $"Variants[{i}].Sku";
                    if (!form.ContainsKey(skuKey)) break;

                    string sku = form[skuKey];
                    string price = form[$"Variants[{i}].Price"];
                    string stock = form[$"Variants[{i}].Stock"];
                    string status = form[$"Variants[{i}].Status"];

                    // Collect attributes
                    var attributes = new List<ProductVariantAttributeCreateRequest>();
                    int j = 0;
                    while (true)
                    {
                        string nameKey = $"Variants[{i}].AttributeNames[{j}]";
                        string valueKey = $"Variants[{i}].AttributeValues[{j}]";
                        if (!form.ContainsKey(nameKey) || !form.ContainsKey(valueKey)) break;

                        var attrName = form[nameKey];
                        var attrValue = form[valueKey];

                        if (!string.IsNullOrWhiteSpace(attrName) && !string.IsNullOrWhiteSpace(attrValue))
                        {
                            attributes.Add(new ProductVariantAttributeCreateRequest
                            {
                                AttributeName = attrName,
                                AttributeValue = attrValue
                            });
                        }

                        j++;
                    }

                    var variantRequest = new ProductVariantCreateRequest
                    {
                        ProductId = productId,
                        Sku = sku,
                        Price = int.Parse(price),
                        Stock = int.Parse(stock),
                        Status = status,
                        Attributes = attributes
                    };

                    await client.PostAsJsonAsync("https://localhost:7236/api/productvariants", variantRequest);

                    i++;
                }
            }



            // 3. Upload ảnh
            if (model.Images != null && model.Images.Any())
            {
                var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", $"product_{productId}");
                Directory.CreateDirectory(uploadPath);

                foreach (var image in model.Images)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    var imageRequest = new ProductImageCreateRequest
                    {
                        ProductId = productId,
                        ImagePath = $"/images/product_{productId}/{fileName}",
                        SortOrder = 0
                    };

                    await client.PostAsJsonAsync("https://localhost:7236/api/productimage", imageRequest);
                }
            }

            return RedirectToAction("Index");
        }




        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var product = await client.GetFromJsonAsync<ProductViewModel>($"https://localhost:7236/api/product/{id}");
            if (product == null) return NotFound();

            var images = await client.GetFromJsonAsync<List<ProductImageViewModel>>($"https://localhost:7236/api/productimage/by-product/{id}");

            var variants = await client.GetFromJsonAsync<List<ProductVariantViewModel>>($"https://localhost:7236/api/productvariants/product/{id}");

            var model = new ProductUpdateViewModel
            {
                IdProduct = product.IdProduct,
                ProductName = product.ProductName,
                Description = product.Description,
                BasePrice = product.BasePrice,
                Quantity = product.Quantity,
                Status = product.Status,
                IdCate = product.IdCate,
                ExistingImages = images,
                Variants = variants // <-- thêm dòng này
            };

            await LoadCategoriesAsync();
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateViewModel model)
        {
            // Kiểm tra ràng buộc nếu không phải sản phẩm có biến thể
            if (!model.IsVariantProduct)
            {
                if (model.BasePrice == null || model.BasePrice <= 0)
                    ModelState.AddModelError(nameof(model.BasePrice), "Base price is required for non-variant products.");

                if (model.Quantity == null || model.Quantity < 0)
                    ModelState.AddModelError(nameof(model.Quantity), "Quantity is required for non-variant products.");
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(model);
            }

            var client = _httpClientFactory.CreateClient();

            var updateRequest = new ProductUpdateRequest
            {
                IdProduct = model.IdProduct,
                ProductName = model.ProductName,
                Description = model.Description,
                BasePrice = model.IsVariantProduct ? null : model.BasePrice,
                Quantity = model.IsVariantProduct ? null : model.Quantity,
                Status = model.Status,
                IdCate = model.IdCate
            };

            var response = await client.PutAsJsonAsync("https://localhost:7236/api/product", updateRequest);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Cập nhật sản phẩm thất bại.");
                await LoadCategoriesAsync();
                return View(model);
            }

            // Upload ảnh mới nếu có
            if (model.Images != null && model.Images.Any())
            {
                var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", $"product_{model.IdProduct}");
                Directory.CreateDirectory(uploadPath);

                foreach (var image in model.Images)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    var imageRequest = new ProductImageCreateRequest
                    {
                        ProductId = model.IdProduct,
                        ImagePath = $"/images/product_{model.IdProduct}/{fileName}",
                        SortOrder = 0
                    };

                    await client.PostAsJsonAsync("https://localhost:7236/api/productimage", imageRequest);
                }
            }

            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var product = await client.GetFromJsonAsync<ProductViewModel>($"https://localhost:7236/api/product/{id}");
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7236/api/product/{id}");
            return RedirectToAction("Index");
        }

        private async Task<int> GetProductIdByNameAsync(HttpClient client, string productName)
        {
            var response = await client.GetFromJsonAsync<List<ProductViewModel>>("https://localhost:7236/api/product");
            return response?.FirstOrDefault(p => p.ProductName == productName)?.IdProduct ?? 0;
        }



        private async Task LoadCategoriesAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var categories = await client.GetFromJsonAsync<List<CategoryViewModel>>("https://localhost:7236/api/category");

            var flatCategories = new List<SelectListItem>();

            void Flatten(List<CategoryViewModel> cats, int level)
            {
                foreach (var cat in cats)
                {
                    flatCategories.Add(new SelectListItem
                    {
                        Value = cat.IdCate.ToString(),
                        Text = new string('-', level * 4) + cat.CateName
                    });

                    if (cat.Children != null && cat.Children.Any())
                        Flatten(cat.Children, level + 1);
                }
            }

            if (categories != null)
                Flatten(categories, 0);

            ViewBag.Categories = flatCategories;
        }


    }
}
