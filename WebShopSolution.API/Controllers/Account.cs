using Microsoft.AspNetCore.Mvc;
using WebShopSolution.Application.Catalog.Accounts;
using WebShopSolution.ViewModels.Catalog.Account;

namespace WebShopSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountCreateRequest request)
        {
            var idAcc = await _accountService.RegisterAsync(request);
            if (idAcc == null)
                return BadRequest("Tài khoản đã tồn tại.");

            return Ok("Đăng ký thành công.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginRequest request)
        {
            var account = await _accountService.LoginAsync(request);
            if (account == null)
                return Unauthorized("Tên đăng nhập hoặc mật khẩu không đúng.");

            return Ok(account);
        }

        [HttpGet("admin/list")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _accountService.GetAllAsync();
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] AccountUpdateRequest request)
        {
            var result = await _accountService.UpdateAsync(request);
            if (!result)
                return NotFound("Không tìm thấy tài khoản cần cập nhật.");

            return Ok("Cập nhật thành công.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _accountService.DeleteAsync(id);
            if (!result)
                return NotFound("Không tìm thấy tài khoản cần xoá.");

            return Ok("Xoá tài khoản thành công.");
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await _accountService.ChangePasswordAsync(request);
            if (!result)
                return BadRequest("Đổi mật khẩu thất bại. Mật khẩu hiện tại không đúng.");

            return Ok("Đổi mật khẩu thành công.");
        }
    }
}
