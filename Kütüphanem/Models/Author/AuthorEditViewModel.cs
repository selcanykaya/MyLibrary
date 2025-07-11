namespace Kütüphanem.Models.Author
{
    public class AuthorEditViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; } // Mevcut Görsel URL'si

        public IFormFile ImageFile { get; set; } // Yeni Görsel dosyası
    }
}
