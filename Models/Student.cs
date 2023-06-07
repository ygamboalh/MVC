using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Name is required")]
        
        public string Name { get; set; }

        [Required(ErrorMessage = "Career is required")]
        public string Career { get; set; }
        
        [Required(ErrorMessage ="Age is required")]
        [Range(18,80,ErrorMessage ="Age must be valid")]
        public int Age { get; set; }
        
        [EmailAddress(ErrorMessage ="The format for the email is not correct")]
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; }

        public string Year { get; set; }

        public bool Housed { get; set; }

        public DateTime Registered { get; set; }
    }
}
