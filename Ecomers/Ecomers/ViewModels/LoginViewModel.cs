using System.ComponentModel.DataAnnotations;

namespace Ecomers.ViewModels
{
    public class LoginViewModel
    { 
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
