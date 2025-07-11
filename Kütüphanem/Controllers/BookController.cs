using Microsoft.AspNetCore.Mvc;
using Kütüphanem.Models;
using Kütüphanem.Models.Book;

namespace Kütüphanem.Controllers
{
    public class BookController : Controller
    {
      public static List<Book> books = new List<Book>
      {
        new Book { Id = 1, Title = "Yüzüklerin Efendisi Yüzük Kardeşliği", AuthorId = 2, Genre = "Fantastik", PublishDate = new DateTime(1949, 6, 8), ISBN = "1234567890", CopiesAvailable = 15 , ImageUrl ="bookImages/1.jpg" },
        new Book { Id = 2, Title = "Dalgalar", AuthorId = 4, Genre = "Fiction", PublishDate = new DateTime(1960, 7, 11), ISBN = "0987654321", CopiesAvailable = 3 , ImageUrl="bookImages/2.jpg"},
        new Book { Id = 3, Title = "İyi Geceler Punpun 1", AuthorId = 1, Genre="Manga", CopiesAvailable = 100, ISBN = "0001", PublishDate = new DateTime(2022,03,01), ImageUrl="bookImages/3.jpg" },
        new Book { Id = 4, Title = "İyi Geceler Punpun 2", AuthorId = 1, Genre="Manga", CopiesAvailable = 100, ISBN = "0002", PublishDate = new DateTime(2023,03,01), ImageUrl = "bookImages/4.jpg" },
        new Book { Id = 5, Title = "Hobbit", AuthorId = 2, Genre="Fantastik", CopiesAvailable = 100, ISBN = "0003", PublishDate = new DateTime(1998,03,01), ImageUrl = "bookImages/5.jpg" },
        new Book { Id = 6, Title = "Kürt Mantolu Madonna", AuthorId = 3, Genre="Romantik", CopiesAvailable = 100, ISBN = "0004", PublishDate = new DateTime(2000,03,01), ImageUrl= "bookImages/6.jpg" },
        new Book { Id = 7, Title = "Tüm Şiirleri", AuthorId = 3, Genre="Fantastik", CopiesAvailable = 100, ISBN = "0004", PublishDate = new DateTime(2000,03,01), ImageUrl= "bookImages/7.jpg" },
        new Book { Id = 8, Title = "Kukla", AuthorId = 5, Genre="Fantastik", CopiesAvailable = 100, ISBN = "0004", PublishDate = new DateTime(2000,03,01), ImageUrl= "bookImages/8.jpg" },
        new Book { Id = 9, Title = "Doğu Ekspresinde Cinayet", AuthorId = 6, Genre="Fantastik", CopiesAvailable = 100, ISBN = "0004", PublishDate = new DateTime(2000,03,01), ImageUrl= "bookImages/9.jpg" },


      };

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Authors = AuthorController.authors
         .Where(a => !a.IsDeleted)
         .Select(x => new BookAuthorAddViewModel
         {
             Id = x.Id,
             FirstName = x.FirstName,
             LastName = x.LastName,
         }).ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Add(BookAddViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Authors = AuthorController.authors
                    .Where(a => !a.IsDeleted)
                    .Select(x => new BookAuthorAddViewModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                    }).ToList();

                return View(formData);
            }

            string imagePath = null;

            if (formData.ImageFile != null && formData.ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formData.ImageFile.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/bookImages", fileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    formData.ImageFile.CopyTo(stream);
                }

                imagePath = "bookImages/" + fileName;
            }

            var newBook = new Book
            {
                Id = books.Count + 1,
                Title = formData.Title,
                AuthorId = formData.AuthorId,
                Genre = formData.Genre,
                PublishDate = formData.PublishDate,
                ISBN = formData.ISBN,
                CopiesAvailable = formData.CopiesAvailable,
                ImageUrl = imagePath
            };

            books.Add(newBook);
            return RedirectToAction("List");
        }
        public IActionResult List()
        {
            var bookList = books.Where(x => x.IsDeleted == false && x.CopiesAvailable > 0 && !string.IsNullOrWhiteSpace(x.Title)).Select(x => new BookListViewModel
            {
                Id = x.Id,
                Title = x.Title,
            }).OrderBy(x => x.Title).ToList();
            return View(bookList);
        }

        public IActionResult Detail(int id)
        {
            var book = books.FirstOrDefault(x => x.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            var author = AuthorController.authors.FirstOrDefault(a => a.Id == book.AuthorId);

            var bookDetail = new BookDetailViewModel
            {
                Id = book.Id,
                Title = book.Title,
                AuthorId = book.AuthorId,
                Genre = book.Genre,
                PublishDate = book.PublishDate,
                ISBN = book.ISBN,
                CopiesAvailable = book.CopiesAvailable,
                AuthorName = author != null ? $"{author.FirstName} {author.LastName}" : "Bilinmiyor",
                ImageUrl = book.ImageUrl
            };
            return View(bookDetail);
        }
        public IActionResult Delete(int id)
        {
            var book = books.FirstOrDefault(x => x.Id == id);

            book.IsDeleted = true;
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound();

            ViewBag.Authors = AuthorController.authors
                .Where(a => !a.IsDeleted)
                .Select(a => new BookAuthorAddViewModel
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName
                }).ToList();

            var model = new BookEditViewModel
            {
                Id = book.Id,
                Title = book.Title,
                AuthorId = book.AuthorId,
                Genre = book.Genre,
                PublishDate = book.PublishDate,
                ISBN = book.ISBN,
                CopiesAvailable = book.CopiesAvailable,
                ImageUrl = book.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(BookEditViewModel formData)
        {
            var book = books.FirstOrDefault(b => b.Id == formData.Id);
            if (book == null)
                return NotFound();

            // 📷 Yeni görsel yüklendiyse kaydet
            if (formData.ImageFile != null && formData.ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formData.ImageFile.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/bookImages", fileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    formData.ImageFile.CopyTo(stream);
                }

                book.ImageUrl = "bookImages/" + fileName;
            }

            // Diğer alanlar
            book.Title = formData.Title;
            book.AuthorId = formData.AuthorId;
            book.Genre = formData.Genre;
            book.PublishDate = formData.PublishDate;
            book.ISBN = formData.ISBN;
            book.CopiesAvailable = formData.CopiesAvailable;

            return RedirectToAction("List");
        }
    }
}
