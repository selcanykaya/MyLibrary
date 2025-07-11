namespace Kütüphanem.Models.Author
{
    public class AuthorAddViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string Description { get; set; } 
        public IFormFile ImageFile { get; set; }

    }
}
