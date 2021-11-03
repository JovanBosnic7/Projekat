namespace Bex.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Cenovnik

    {

        public int? IdCenovnika { get; set; }
        public int? BrojCenovnika { get; set; }
        public DateTime? DatumCenovnika { get; set; }
        public string OpisCenovnika { get; set; }
        public bool Storno { get; set; }
        //public string KorisnikCenovnika { get; set; }

        //public virtual Ugovor Ugovor { get; set; }
        //public virtual ICollection<Ugovor> Ugovor { get; set; }
        //public virtual ICollection<Cene> Cene { get; set; }
        //public virtual ICollection<UgovorNapomena> UgovorNapomena { get; set; }
        //public virtual ICollection<UgovorObracunCena> UgovorObracunCena { get; set; }
        //public virtual ICollection<UgovorVip> UgovorVip { get; set; }
        public virtual ICollection<Cene> Cene { get; set; }
    }
}
