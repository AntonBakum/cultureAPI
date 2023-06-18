namespace CultureAPI.Domain.ServiceAbstractions
{
    public interface IFileService
    {
        string CreateUrl(string title, string id);
        //string CreateFilePath(string title, string id);
        Task UploadFile(IFormFile file, string title, string id);
    }
}
