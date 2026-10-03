# WebShopSolution

WebShopSolution là solution thương mại điện tử viết bằng ASP.NET Core MVC và Web API. Solution tách giao diện khách hàng, giao diện quản trị, API, nghiệp vụ ứng dụng, truy cập dữ liệu và các mô hình trao đổi dữ liệu thành những project riêng.

## Chức năng

### Website khách hàng

- Đăng ký, đăng nhập và đăng xuất; tài khoản khách hàng được lưu trong session.
- Duyệt sản phẩm đang hoạt động, lọc theo danh mục, phân trang và xem chi tiết sản phẩm, hình ảnh, biến thể và thuộc tính.
- Quản lý giỏ hàng: thêm sản phẩm, thay đổi số lượng, xóa sản phẩm và chọn sản phẩm để thanh toán.
- Xem và áp dụng voucher; hỗ trợ giảm giá theo số tiền hoặc phần trăm, giới hạn mức giảm và lượt sử dụng.
- Tạo đơn hàng, cập nhật tồn kho sau khi đặt hàng, xem lịch sử đơn hàng và thanh toán qua PayPal.
- Xem và cập nhật hồ sơ khách hàng.
- Chat hỗ trợ với một số câu trả lời cho câu hỏi thường gặp và tích hợp OpenRouter cho câu hỏi khác.

### Trang quản trị

- Đăng nhập bằng tài khoản có vai trò `Admin`.
- Quản lý danh mục cha/con, sản phẩm, hình ảnh sản phẩm, biến thể và thuộc tính.
- Xem danh sách/chi tiết đơn hàng và cập nhật trạng thái.
- Tạo, sửa, xóa voucher; hỗ trợ mẫu voucher và sinh mã tự động.
- Quản lý tài khoản, tạo tài khoản và thay đổi vai trò.
- Xem thống kê đơn hàng theo ngày, tháng hoặc năm; xuất báo cáo Excel/PDF. Các thống kê doanh thu trong service chỉ tính đơn hàng có trạng thái `Đã giao`.

### Web API

API REST cung cấp các thao tác cho tài khoản, danh mục, sản phẩm, ảnh, biến thể, đơn hàng và voucher. Swagger được bật trong môi trường Development.

| Nhóm | Một số route |
| --- | --- |
| Tài khoản | `POST /api/account/register`, `POST /api/account/login`, `PUT /api/account/change-password` |
| Danh mục | `GET /api/category`, `GET /api/category/tree`, `GET /api/category/{id}`, `POST /api/category`, `PUT /api/category/{id}`, `DELETE /api/category/{id}` |
| Sản phẩm | `GET /api/product`, `GET /api/product/{id}`, `POST /api/product`, `PUT /api/product`, `DELETE /api/product/{id}` |
| Ảnh sản phẩm | `POST /api/productimage`, `GET /api/productimage/by-product/{productId}` |
| Biến thể | `POST /api/productvariants`, `GET /api/productvariants/product/{productId}` |
| Đơn hàng | `GET /api/order/all`, `GET /api/order/user/{userId}`, `GET /api/order/{id}`, `PUT /api/order/updatestatus` |
| Voucher | `GET /api/voucher/all`, `GET /api/voucher/{id}`, `POST /api/voucher/create`, `PUT /api/voucher/update`, `DELETE /api/voucher/delete/{id}` |

## Kiến trúc solution

```text
WebShopSolution.WebApp ─────┐
WebShopSolution.Admin ──────┼──> WebShopSolution.Application ──> WebShopSolution.Data ──> SQL Server
WebShopSolution.API ────────┘                │
							  WebShopSolution.ViewModels
```

- **WebShopSolution.WebApp**: storefront ASP.NET Core MVC; xử lý giao diện, session, giỏ hàng và luồng checkout.
- **WebShopSolution.Admin**: giao diện quản trị MVC và Razor Pages. Một số màn hình gọi API qua HTTP.
- **WebShopSolution.API**: các endpoint REST và Swagger.
- **WebShopSolution.Application**: service nghiệp vụ cho tài khoản, khách hàng, giỏ hàng, danh mục, sản phẩm, biến thể, đơn hàng, voucher, PayPal, chat và thống kê.
- **WebShopSolution.Data**: entity, `WebShopDbContext`, repository, Unit of Work và EF Core migrations.
- **WebShopSolution.ViewModels**: request, response và view model dùng giữa các project.
- **WebShopSolution.Utilities**: project tiện ích dùng chung.

Mô hình dữ liệu chính gồm tài khoản/khách hàng, danh mục phân cấp, sản phẩm, ảnh, biến thể và thuộc tính, giỏ hàng/giỏ hàng chi tiết, đơn hàng/chi tiết đơn hàng, voucher và voucher gắn với khách hàng. EF Core dùng SQL Server; migration khởi tạo nằm trong `WebShopSolution.Data/Migrations`.

