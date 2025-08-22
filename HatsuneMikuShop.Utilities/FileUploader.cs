using Microsoft.AspNetCore.Http;

namespace LifetimeLiveHouse.Utilities
{
    public class FileUploader
    {
        private readonly string _baseFolder;

        public FileUploader(string baseFolder = null)
        {
            _baseFolder = baseFolder ?? Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                "Assets", "Images"
            );

            Directory.CreateDirectory(_baseFolder);
        }

        public string SaveFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("上傳檔案無效");

            string fileName = Path.GetFileName(file.FileName);
            string filePath = Path.Combine(_baseFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return "檔案上傳成功!!";
        }
    }
}
