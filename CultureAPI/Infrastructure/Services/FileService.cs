using CultureAPI.Domain.ServiceAbstractions;

namespace CultureAPI.Infrastructure.Services
{
    public class FileService: IFileService
    {
        private readonly IWebHostEnvironment _environment;

        private readonly IConfiguration _configuration;

        public FileService(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }

        private string CreateFilePath(string title, string id)
        {
            string staticFilesPath = _environment.WebRootPath;
            string formattedString = title.Replace(" ", "-");
            string path = "news/" + $"{id}" + '_' + formattedString;
            return Path.Combine(staticFilesPath, path + ".png");
        }
        public async Task UploadFile (IFormFile file, string title, string id)
        {
            using (var fileStream = new FileStream(CreateFilePath(title, id), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        public string CreateUrl(string title, string id)
        {
            string staticFilesPath = _configuration.GetSection("AppURLs:BaseUrl").Value;
            string formattedString = title.Replace(" ", "-");
            string path = "news/" + $"{id}" + '_' + formattedString + ".png";
            return staticFilesPath + path;
        }
    }
}
