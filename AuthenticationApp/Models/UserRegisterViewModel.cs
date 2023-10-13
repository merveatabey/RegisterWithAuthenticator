using System.ComponentModel.DataAnnotations;

namespace AuthenticationApp.Models
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "Bu alanı boş bırakamazsınız")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bu alanı boş bırakamazsınız")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Bu alanı boş bırakamazsınız")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Bu alanı boş bırakamazsınız")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bu alanı boş bırakamazsınız")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bu alanı boş bırakamazsınız")]
        [Compare("Password", ErrorMessage = "Şifreler uyumlu değil" )]
        public string ConfirmPassword { get; set; }
    }
}
