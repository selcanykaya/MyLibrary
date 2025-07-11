using System.Reflection.Metadata;

namespace Kütüphanem.Models.Author
{
    public class Author : BaseClass
    {
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
        public string Description { get; set; }

     
        
    }
}
