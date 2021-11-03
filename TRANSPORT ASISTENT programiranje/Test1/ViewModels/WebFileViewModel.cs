using Bex.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DDtrafic.ViewModels
{
    public class WebFileViewModel
    {        
        public static WebFiles getEntityModel(HttpPostedFileBase file, int fileDirType, int dirId)
        {
            WebFiles webfile = new WebFiles();

            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            byte[] data = target.ToArray();


            webfile.Data = data;            
            webfile.ContentType = file.ContentType;
            webfile.FileExt = Path.GetExtension(file.FileName); 
            webfile.FileLength = file.ContentLength;
            webfile.FileName = file.FileName;
            webfile.IsActive = true;
            webfile.UpdateDate = DateTime.Now;
            

            return webfile;
        }

        
    }
}