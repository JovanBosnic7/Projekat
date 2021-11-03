using Bex.Common;
using Bex.DAL.EF.UOW;
using DDtrafic.MVC.Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace DDtrafic.Controllers
{
    public class DownloadFileController : Controller
    {
        public DownloadFileController() : this(new BexUow(), new ExceptionSolver())
        { }
        public DownloadFileController(
            IBexUow bexUow,
            IExceptionSolver exceptionSolver)
        {
            BexUow = bexUow;
            ExceptionSolver = exceptionSolver;
        }

        [OutputCache(Duration = 60, Location = OutputCacheLocation.Any, VaryByParam = "Id;t")]
        public ActionResult GetImage(int Id = 0, int t = 0)
        {
            if (Id == 0)
            {
                return File("~/images/NoImage.png", "image/png");
            }
           

            var model = BexUow.WebFiles.Find(Id);
            //var dirName = BexUow.WebFilesTip.Find(model.TypeId);
             
            if (model != null)
            {
                if (t != 0)
                {
                    byte[] img = getThumbNail(model.Data, t);
                    return File(img, model.ContentType, "thumb_" + model.FileName);
                }

                    
                return File(model.Data, model.ContentType, model.FileName);
                
                    //return File("http://"+ WebFileViewModel.host + "/"+ dirName.DirName + "/"+model.StraniId+"/" + model.FileName, model.ContentType);
            }
            else
            {
                return HttpNotFound();
            }
        }

        private byte[] getThumbNail(byte[] data, int multi = 1)
        {
            using (var file = new MemoryStream(data))
            {
                int width = 200 * multi; 
                using (var image = Image.FromStream(file, true, true)) /* Creates Image from specified data stream */
                {
                    int X = image.Width;
                    int Y = image.Height;
                    int height = (int)((width * Y) / X);

                    using (var thumb = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero))
                    {
                        var jpgInfo = ImageCodecInfo.GetImageEncoders().Where(codecInfo => codecInfo.MimeType == "image/png").First(); /* Returns array of image encoder objects built into GDI+ */
                        using (var encParams = new EncoderParameters(1))
                        {
                            using (var samllfile = new MemoryStream())
                            {
                                long quality = 100;
                                encParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                                thumb.Save(samllfile, jpgInfo, encParams);
                                return samllfile.ToArray();
                            }
                        };
                    };
                };
            };
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            { BexUow.Dispose(); }
            base.Dispose(disposing);
        }

        private IExceptionSolver ExceptionSolver { get; }
        private IBexUow BexUow { get; }
    } 
}