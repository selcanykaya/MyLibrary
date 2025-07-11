using Kütüphanem.Models.Book;

namespace Kütüphanem.Models.Author
{
    public class AuthorDetailViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
        public DateTime DateOfBirth { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        //Yazara ait kitaplar
        public List<BookSummaryViewModel> Books { get; set; } = new List<BookSummaryViewModel>();
    }
}
