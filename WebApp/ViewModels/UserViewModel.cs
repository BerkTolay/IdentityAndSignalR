using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı Gereklidir")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Ad Gereklidir")]
        [Display(Name="Adı")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Soyad Gereklidir")]
        [Display(Name = "Soyadı")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Email Gereklidir")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre Gereklidir")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}