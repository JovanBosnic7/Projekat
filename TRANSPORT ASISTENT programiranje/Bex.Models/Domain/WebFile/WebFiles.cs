using System;

namespace Bex.Models
{
   

    public  partial class WebFiles
    {
       
        public int Id { get; set; }
        public byte[] Data { get; set; }
        public bool IsActive { get; set; }

        public DateTime UpdateDate  { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }

        public int FileLength { get; set; }
        public string ContentType { get; set; }

        public int StraniId { get; set; }
        public int TypeId { get; set; }
        public int UserUneoId { get; set; }

        public virtual WebFilesTip WebFilesTip { get; set; }

    }
}
