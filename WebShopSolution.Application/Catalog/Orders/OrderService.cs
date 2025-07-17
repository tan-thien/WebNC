using WebShopSolution.Application.Catalog.Orders;
using WebShopSolution.Data.Entities;
using WebShopSolution.Data.UnitOfWork;
using WebShopSolution.ViewModels.Catalog.Order;
using WebShopSolution.ViewModels.Catalog.OrderDetail;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShopSolution.ViewModels.Catalog.CartItem;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public OrderService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<bool> CreateOrderAsync(int userId, OrderCreateRequest request)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            var customer = await _unitOfWork.Customers.GetByAccountIdAsync(userId);
            if (customer == null) return false;

            int? voucherId = null;
            int? customerVoucherId = null;

            // ✅ Xử lý giảm giá từ mã khuyến mãi (nếu có)
            if (!string.IsNullOrEmpty(request.VoucherCode))
            {
                var voucher = await _unitOfWork.Vouchers
                    .GetFirstOrDefaultAsync(v => v.Code == request.VoucherCode && v.IsActive && v.Quantity > v.Used);

                if (voucher != null)
                {
                    // Tìm CustomerVoucher chưa dùng
                    var customerVoucher = await _unitOfWork.CustomerVouchers
                        .GetFirstOrDefaultAsync(cv => cv.VoucherId == voucher.VoucherId && cv.IdCus == customer.IdCus && !cv.IsUsed);

                    if (customerVoucher == null) return false;

                    int discount = 0;
                    if (voucher.DiscountType == 0) // Fixed
                        discount = voucher.DiscountValue;
                    else if (voucher.DiscountType == 1) // Percent
                        discount = request.TotalAmount * voucher.DiscountValue / 100;

                    if (voucher.MaxDiscountValue.HasValue && discount > voucher.MaxDiscountValue.Value)
                        discount = voucher.MaxDiscountValue.Value;

                    request.TotalAmount = Math.Max(0, request.TotalAmount - discount);

                    // ✅ tăng số lượt dùng
                    voucher.Used++;
                    await _unitOfWork.Vouchers.UpdateAsync(voucher);

                    // ✅ đánh dấu CustomerVoucher đã dùng
                    customerVoucher.IsUsed = true;
                    customerVoucher.UsedAt = DateTime.Now;
                    await _unitOfWork.CustomerVouchers.UpdateAsync(customerVoucher);

                    voucherId = voucher.VoucherId;
                    customerVoucherId = customerVoucher.CustomerVoucherId;
                }
            }

            var order = new Order
            {
                OrderDate = DateTime.Now,
                ShippingAddress = string.IsNullOrEmpty(request.ShippingAddress) ? customer.Address : request.ShippingAddress,
                Phone = string.IsNullOrEmpty(request.Phone) ? customer.Phone : request.Phone,
                TotalAmount = request.TotalAmount,
                IdCus = customer.IdCus,
                OrderStatus = "Chờ xử lý",
                VoucherId = voucherId,
                CustomerVoucherId = customerVoucherId
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            foreach (var item in request.Items)
            {
                int productId = item.ProductId;

                if (item.VariantId.HasValue && productId == 0)
                {
                    var variant = await _unitOfWork.ProductVariants.GetByIdAsync(item.VariantId.Value);
                    if (variant != null) productId = variant.ProductId;
                }

                var detail = new OrderDetail
                {
                    OrderId = order.IdOrder,
                    ProductId = productId,
                    VariantId = item.VariantId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };

                await _unitOfWork.OrderDetails.AddAsync(detail);
            }

            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }


    public async Task<List<OrderViewModel>> GetOrdersByUserIdAsync(int userId)
    {
        var customer = await _unitOfWork.Customers.GetByAccountIdAsync(userId);
        if (customer == null) return new List<OrderViewModel>();

        var orders = await _unitOfWork.Orders
            .GetWithIncludeAsync(o => o.IdCus == customer.IdCus, "OrderDetails");

        var result = new List<OrderViewModel>();
        var adminBaseUrl = _configuration["AdminBaseUrl"];

        foreach (var order in orders)
        {
            var orderVm = new OrderViewModel
            {
                IdOrder = order.IdOrder,
                OrderDate = order.OrderDate,
                NgayGiao = order.NgayGiao,
                OrderStatus = order.OrderStatus,
                ShippingAddress = order.ShippingAddress,
                Phone = order.Phone,
                TotalAmount = order.TotalAmount,
                CustomerId = customer.IdCus,
                CustomerName = customer.CusName,
                OrderDetails = new List<OrderDetailViewModel>()
            };

            foreach (var od in order.OrderDetails)
            {
                string productName = "";
                string imagePath = $"{adminBaseUrl}/images/no-image.png";
                string variantInfo = "-";

                var product = await _unitOfWork.Products.GetByIdAsync(od.ProductId);
                if (product != null)
                {
                    productName = product.ProductName;

                    var productImage = await _unitOfWork.ProductImages
                        .GetFirstOrDefaultAsync(img => img.ProductId == product.IdProduct, q => q.OrderBy(i => i.SortOrder));

                    if (productImage != null && !string.IsNullOrEmpty(productImage.ImagePath))
                    {
                        imagePath = $"{adminBaseUrl}/{productImage.ImagePath.TrimStart('/')}";
                    }
                }

                if (od.VariantId.HasValue)
                {
                    var variant = (await _unitOfWork.ProductVariants
                        .GetWithIncludeAsync(v => v.Id == od.VariantId.Value, "ProductVariantAttributes"))
                        .FirstOrDefault();

                    if (variant?.ProductVariantAttributes != null)
                    {
                        variantInfo = string.Join(", ", variant.ProductVariantAttributes.Select(a => $"{a.AttributeName}: {a.AttributeValue}"));
                    }
                }

                orderVm.OrderDetails.Add(new OrderDetailViewModel
                {
                    IdOrderDetail = od.IdOrderDetail,
                    ProductId = od.ProductId,
                    VariantId = od.VariantId,
                    Quantity = od.Quantity,
                    Price = od.Price,
                    ProductName = productName,
                    ImagePath = imagePath,
                    VariantInfo = variantInfo
                });
            }

            result.Add(orderVm);
        }

        return result;
    }
    public async Task<(bool IsValid, int DiscountAmount, int OriginalTotal, string Message, int? VoucherId, int? CustomerVoucherId)> ApplyVoucherAsync(string voucherCode, int originalTotal, int userId)
    {
        var customer = await _unitOfWork.Customers.GetByAccountIdAsync(userId);
        if (customer == null)
            return (false, 0, originalTotal, "Không tìm thấy khách hàng.", null, null);

        var voucher = await _unitOfWork.Vouchers
            .GetFirstOrDefaultAsync(v => v.Code == voucherCode && v.IsActive && v.Quantity > v.Used);

        if (voucher == null)
            return (false, 0, originalTotal, "Mã giảm giá không hợp lệ hoặc đã hết lượt sử dụng.", null, null);

        // ✅ Tìm CustomerVoucher đã tồn tại
        var customerVoucher = await _unitOfWork.CustomerVouchers
            .GetFirstOrDefaultAsync(cv => cv.VoucherId == voucher.VoucherId && cv.IdCus == customer.IdCus);

        // ✅ Nếu chưa có -> tạo mới
        if (customerVoucher == null)
        {
            customerVoucher = new CustomerVoucher
            {
                VoucherId = voucher.VoucherId,
                IdCus = customer.IdCus,
                IsUsed = false,
                UsedAt = null
            };
            await _unitOfWork.CustomerVouchers.AddAsync(customerVoucher);
            await _unitOfWork.SaveChangesAsync();
        }

        // ✅ Nếu đã sử dụng rồi
        if (customerVoucher.IsUsed)
            return (false, 0, originalTotal, "Bạn đã sử dụng mã này.", voucher.VoucherId, customerVoucher.CustomerVoucherId);

        // ✅ Tính giảm giá
        int discount = 0;
        if (voucher.DiscountType == 0) // Fixed
            discount = voucher.DiscountValue;
        else if (voucher.DiscountType == 1) // Percent
            discount = originalTotal * voucher.DiscountValue / 100;

        if (voucher.MaxDiscountValue.HasValue && discount > voucher.MaxDiscountValue.Value)
            discount = voucher.MaxDiscountValue.Value;

        return (true, discount, originalTotal, "Áp dụng mã thành công.", voucher.VoucherId, customerVoucher.CustomerVoucherId);
    }


    //admin
    public async Task<List<OrderViewModel>> GetAllOrdersAsync()
    {
        var orders = await _unitOfWork.Orders.GetWithIncludeAsync(x => true, "OrderDetails");
        var result = new List<OrderViewModel>();
        var adminBaseUrl = _configuration["AdminBaseUrl"];

        foreach (var order in orders)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(order.IdCus);

            var orderVm = new OrderViewModel
            {
                IdOrder = order.IdOrder,
                OrderDate = order.OrderDate,
                NgayGiao = order.NgayGiao,
                OrderStatus = order.OrderStatus,
                ShippingAddress = order.ShippingAddress,
                Phone = order.Phone,
                TotalAmount = order.TotalAmount,
                CustomerId = customer?.IdCus ?? 0,
                CustomerName = customer?.CusName ?? "(Không có tên)",
                OrderDetails = new List<OrderDetailViewModel>()
            };

            foreach (var od in order.OrderDetails)
            {
                string productName = "";
                string imagePath = $"{adminBaseUrl}/images/no-image.png";
                string variantInfo = "-";

                var product = await _unitOfWork.Products.GetByIdAsync(od.ProductId);
                if (product != null)
                {
                    productName = product.ProductName;

                    var productImage = await _unitOfWork.ProductImages
                        .GetFirstOrDefaultAsync(img => img.ProductId == product.IdProduct, q => q.OrderBy(i => i.SortOrder));

                    if (productImage != null && !string.IsNullOrEmpty(productImage.ImagePath))
                    {
                        imagePath = $"{adminBaseUrl}/{productImage.ImagePath.TrimStart('/')}";
                    }
                }

                if (od.VariantId.HasValue)
                {
                    var variant = (await _unitOfWork.ProductVariants
                        .GetWithIncludeAsync(v => v.Id == od.VariantId.Value, "ProductVariantAttributes"))
                        .FirstOrDefault();

                    if (variant?.ProductVariantAttributes != null)
                    {
                        variantInfo = string.Join(", ", variant.ProductVariantAttributes.Select(a => $"{a.AttributeName}: {a.AttributeValue}"));
                    }
                }

                orderVm.OrderDetails.Add(new OrderDetailViewModel
                {
                    IdOrderDetail = od.IdOrderDetail,
                    ProductId = od.ProductId,
                    VariantId = od.VariantId,
                    Quantity = od.Quantity,
                    Price = od.Price,
                    ProductName = productName,
                    ImagePath = imagePath,
                    VariantInfo = variantInfo
                });
            }

            result.Add(orderVm);
        }

        return result;
    }
    public async Task<OrderViewModel?> GetOrderByIdAsync(int orderId)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order == null) return null;

        var customer = await _unitOfWork.Customers.GetByIdAsync(order.IdCus);
        var adminBaseUrl = _configuration["AdminBaseUrl"];

        var orderDetails = await _unitOfWork.OrderDetails.GetDetailsByOrderIdAsync(orderId);
        var detailVms = new List<OrderDetailViewModel>();

        foreach (var od in orderDetails)
        {
            string productName = "";
            string imagePath = $"{adminBaseUrl}/images/no-image.png";
            string variantInfo = "-";

            var product = await _unitOfWork.Products.GetByIdAsync(od.ProductId);
            if (product != null)
            {
                productName = product.ProductName;

                var productImage = await _unitOfWork.ProductImages
                    .GetFirstOrDefaultAsync(img => img.ProductId == product.IdProduct, q => q.OrderBy(i => i.SortOrder));
                if (productImage != null && !string.IsNullOrEmpty(productImage.ImagePath))
                {
                    imagePath = $"{adminBaseUrl}/{productImage.ImagePath.TrimStart('/')}";
                }
            }

            if (od.VariantId.HasValue)
            {
                var variant = (await _unitOfWork.ProductVariants
                    .GetWithIncludeAsync(v => v.Id == od.VariantId.Value, "ProductVariantAttributes"))
                    .FirstOrDefault();

                if (variant?.ProductVariantAttributes != null)
                {
                    variantInfo = string.Join(", ", variant.ProductVariantAttributes.Select(a => $"{a.AttributeName}: {a.AttributeValue}"));
                }
            }

            detailVms.Add(new OrderDetailViewModel
            {
                IdOrderDetail = od.IdOrderDetail,
                ProductId = od.ProductId,
                VariantId = od.VariantId,
                Quantity = od.Quantity,
                Price = od.Price,
                ProductName = productName,
                ImagePath = imagePath,
                VariantInfo = variantInfo
            });
        }

        return new OrderViewModel
        {
            IdOrder = order.IdOrder,
            OrderDate = order.OrderDate,
            NgayGiao = order.NgayGiao,
            OrderStatus = order.OrderStatus,
            ShippingAddress = order.ShippingAddress,
            Phone = order.Phone,
            TotalAmount = order.TotalAmount,
            CustomerId = customer?.IdCus ?? 0,
            CustomerName = customer?.CusName ?? "(Không có tên)",
            OrderDetails = detailVms
        };
    }

    public async Task<bool> UpdateOrderStatusAsync(OrderStatusUpdateRequest request)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
        if (order == null) return false;

        order.OrderStatus = request.NewStatus;
        await _unitOfWork.Orders.UpdateAsync(order);
        return true;
    }


    public async Task<bool> UpdateStockAfterOrderAsync(List<CartItemSelectionRequest> items)
    {
        try
        {
            foreach (var item in items)
            {
                if (item.VariantId.HasValue)
                {
                    var variant = await _unitOfWork.ProductVariants.GetByIdAsync(item.VariantId.Value);
                    if (variant != null && variant.Stock >= item.Quantity)
                    {
                        variant.Stock -= item.Quantity;
                        await _unitOfWork.ProductVariants.UpdateAsync(variant);
                    }
                    else
                    {
                        // Thiếu hàng
                        return false;
                    }
                }
                else
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                    if (product != null && product.Quantity >= item.Quantity)
                    {
                        product.Quantity -= item.Quantity;
                        await _unitOfWork.Products.UpdateAsync(product);
                    }
                    else
                    {
                        // Thiếu hàng
                        return false;
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }



}
