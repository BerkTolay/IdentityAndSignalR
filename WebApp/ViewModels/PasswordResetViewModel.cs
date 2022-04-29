using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class PasswordResetViewModel
    {
        [Required(ErrorMessage = "Email Gereklidir")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Yeni Şifre")]
        [Required(ErrorMessage = "Şifre gereklidir")]
        [DataType(DataType.Password)]
        public string PasswordNew { get; set; }
    }
}