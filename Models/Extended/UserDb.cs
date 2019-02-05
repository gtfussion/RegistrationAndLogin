using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RegistrationAndLogin.Models
{
    [MetadataType(typeof(UserMeataData))]
    public partial class UserDb
    {
        [DataType("Password")]
        [Compare("Password",ErrorMessage ="Password not same")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }

    public class UserMeataData
    {
        
        [Required(ErrorMessage ="Username required")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="EmailId Required")]
        
        public string EmailID { get; set; }
        [DataType("Password")]
        [Required(ErrorMessage ="Password Required")]
        public string Password { get; set; }
     
        
    }
}