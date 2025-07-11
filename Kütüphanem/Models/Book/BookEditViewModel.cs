namespace Kütüphanem.Models.Book
{
    public class BookEditViewModel
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public string Genre { get; set; }
        public DateTime PublishDate { get; set; }
        public string ISBN { get; set; }
        public int CopiesAvailable { get; set; }

        public IFormFile ImageFile { get; set; }
        public string ImageUrl { get; set; }      // Mevcut görseli göstermek için
        
    }
}
