using Microsoft.AspNetCore.Components.Forms;

namespace FinalAssignemnt_APDP.Services
{
    public class FileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileUploadService> _logger;
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB
        private readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

        public FileUploadService(IWebHostEnvironment environment, ILogger<FileUploadService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task<(bool Success, string? FilePath, string? ErrorMessage)> UploadAvatarAsync(IBrowserFile file, string userId)
        {
            try
            {
                // Validate file size
                if (file.Size > MaxFileSize)
                {
                    return (false, null, $"File size exceeds the maximum allowed size of {MaxFileSize / 1024 / 1024}MB");
                }

                // Validate file extension
                var extension = Path.GetExtension(file.Name).ToLowerInvariant();
                if (!AllowedExtensions.Contains(extension))
                {
                    return (false, null, $"File type not allowed. Allowed types: {string.Join(", ", AllowedExtensions)}");
                }

                // Create upload directory if it doesn't exist
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "avatars");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique filename
                var fileName = $"{userId}_{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save the file
                await using FileStream fs = new(filePath, FileMode.Create);
                await file.OpenReadStream(MaxFileSize).CopyToAsync(fs);

                // Return relative path for storing in database
                var relativePath = $"/uploads/avatars/{fileName}";
                return (true, relativePath, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file");
                return (false, null, "An error occurred while uploading the file");
            }
        }

        public bool DeleteAvatar(string? avatarPath)
        {
            try
            {
                if (string.IsNullOrEmpty(avatarPath))
                    return false;

                // Convert relative path to physical path
                var physicalPath = Path.Combine(_environment.WebRootPath, avatarPath.TrimStart('/'));
                
                if (File.Exists(physicalPath))
                {
                    File.Delete(physicalPath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file: {FilePath}", avatarPath);
                return false;
            }
        }

        // Generic delete helper for files stored under wwwroot (e.g. /uploads/grades/xxx)
        public bool DeleteFile(string? relativePath)
        {
            try
            {
                if (string.IsNullOrEmpty(relativePath))
                    return false;

                var physicalPath = Path.Combine(_environment.WebRootPath, relativePath.TrimStart('/'));

                if (File.Exists(physicalPath))
                {
                    File.Delete(physicalPath);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file: {FilePath}", relativePath);
                return false;
            }
        }
    }
}