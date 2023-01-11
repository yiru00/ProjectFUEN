using System.IO;

namespace fileUpload.Models
{
	public class FileManager
	{
		/// <summary>
		/// 上傳是否成功,erroMessage,fileName
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public (bool, string,string) UploadFile(IFormFile file)
		{

			bool isCopied = false;
			string message = string.Empty;
			string sourcephoto = string.Empty;
			//1 check if the file length is greater than 0 bytes 
			if (file!=null)
			{
				string fileName = file.FileName;
				//2 Get the extension of the file
				string extension = Path.GetExtension(fileName);
				//3 check the file extension as png
				if (extension == ".png" || extension == ".jpg")
				{
					string path = Directory.GetCurrentDirectory();
					//string newFileName = GetNewFileName(path, fileName);/
					//4 set the path where file will be copied
					string filePath = Path.GetFullPath(
							Path.Combine(Directory.GetCurrentDirectory(),
														"ProductImgFiles"));
					//5 copy the file to the path
					using (var fileStream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
					{
						file.CopyTo(fileStream);
						isCopied = true;
					}
                    sourcephoto = fileName;
				}
				else
				{
					message = "檔案必須是.png 或 .jpg";
				}

			}
			else
			{
				message = "記得選取檔案";
			}

			return (isCopied, message, sourcephoto);

		}

		private string GetNewFileName(string path, string fileName)
		{
			string ext = Path.GetExtension(fileName);  //得到副檔名(包含.)
			string newFileName = string.Empty;
			string fullPath = string.Empty;

			do
			{
				fullPath = System.IO.Path.Combine(path, newFileName);
			} while (System.IO.File.Exists(fullPath));
			return newFileName;
		}
	}


}

