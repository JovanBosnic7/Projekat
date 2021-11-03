using Bex.Models;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BexMVC.ViewModels
{
    public class WebFileViewModel
    {
        //private static string host = "213.133.127.76";
        public static string host = "192.168.101.80";
        private static int port = 2301;
        // Enter your sftp username here
        private static string username = "miroslav";
        // Enter your sftp password here
        private static string password = "bex1ckeks1c";
        public static WebFiles getEntityModel(HttpPostedFileBase file, int fileDirType, int dirId)
        {
            WebFiles webfile = new WebFiles();

            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            byte[] data = target.ToArray();

            using (var client = new SftpClient(host, port, username, password))
            {
                client.Connect();

                string path = getFolderNameFromType(fileDirType) + "/" + dirId + "/";
                if (!client.Exists(path))
                    client.CreateDirectory(path);

                client.ChangeDirectory(path);
                if (client.IsConnected)
                {
                    target.Write(data, 0, data.Length);
                    target.Position = 0;
                    client.UploadFile(target, file.FileName, true);

                }

                client.Disconnect();
                
            }

            //webfile.Data = data;
            webfile.Data = null;
            webfile.ContentType = file.ContentType;
            webfile.FileExt = Path.GetExtension(file.FileName); 
            webfile.FileLength = file.ContentLength;
            webfile.FileName = file.FileName;
            webfile.IsActive = true;
            webfile.UpdateDate = DateTime.Now;
            

            return webfile;
        }

        public static string getFolderNameFromType(int fileDirType)
        {
            string dirPathName = "";
            switch (fileDirType)
            {
                case 2:
                    dirPathName = "/VP/";
                    break;
                case 4:
                    dirPathName = "/Sub/";
                    break;
                default:
                    dirPathName = "";
                    break;

            }
            return dirPathName;
        }
    }
}