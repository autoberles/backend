using System.ComponentModel.DataAnnotations;

namespace autoberles_backend.Classes
{
    public class CreateUserDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Érvénytelen email formátum.")]
        public string Email { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
