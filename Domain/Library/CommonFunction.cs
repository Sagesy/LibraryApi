using Microsoft.AspNetCore.Http;

namespace LibraryApi.Domain.Library
{
    public class CommonFunction
    {
        public static bool checkAndDeleteAttacment(string path, string fileName)
        {
            var fullPath = Path.GetFullPath(path).Replace("~\\", "");
            fullPath += fileName;
            if (File.Exists(fullPath)) File.Delete(fullPath);
            return true;
        }

        public static bool UpdateAttachment(string path, string fileName, string contentRootPath)
        {
            path = path.Replace("~", "").Replace("/", Path.DirectorySeparatorChar.ToString());
            var fullPath = Path.GetFullPath(contentRootPath + path);

            checkAndDeleteAttacment(fullPath, fileName);

            return true;
        }

        public static string saveAttachment(IFormFile paramFile, string path
            , string contentRootPath
            , string? txtRenameFile = null)
        {
            //var fullPath = Path.GetFullPath(path).Replace("~\\", "");
            path = path.Replace("~", "").Replace("/", Path.DirectorySeparatorChar.ToString());

            var fullPath = Path.GetFullPath(contentRootPath + path);
            var fileName = string.IsNullOrEmpty(txtRenameFile) ? paramFile.FileName : txtRenameFile;

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            checkAndDeleteAttacment(fullPath, fileName);

            using (var stream = File.Create(fullPath + fileName))
            {
                paramFile.CopyTo(stream);
            }

            return fileName;
        }
    }
}