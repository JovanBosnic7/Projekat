using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(TicketStatusMetadata))]
    public partial class TicketStatus
    {
        public class TicketStatusMetadata
        {

            public int Id { get; set; }           
            public string Naziv { get; set; }
            
            private TicketStatusMetadata() { }
        }
    }
}