## Công nghệ

- .NET 8 / ASP.NET Core MVC, Razor Pages và Web API
- Entity Framework Core với SQL Server
- Swagger / Swashbuckle
- PayPal Checkout SDK
- EPPlus để xuất Excel; QuestPDF để tạo PDF
- Session và `HttpClient` cho một số luồng giao tiếp giữa các ứng dụng

## Yêu cầu

- .NET 8 SDK
- SQL Server (local hoặc máy chủ có thể truy cập)
- Visual Studio 2022 hoặc VS Code (không bắt buộc)
- Tài khoản/khóa PayPal và OpenRouter nếu cần dùng thanh toán hoặc chat AI

## Cấu hình

Các host kết nối cơ sở dữ liệu bằng khóa `ConnectionStrings:WebShopDb`. Hãy cấu hình giá trị này trong User Secrets hoặc biến môi trường `ConnectionStrings__WebShopDb` cho từng host cần chạy. Cấu hình storefront hiện có khóa connection string mang tên khác với tên mà `Program.cs` đọc, vì vậy cần chuẩn hóa tên hoặc đặt biến môi trường nói trên trước khi khởi chạy.

Storefront cần thêm các khóa sau nếu sử dụng các tích hợp tương ứng:

| Khóa | Mục đích |
| --- | --- |
| `PayPal:ClientId` | PayPal client ID |
| `PayPal:Secret` | PayPal secret |
| `PayPal:Mode` | `sandbox` hoặc `live` |
| `OpenRouter:ApiKey` | Khóa gọi dịch vụ chat OpenRouter |
| `AdminBaseUrl` | Địa chỉ Admin để storefront tạo URL hình ảnh; mặc định theo launch profile là `https://localhost:7274` |

Ví dụ đặt cấu hình bằng User Secrets cho storefront:

```powershell
dotnet user-secrets init --project WebShopSolution.WebApp
dotnet user-secrets set "ConnectionStrings:WebShopDb" "<connection-string>" --project WebShopSolution.WebApp
dotnet user-secrets set "PayPal:ClientId" "<client-id>" --project WebShopSolution.WebApp
dotnet user-secrets set "PayPal:Secret" "<client-secret>" --project WebShopSolution.WebApp
dotnet user-secrets set "PayPal:Mode" "sandbox" --project WebShopSolution.WebApp
dotnet user-secrets set "OpenRouter:ApiKey" "<api-key>" --project WebShopSolution.WebApp
```

Không commit connection string, mật khẩu hoặc API key vào repository. Trong quá trình rà soát, các file cấu hình hiện có chứa giá trị khóa dịch vụ; nếu các khóa này đã được chia sẻ hoặc đẩy lên remote, hãy thu hồi/thay khóa và chuyển sang User Secrets hoặc secret store của môi trường triển khai.

## Chạy ứng dụng

Từ thư mục gốc solution:

```powershell
dotnet restore Web.sln
dotnet build Web.sln
```

Sau khi cấu hình connection string, áp dụng migration bằng EF Core CLI:

```powershell
dotnet tool install --global dotnet-ef
dotnet ef database update --project WebShopSolution.Data --startup-project WebShopSolution.API
```

Chạy các host trong các terminal riêng. Admin gọi API tại `https://localhost:7236`, nên giữ API ở cổng này khi chạy theo cấu hình hiện tại.

```powershell
dotnet run --project WebShopSolution.API --launch-profile https
dotnet run --project WebShopSolution.Admin --launch-profile https
dotnet run --project WebShopSolution.WebApp --launch-profile https
```

| Ứng dụng | URL mặc định HTTPS | Ghi chú |
| --- | --- | --- |
| API | `https://localhost:7236` | Swagger tại `/swagger` trong Development |
| Admin | `https://localhost:7274` | Giao diện quản trị |
| Storefront | `https://localhost:7153` | Website khách hàng |

Các URL trên lấy từ `Properties/launchSettings.json`; có thể thay đổi theo môi trường. Admin hiện gọi API bằng URL cố định, nên nếu đổi cổng API cần cập nhật các URL được sử dụng trong Admin. `AdminBaseUrl` cũng cần khớp với địa chỉ Admin để ảnh sản phẩm hiển thị chính xác.

## Lưu ý triển khai

- Cần có tài khoản phù hợp trong cơ sở dữ liệu để đăng nhập; solution không cung cấp thông tin tài khoản mẫu trong README này.
- Luồng PayPal đang dùng URL callback cố định cho môi trường local. Cần cấu hình callback theo domain khi triển khai.
- Session được cấu hình với thời hạn không hoạt động 30 phút.
- Cần rà soát cấu hình bảo mật, quyền truy cập các route quản trị/API, thông tin đăng nhập và cấu hình HTTPS trước khi đưa lên môi trường thật.
