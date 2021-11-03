namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class UgovorVip
    {
        public int Id { get; set; }
        public int? UgovorId { get; set; }
        public bool VipD { get; set; }
        public string VipDvremeDo { get; set; }
        public DateTime VipDdatumDo { get; set; }
        public bool VipT { get; set; }
        public bool VipN { get; set; }       
        public DateTime DatumUnosa { get; set; }

        public virtual Ugovor Ugovor { get; set; }
    }
}
