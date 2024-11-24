namespace TaskManagementSystem.Services
{
    public class FileUploadService
    {
        private readonly IWebHostEnvironment environment;

        public FileUploadService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public string UploadProfilePicture(IFormFile photo)
        {
            //validate if photo is null
            if (photo == null)
            {
                throw new ArgumentNullException("No file uploaded");
            }

            var size = photo.Length;
            var ext = Path.GetExtension(photo.FileName).ToLower();

            //validate file size and extension
            if (size > 2000000)
            {
                return "SizeError";
            }
            else 
            {
                if (ext.Equals(".png") || ext.Equals(".jpg") || ext.Equals(".jpeg"))
                {
                    string folder = Path.Combine(environment.WebRootPath, "Images/ProfilePictures");
                    string filename = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
                    string filePath = Path.Combine(folder, filename);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(stream);
                    }
                    return filename; //Return filename for storing in the database
                }
                else
                {
                    return "ExtensionError";
                }
            }
        }

        public string GetProfilePicturePath(string filename) 
        {
            return Path.Combine("Images/ProfilePictures", filename);
        }
    }
}
