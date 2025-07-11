namespace Kütüphanem.Models
{
    public class BaseClass
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; } = false;

        public string ImageUrl { get; set; }
    }
}
