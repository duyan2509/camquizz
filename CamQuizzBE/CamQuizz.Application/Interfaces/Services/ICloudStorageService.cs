namespace CamQuizz.Application.Interfaces;

public interface ICloudStorageService
{
    Task<string?> UploadAsync(Stream fileStream, string fileName);
    Task DeleteAsync(string publicId);
}