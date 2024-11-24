using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class School
    {
        public int SchoolID { get; set; }

        [Required(ErrorMessage = "Le nom de l'école est requis.")]
        public string SchoolName { get; set; }

        [Required(ErrorMessage = "L'adresse de l'école est requise.")]
        public string SchoolAdress { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
