using Backend.DTOs.Response;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly FileService _fileService;

        public UploadController(FileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Upload ảnh nhà hàng
        /// POST /api/Upload/restaurant
        /// </summary>
        [Authorize(Roles = "admin,restaurant")]
        [HttpPost("restaurant")]
        public async Task<ActionResult<ApiResponse<string>>> UploadRestaurantImage(IFormFile file)
        {
            try
            {
                var url = await _fileService.UploadImageAsync(file, "restaurants");
                return Ok(new ApiResponse<string>
                {
                    Code = 200,
                    Message = "Upload thành công",
                    Results = url
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Code = 1003,
                    Message = ex.Message,
                    Results = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Code = 9999,
                    Message = $"Lỗi upload: {ex.Message}",
                    Results = null
                });
            }
        }

        /// <summary>
        /// Upload ảnh món ăn
        /// POST /api/Upload/food
        /// </summary>
        [Authorize(Roles = "admin,restaurant")]
        [HttpPost("food")]
        public async Task<ActionResult<ApiResponse<string>>> UploadFoodImage(IFormFile file)
        {
            try
            {
                var url = await _fileService.UploadImageAsync(file, "foods");
                return Ok(new ApiResponse<string>
                {
                    Code = 200,
                    Message = "Upload thành công",
                    Results = url
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Code = 1003,
                    Message = ex.Message,
                    Results = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Code = 9999,
                    Message = $"Lỗi upload: {ex.Message}",
                    Results = null
                });
            }
        }

        /// <summary>
        /// Upload ảnh chung
        /// POST /api/Upload/image?folder=avatars
        /// </summary>
        [HttpPost("image")]
        public async Task<ActionResult<ApiResponse<string>>> UploadImage(IFormFile file, [FromQuery] string folder = "general")
        {
            try
            {
                var allowedFolders = new[] { "general", "avatars", "banners", "restaurants", "foods" };
                if (!allowedFolders.Contains(folder.ToLower()))
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        Code = 1003,
                        Message = "Folder không hợp lệ",
                        Results = null
                    });
                }

                var url = await _fileService.UploadImageAsync(file, folder.ToLower());
                return Ok(new ApiResponse<string>
                {
                    Code = 200,
                    Message = "Upload thành công",
                    Results = url
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Code = 1003,
                    Message = ex.Message,
                    Results = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Code = 9999,
                    Message = $"Lỗi upload: {ex.Message}",
                    Results = null
                });
            }
        }

        /// <summary>
        /// Xóa ảnh
        /// DELETE /api/Upload?imageUrl=https://res.cloudinary.com/...
        /// </summary>
        [HttpDelete]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteImage([FromQuery] string imageUrl)
        {
            try
            {
                var result = await _fileService.DeleteImageAsync(imageUrl);
                return Ok(new ApiResponse<bool>
                {
                    Code = 200,
                    Message = result ? "Xóa ảnh thành công" : "Không tìm thấy ảnh",
                    Results = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    Code = 9999,
                    Message = $"Lỗi xóa ảnh: {ex.Message}",
                    Results = false
                });
            }
        }
    }
}
