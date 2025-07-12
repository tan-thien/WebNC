using Microsoft.AspNetCore.Mvc;
using WebShopSolution.Application.Catalog.Vouchers;
using WebShopSolution.ViewModels.Catalog.Voucher;

namespace WebShopSolution.API.Controllers
{
    [Route("api/voucher")]
    [ApiController]
    public class VoucherApiController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherApiController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _voucherService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _voucherService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Không tìm thấy voucher." });

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] VoucherCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _voucherService.CreateAsync(request);
            if (!success)
                return BadRequest(new { message = "Tạo voucher thất bại." });

            return Ok(new { message = "Tạo voucher thành công." });
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] VoucherUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _voucherService.UpdateAsync(request);
            if (!success)
                return NotFound(new { message = "Voucher không tồn tại hoặc cập nhật thất bại." });

            return Ok(new { message = "Cập nhật voucher thành công." });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var success = await _voucherService.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = "Voucher không tồn tại hoặc đã bị xoá." });

            return Ok(new { message = "Xoá voucher thành công." });
        }

    }
}
