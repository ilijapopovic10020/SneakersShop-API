using System;

namespace SneakersShop.Application.Uploads;

public enum UploadType
{
    BrandImage,
    ProductImage,
    ProfileImage
}

public interface IBase64FileUploader
{
    bool IsExtensionValid (string file);
    string GetExtention(string file);
    string Upload(string file, UploadType type, string? fileName);
}
