using Microsoft.AspNetCore;

namespace Dashboard.Helper
{
    public class CreateImage : ICreateImage
    {
        public string ImagesCRa(string folder, IFormFile image, string serverPath)
        {
            string Folder = $"Images/{folder}/";
            string ImagesName = Guid.NewGuid().ToString() + "_" + image.FileName;
            Folder += ImagesName;
            string server = Path.Combine(serverPath, Folder);

            image.CopyTo(new FileStream(server, FileMode.Create));

            return ImagesName;
        }
    }
}
