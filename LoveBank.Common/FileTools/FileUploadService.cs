using System;
using System.IO;
using System.Web;

namespace LoveBank.Common
{
    public class FileUploadService : IFileUploadService {
        public string UploadFile(HttpPostedFileBase postedFile, string savePath, bool isReplace) {
            if (postedFile == null) {
                throw new Exception("文件为空");
            }
            if (!Directory.Exists(savePath)) {
                Directory.CreateDirectory(savePath);
            }
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            var name = postedFile.FileName;
            var stuffix = Path.GetExtension(name);
            fileName = fileName + stuffix;
            var fullName = Path.Combine(savePath, fileName);
            postedFile.SaveAs(fullName);
            return fullName;
        }

        public string UploadFile(HttpPostedFileBase postedFile, string savePath) {
            return UploadFile(postedFile, savePath, true);
        }

        public void FileDownload(HttpResponseBase response,string filePah) {
            if(!IsExists(filePah)) {
                throw new Exception("文件不存在");
            }
            var fileInfo = new FileInfo(filePah);
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileInfo.Name,System.Text.Encoding.UTF8));
            response.AddHeader("Content-Length", fileInfo.Length.ToString());
            //response.AddHeader("Content-Transfer-Encoding", "binary");
            response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
            response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            response.WriteFile(fileInfo.FullName);
            response.Flush();
            response.End();
        }

        public bool IsExists(string fullName) {
            return File.Exists(fullName);
        }

        public bool DeleteFile(string fullName) {
            if (IsExists(fullName)) {
                File.Delete(fullName);
                return true;
            }
            return false;

        }
    }
}