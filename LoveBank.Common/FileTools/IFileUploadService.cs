using System.Web;

namespace LoveBank.Common
{
    public interface IFileUploadService {
        string UploadFile(HttpPostedFileBase postedFile, string savePath, bool isReplace);
        string UploadFile(HttpPostedFileBase postedFile, string savePath);
        void FileDownload(HttpResponseBase response,string filePah);
        bool IsExists(string fullName);
        bool DeleteFile(string fullName);

    }
}