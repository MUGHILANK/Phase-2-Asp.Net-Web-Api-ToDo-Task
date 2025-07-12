using todoTask.Data;
using todoTask.Models.Domain;

namespace todoTask.Repositories
{
    public class LocalImageRepository : IimageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MKTodotaskDbContext _dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,MKTodotaskDbContext dbContext) 
        {
            this._webHostEnvironment = webHostEnvironment;
            this._httpContextAccessor = httpContextAccessor;
            this._dbContext = dbContext;
        }
        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, // To get Root Path to the Folder 
                "Images", // Folder Name
                $"{image.FileName}{image.FileExtension}");
                //image.FileName, // "SuperMan" image Name 
                //image.FileExtension // ".png" File Extension
                //);


            // Upload Image to the local Path
            var stream = new FileStream(localFilePath,FileMode.Create);
            await image.File.CopyToAsync(stream);

            // Path to save the file
            // https://loaclhost:1234/Images/SuperMan.png
            // {_httpContextAccessor.HttpContext.Request.Scheme} to see it is Https or Http Scheme "Https or Http"
            // {_httpContextAccessor.HttpContext.Request.Host} to Define Host "Local Host"
            //{_httpContextAccessor.HttpContext.Request.PathBase} to Path base "Port ID: 1234"

            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.Filepath = urlFilePath;

            // Add image to the Images Table
            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();

            return image;


            
        }
    }
}
