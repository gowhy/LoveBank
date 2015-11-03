using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoveBank.Core;
using LoveBank.Core.Domain;
using LoveBank.Common;
using System.IO;
using LoveBank.Core.MSData;

namespace LoveBank.Services
{
    public class UploadFileInstance
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static SourceFile SaveFile(System.Web.HttpPostedFileBase file, string dir, object obj)
        {
            SourceFile img = new SourceFile();

         
            string FtpServerHttpUrl = System.Configuration.ConfigurationManager.AppSettings["FtpServerHttpUrl"];
            string FtpServer = System.Configuration.ConfigurationManager.AppSettings["FtpServer"];
            string FtpUser = System.Configuration.ConfigurationManager.AppSettings["FtpUser"];
            string FtpPassWord = System.Configuration.ConfigurationManager.AppSettings["FtpPassWord"];

            string Dir = DateTime.Now.ToString("yyyyMMdd");
            FTPHelper ftp = new FTPHelper(FtpServer, dir + "/" + Dir, FtpUser, FtpPassWord);

            FileInfo file2 = new FileInfo(file.FileName);

          
            //string fileName = Guid.NewGuid() + file2.Extension;//文件的真实存储名是GUID
            IdWorker workId = new IdWorker();
            string fileName = workId.nextId() + file2.Extension;//新的自定义能全局的Id,
            ftp.Upload(file, fileName);
           
            img.Domain = FtpServerHttpUrl;
            img.Server = ftp.FtpServer;
            img.Path = ftp.FtpRemotePath + "/" + fileName;
            img.FileName = file.FileName;//上传时的文件名
            //img.Guid = fileName;
            img.Extension = file2.Extension;
            //img.AddTime = DateTime.Now;
            img.AddUserId = int.Parse(obj.ToString());

            return img;
        }

        public static SocSerImgEntity SaveFileOld(System.Web.HttpPostedFileBase file, string dir, string fileName)
        {

            string FtpServerHttpUrl = System.Configuration.ConfigurationManager.AppSettings["FtpServerHttpUrl"];
            string FtpServer = System.Configuration.ConfigurationManager.AppSettings["FtpServer"];
            string FtpUser = System.Configuration.ConfigurationManager.AppSettings["FtpUser"];
            string FtpPassWord = System.Configuration.ConfigurationManager.AppSettings["FtpPassWord"];

            string Dir = DateTime.Now.ToString("yyyyMMdd");
            FTPHelper ftp = new FTPHelper(FtpServer, dir + "/" + Dir, FtpUser, FtpPassWord);


            if (string.IsNullOrEmpty(fileName))
            {
                FileInfo file2 = new FileInfo(file.FileName);
                fileName = Guid.NewGuid() + file2.Extension;
            }


            ftp.Upload(file, fileName);

            SocSerImgEntity img = new SocSerImgEntity();
            img.FTPUrl = ftp.FtpURI;
            img.HttpUrl = FtpServerHttpUrl + ftp.FtpRemotePath + "/" + fileName;
            img.Name = file.FileName;
            img.Module = "保存服务";
            img.AddTime = DateTime.Now;
            return img;
        }

    }
}
