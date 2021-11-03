namespace Bex.Models
{
    using System;
    using System.Collections.Generic;


    public  partial class Street
    {

        public int? IdStari { get; set; }

        public int Id { get; set; }

        public string StreetName { get; set; }

        public int PlaceId { get; set; }
        public string MestoNaziv { get; set; }

        public int? UserUnosId { get; set; }

        public int? UserIzmeneId { get; set; }

        public string Comment { get; set; }

        public DateTime? DI { get; set; }

        public virtual Place Place { get; set; }

        public virtual ICollection<PAK> PAKs { get; set; }


    }
}
