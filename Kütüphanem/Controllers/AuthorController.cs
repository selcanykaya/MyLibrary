using Microsoft.AspNetCore.Mvc;
using Kütüphanem.Models.Author;
using Kütüphanem.Models.Book;
using static System.Reflection.Metadata.BlobBuilder;


namespace Kütüphanem.Controllers
{
    public class AuthorController : Controller
    {
        public static List<Author> authors = new List<Author> {
            new Author { Id = 1, FirstName = "Inio", LastName = "Asano", DateOfBirth = new DateTime(1980, 09, 22) , ImageUrl = "authorImages/1.jpg", Description ="Inio Asano bir Japon manga sanatçısıdır. Asano, Aoi Miyazaki'nin oynadığı, Nisan 2010'da Japonya'da uzun metrajlı bir film olarak gösterime giren beğenilen manga dizisi Solanin'i yarattı. Ayrıca Goodnight Punpun ve Dead Dead Demon's Dededede Destruction dizileriyle ünlüdür." },
            new Author { Id = 2, FirstName = "J.R.R.", LastName = "Tolkien", DateOfBirth = new DateTime(1892, 01, 03) , ImageUrl = "authorImages/2.jpg" ,Description ="John Ronald Reuel Tolkien, İngiliz yazar, şair, filolog ve akademisyendir. Hobbit ve Yüzüklerin Efendisi gibi fantastik kurgu eserleriyle tanınır. 1925'ten 1945'e kadar Tolkien, Oxford Üniversitesi'nde Rawlinson ve Bosworth Anglo-Sakson Profesörü ve Pembroke Koleji üyesiydi." },
            new Author { Id = 3, FirstName = "Sabahattin", LastName = "Ali", DateOfBirth = new DateTime(1907,02, 25) , ImageUrl = "authorImages/3.jpg", Description ="Sabahattin Ali, Türk yazar ve şair. Edebî kişiliğini toplumcu gerçekçi bir düzleme oturtarak yaşamındaki deneyimlerini okuyucusuna yansıttı ve kendisinden sonraki Cumhuriyet dönemi Türk edebiyatını etkileyen bir figür hâline geldi."},
            new Author { Id = 4, FirstName = "Virginia", LastName = "Woolf", DateOfBirth = new DateTime(1882, 01, 25) , ImageUrl = "authorImages/4.jpg" ,Description ="Virginia Woolf, İngiliz feminist, yazar, romancı ve eleştirmen"},
            new Author { Id = 5, FirstName = "Ahmet", LastName = "Ümit" , DateOfBirth = new DateTime(1960, 01, 01) , ImageUrl = "authorImages/5.jpg" , Description = "Ahmet Ümit, Türk şair ve yazardır. Daha çok polisiye roman türünde eser veren bir yazardır."},
            new Author { Id = 6, FirstName = "Agatha", LastName = "Christie", DateOfBirth = new DateTime(1890, 09, 15) , ImageUrl = "authorImages/6.jpg" ,Description = "Tam adı Agatha Mary Clarissa Miller Christie Mallowan olan Agatha Christie, İngiliz yazar, polisiye edebiyatın en önemli isimlerinden biri ve dedektif Hercule Poirot karakterinin yaratıcısıdır. Mary Westmacott takma adıyla aşk romanları da yazmıştır." },
        };
    
    public IActionResult List()
        {
            var authorList = authors.Where(x => x.IsDeleted == false && !string.IsNullOrWhiteSpace(x.FirstName)).Select(x => new AuthorListViewModel
            {
                Id = x.Id,
                FullName = x.FullName,
            }).OrderBy(x => x.FullName).ToList();
            return View(authorList);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AuthorAddViewModel formData)
        {
            string imagePath = null;

            // Eğer görsel yüklendiyse dosyayı kaydet
            if (formData.ImageFile != null && formData.ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formData.ImageFile.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/authorImages", fileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    formData.ImageFile.CopyTo(stream);
                }

                imagePath = "authorImages/" + fileName; // wwwroot'tan sonrası
            }

            int newId = authors.Count == 0 ? 1 : authors.Max(a => a.Id) + 1;

            var newAuthor = new Author
            {
                Id = newId,
                FirstName = formData.FirstName,
                LastName = formData.LastName,
                DateOfBirth = formData.DateOfBirth,
                Description = formData.Description,
                ImageUrl = imagePath // bu alan artık görsel yolunu içeriyor
            };

            authors.Add(newAuthor);
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var author = authors.FirstOrDefault(x => x.Id == id);
            if (author == null)
                return NotFound();

            var model = new AuthorEditViewModel
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                DateOfBirth = author.DateOfBirth,
                Description = author.Description,
                ImageUrl = author.ImageUrl
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(AuthorEditViewModel formData)
        {
            var author = authors.FirstOrDefault(x => x.Id == formData.Id);
            if (author == null)
                return NotFound();

            author.FirstName = formData.FirstName;
            author.LastName = formData.LastName;
            author.DateOfBirth = formData.DateOfBirth;
            author.Description = formData.Description;

            // Yeni görsel yüklendiyse
            if (formData.ImageFile != null && formData.ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formData.ImageFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/authorImages");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var savePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    formData.ImageFile.CopyTo(stream);
                }

                // Eski görsel varsa sil
                if (!string.IsNullOrEmpty(author.ImageUrl))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", author.ImageUrl);
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                author.ImageUrl = "authorImages/" + fileName;
            }

            return RedirectToAction("List");
        }


        [HttpGet]
        public IActionResult Detail(int id)
        {
            var author = authors.FirstOrDefault(x => x.Id == id);
            if (author == null)
                return NotFound();
            
            var booksByAuthor = BookController.books.Where(x => x.AuthorId == id && x.IsDeleted == false)
                .Select(x => new BookSummaryViewModel
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList();
           
            var authorDetail = new AuthorDetailViewModel
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                DateOfBirth = author.DateOfBirth,
                ImageUrl = author.ImageUrl,
                Description = author.Description,
                Books = booksByAuthor
            };

            return View(authorDetail);
        }

        public IActionResult Delete(int id)
        {
            var author = authors.FirstOrDefault(x => x.Id == id);

            author.IsDeleted = true;
            author.FirstName = "Bilinmiyor";
            author.LastName = "";

            return RedirectToAction("List");
        }

    
    }

}