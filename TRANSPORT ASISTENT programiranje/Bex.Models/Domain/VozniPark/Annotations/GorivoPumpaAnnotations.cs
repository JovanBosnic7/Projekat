using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(GorivoPumpaMetadata))]
    public partial class GorivoPumpa
    {
        public class GorivoPumpaMetadata
        {

            public int Id { get; set; }
            public string NazivPumpe { get; set; }

            private GorivoPumpaMetadata() { }
        }
    }
}
