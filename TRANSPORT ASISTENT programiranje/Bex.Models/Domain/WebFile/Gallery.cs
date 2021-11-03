namespace Bex.Models
{
   

    public  partial class Gallery
    {
       
        public int Id { get; set; }
        public string Title { get; set; }
        public int WebImageId { get; set; }

        public bool IsActive { get; set; }

        public bool IsProfile { get; set; }
        public int? OrderNo { get; set; }

    }
}
