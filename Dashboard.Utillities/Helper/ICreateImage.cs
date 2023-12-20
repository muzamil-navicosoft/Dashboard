using Microsoft.AspNetCore.Http;

namespace Dashboard.Utillities.Helper
{
    public interface ICreateImage
    {
        string ImagesCRa(string folder, IFormFile image, string serverPath);
    }
}