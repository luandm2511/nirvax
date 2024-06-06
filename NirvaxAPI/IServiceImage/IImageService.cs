namespace WebAPI.IServiceImage
{
    public interface IImageService
    {
        public string SaveImage(IFormFile imageFile, string path);
        public void DeleteImage(string imagePath);
    }
}
