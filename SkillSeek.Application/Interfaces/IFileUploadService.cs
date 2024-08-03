using Microsoft.AspNetCore.Http;

namespace SkillSeek.Application.Interfaces;

public interface IFileUploadService
{
    string UploadFile(IFormFile file, string folderPath);
}