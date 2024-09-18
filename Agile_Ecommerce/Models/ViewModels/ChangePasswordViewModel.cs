using System.ComponentModel.DataAnnotations;

namespace Agile_Ecommerce.Models.ViewModels
{
    public class ChangePasswordViewModel
   
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmPassword { get; set; }
        }
    }


