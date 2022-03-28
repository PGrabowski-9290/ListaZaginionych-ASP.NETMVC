using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace ListaZaginionych.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; } 
        public string Email { get; set; }
        [Display(Name = "Role")]
        public IEnumerable<string> Roles { get; set; }
    }
}
