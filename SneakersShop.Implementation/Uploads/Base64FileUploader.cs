using SneakersShop.Application.Uploads;
using SneakersShop.Implementation.Extensions;

namespace SneakersShop.Implementation.Uploads;

public class Base64FileUploader : IBase64FileUploader
{
    private List<string> _allowedExtensions = ["jpg", "png", "jpeg"];

    private readonly Dictionary<UploadType, List<string>> _uploadPaths =
        new()
        {
            {UploadType.BrandImage, new List<string> {"wwwroot", "images", "brands"}},
            {UploadType.ProductImage, new List<string> {"wwwroot", "images", "products"}},
            {UploadType.ProfileImage, new List<string> {"wwwroot", "images", "profile"}}
        };

    public string GetExtention(string file)
    {
        return Path.GetExtension(file);
    }

    public bool IsExtensionValid(string file)
    {
        return _allowedExtensions.Contains(GetExtention(file).ToLower());
    }

    public string Upload(string file, UploadType type, string? fileName = null)
    {

        var extension = file.GetFileExtension();

        if (!_allowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException("Unspported file extension.");
        }

        var path = "";

        if (string.IsNullOrEmpty(fileName))
        {
            path = GetPath(type, null, extension);
        }
        else
        {
            path = GetPath(type, fileName, extension);
        }

        File.WriteAllBytes(path, Convert.FromBase64String(file));
        return path;
    }

    private string GetPath(UploadType type, string? name, string ext)
    {
        var path = _uploadPaths[type];

        var fileName = "";
        var _name = "";

        if (string.IsNullOrEmpty(name))
        {
            _name = Guid.NewGuid().ToString();
        }
        else
        {
            _name = name;
        }

        foreach (var pathItem in path)
        {
            fileName = Path.Combine(fileName, pathItem);
        }

        return Path.Combine(fileName, _name + "." + ext);
    }
}

