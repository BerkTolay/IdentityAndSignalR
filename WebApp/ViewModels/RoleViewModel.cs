using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class RoleViewModel
    {
        
        [Required(ErrorMessage = "Role ismi gereklidir")]
        [Display(Name = "Role ismi")]
        public string Name { get; set; }

        public string Id { get; set; }
    }
}