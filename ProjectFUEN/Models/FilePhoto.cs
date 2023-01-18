using System.IO;

namespace fileUpload.Models
{
    public class FilePhoto
    {
        /// <summary>
        /// 上傳是否成功,erroMessage,fileName
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public (bool, string, string) UploadFile(IFormFile file)
        {

            bool isCopied = false;
            string message = string.Empty;
            string photo = string.Empty;
            //1 check if the file length is greater than 0 bytes 
            if (file != null)
            {
                string fileName = file.FileName;
                //2 Get the extension of the file
                string extension = Path.GetExtension(fileName);
                //3 check the file extension as png
                if (extension == ".png" || extension == ".jpeg" || extension == ".jpg" || extension == ".gif" || extension == "webp" || extension == "svg" || extension == "tiff" || extension == "icon")
                {
                    string path = Directory.GetCurrentDirectory();
                    string newFileName = GetNewFileName(path, fileName);
                    //4 set the path where file will be copied
                    string filePath = Path.GetFullPath(
                            Path.Combine(Directory.GetCurrentDirectory(),
                                                        "wwwroot/UploadedFiles"));

                    //5 copy the file to the path
                    using (var fileStream = new FileStream(Path.Combine(filePath, newFileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        isCopied = true;
                    }
                    photo = newFileName;
                }
                else
                {
                    message = "檔案必須是圖片檔案";
                }

            }
            else
            {
                message = "記得選取檔案";
            }

            return (isCopied, message, photo);

        }

        private string GetNewFileName(string path, string fileName)
        {
            string ext = Path.GetExtension(fileName);  //得到副檔名(包含.)
            string newFileName = string.Empty;
            string fullPath = string.Empty;

            do
            {
                newFileName = Guid.NewGuid().ToString("N").Substring(0, 25) + ext;  //guid會有dash-，ToString("N")去掉dash
                fullPath = System.IO.Path.Combine(path, newFileName);
            } while (System.IO.File.Exists(fullPath));
            return newFileName;
        }
    }


}

