using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Backend.Services
{
    public class FileService
    {
        private readonly Cloudinary _cloudinary;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public FileService(IConfiguration configuration)
        {
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        /// <summary>
        /// Upload ảnh lên Cloudinary
        /// </summary>
        /// <param name="file">File ảnh</param>
        /// <param name="folder">Thư mục trên Cloudinary (restaurants, foods, categories...)</param>
        /// <returns>URL ảnh trên Cloudinary</returns>
        public async Task<string> UploadImageAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File không hợp lệ");

            if (file.Length > MaxFileSize)
                throw new ArgumentException("File vượt quá 5MB");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                throw new ArgumentException("Chỉ chấp nhận file ảnh: jpg, jpeg, png, gif, webp");

            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = $"DAPM_32/{folder}",
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                throw new Exception($"Upload thất bại: {uploadResult.Error.Message}");

            return uploadResult.SecureUrl.ToString();
        }

        /// <summary>
        /// Xóa ảnh trên Cloudinary theo URL
        /// </summary>
        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return false;

            try
            {
                // Lấy public_id từ URL
                var publicId = GetPublicIdFromUrl(imageUrl);
                if (string.IsNullOrEmpty(publicId))
                    return false;

                var deleteParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);

                return result.Result == "ok";
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Trích xuất public_id từ Cloudinary URL
        /// VD: https://res.cloudinary.com/xxx/image/upload/v123/225dapm32/restaurants/abc.jpg
        /// → 225dapm32/restaurants/abc
        /// </summary>
        private string? GetPublicIdFromUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                var path = uri.AbsolutePath; // /image/upload/v123/225dapm32/restaurants/abc.jpg

                // Tìm phần sau "upload/vXXX/"
                var uploadIndex = path.IndexOf("/upload/");
                if (uploadIndex == -1) return null;

                var afterUpload = path.Substring(uploadIndex + 8); // v123/225dapm32/restaurants/abc.jpg

                // Bỏ version (v123/)
                var slashIndex = afterUpload.IndexOf('/');
                if (slashIndex == -1) return null;

                var publicIdWithExt = afterUpload.Substring(slashIndex + 1); // 225dapm32/restaurants/abc.jpg

                // Bỏ extension
                var lastDot = publicIdWithExt.LastIndexOf('.');
                if (lastDot > 0)
                    return publicIdWithExt.Substring(0, lastDot); // 225dapm32/restaurants/abc

                return publicIdWithExt;
            }
            catch
            {
                return null;
            }
        }
    }
}
