using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(ZaposleniStrucnaSpremaMetadata))]
    public partial class ZaposleniStrucnaSprema
    {
        public class ZaposleniStrucnaSpremaMetadata
        {
           
            public int Id { get; set; }

            public int Sifra { get; set; }

            public string Opis { get; set; }

            private ZaposleniStrucnaSpremaMetadata() { }
        }
    }
}