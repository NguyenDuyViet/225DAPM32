using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ApiResponse<List<UserResponse>>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();

                return Ok( new ApiResponse<List<UserResponse>>
                {
                    Code = 200,
                    Message = "Successfully",
                    Results = users
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserResponse>>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound(new ApiResponse<UserResponse>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy tài khoản.",
                        Results = default!
                    });

                return Ok(new ApiResponse<UserResponse>
                {
                    Code = 1000,
                    Message = "Lấy thông tin tài khoản thành công.",
                    Results = user
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by id: {UserId}", id);
                return StatusCode(500, new ApiResponse<UserResponse>
                {
                    Code = 9999,
                    Message = "Không thể tải thông tin tài khoản.",
                    Results = default!
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserResponse>>> CreateUser([FromBody] CreateUserDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdUser = await _userService.CreateUserAsync(dto);

                if(createdUser == null)
                    return BadRequest(new ApiResponse<bool>
                    {
                        Code = 400,
                        Message = "Username or Email already exists!",
                        Results = false
                    });
                return CreatedAtAction(
                    nameof(GetUserById),
                    new { id = createdUser.IdUser },
                    new ApiResponse<UserResponse>
                    {
                        Code = 201,
                        Message = "Create user successfully",
                        Results = createdUser
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin,customer")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUserId = User.FindFirstValue("UserId");
                var isAdmin = User.IsInRole("admin");
                if (!isAdmin && (!int.TryParse(currentUserId, out var authenticatedId) || authenticatedId != id))
                {
                    return Forbid();
                }

                var updatedUser = await _userService.UpdateUserAsync(id, dto);
                if (updatedUser == null)
                    return NotFound(new ApiResponse<UserResponse>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy tài khoản.",
                        Results = default!
                    });

                return Ok(new ApiResponse<UserResponse>
                {
                    Code = 1000,
                    Message = "Cập nhật thông tin thành công.",
                    Results = updatedUser
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<UserResponse>
                {
                    Code = 1003,
                    Message = ex.Message,
                    Results = default!
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {UserId}", id);
                return StatusCode(500, new ApiResponse<UserResponse>
                {
                    Code = 9999,
                    Message = "Không thể cập nhật thông tin. Vui lòng kiểm tra dữ liệu và thử lại.",
                    Results = default!
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (!result)
                    return NotFound(new { message = "User not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: {UserId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("{id}/toggle-status")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> ToggleStatus(int id)
        {
            try
            {
                var user = await _userService.ToggleStatusAsync(id);
                if (user == null)
                {
                    return NotFound(new ApiResponse<UserResponse>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy tài khoản.",
                        Results = default!
                    });
                }

                var message = string.Equals(user.Status, "active", StringComparison.OrdinalIgnoreCase)
                    ? "Đã mở khóa tài khoản."
                    : "Đã khóa tài khoản.";

                return Ok(new ApiResponse<UserResponse>
                {
                    Code = 1000,
                    Message = message,
                    Results = user
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling status for user: {UserId}", id);
                return StatusCode(500, new ApiResponse<UserResponse>
                {
                    Code = 9999,
                    Message = "Không thể cập nhật trạng thái tài khoản.",
                    Results = default!
                });
            }
        }
    }
}
