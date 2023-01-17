using System.IO;

namespace fileUpload.Models
{
	public class FileManager2
	{
		/// <summary>
		/// 上傳是否成功,erroMessage,fileName
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public (bool isCopied, string message, string Source) UploadFile(IFormFile file)
		{

			bool isCopied = false;
			string message = string.Empty;
			string Source = string.Empty;
			//1 check if the file length is greater than 0 bytes 
			if (file!=null)
			{
				string fileName = file.FileName;
				//2 Get the extension of the file
				string extension = Path.GetExtension(fileName);
                //3 check the file extension as png
                if (extension == ".PNG" || extension == ".jepg" || extension == ".jpg" || extension == ".gif" || extension == "webp" || extension == "svg" || extension == "tiff" || extension == "icon")
                {
                    string path = Directory.GetCurrentDirectory();
					//string newFileName = GetNewFileName(path, fileName);/
					//4 set the path where file will be copied
					string filePath = Path.GetFullPath(
							Path.Combine(Directory.GetCurrentDirectory(),
                                                        "wwwroot/ProductImgFiles"));

					//5 copy the file to the path
					using (var fileStream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
					{
						file.CopyTo(fileStream);
						isCopied = true;
					}
                    Source = fileName;
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
			return (isCopied, message, Source);
		}
	}
}

