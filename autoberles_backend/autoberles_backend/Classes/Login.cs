using System.ComponentModel.DataAnnotations;

namespace autoberles_backend.Classes
{
    public class Login
    {

        [Required(ErrorMessage = "Az e-mail cím megadása kötelező.")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "A jelszó megadása kötelező.")]
        public string Password { get; set; } = null!;
    }
}
