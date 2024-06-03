using WebAPI.IService;

namespace WebAPI.Service
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public string SaveImage(IFormFile imageFile, string path)
        {
            // Chỉ chấp nhận các tệp hình ảnh
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(imageFile.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Invalid file type. Only JPG, JPEG, PNG, and GIF files are allowed.");
            }

            var folderName = Path.Combine("wwwroot", "images", path);
            var pathToSave = Path.Combine(_env.ContentRootPath, folderName);

            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            var fileName = Guid.NewGuid().ToString() + extension;
            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine("images", path, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            return dbPath.Replace("\\", "/"); // Convert to URL format
        }

        public void DeleteImage(string imagePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, imagePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
