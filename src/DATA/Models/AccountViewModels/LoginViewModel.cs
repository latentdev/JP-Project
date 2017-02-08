using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DATA.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string username { get; set; }

        //[Required]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name = "Remember me?")]
        public bool rememberMe { get; set; }
    }
}
