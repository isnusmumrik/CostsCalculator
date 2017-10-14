using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CostsCalculator.Models.ServieceModels
{
    public class UserData
    {
        [HiddenInput(DisplayValue = false)]
        public  int Id { get; set; }

        [Required]
        [Remote("CheckPassword", "Account", HttpMethod = "POST", ErrorMessage = "UserName already taken!")]
        [DataType(DataType.Password)]      
        public  string OldPassword { get;set;}

        [Required]
        [DataType(DataType.Password)]      
        public  string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public  string ConfirmedNewPassword { get; set; }
    }
}