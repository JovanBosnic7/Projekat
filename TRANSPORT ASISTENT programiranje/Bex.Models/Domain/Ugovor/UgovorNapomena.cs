namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class UgovorNapomena

    {
       
        public int Id { get; set; }
        public int? UgovorId { get; set; }
        public int UgovorNapomenaTipId { get; set; }
        public string Napomena { get; set; }
        public DateTime DatumUnosa { get; set; }

        public virtual Ugovor Ugovor { get; set; }
        public virtual UgovorNapomenaTip UgovorNapomenaTip { get; set; }

    }
}
