using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
namespace ListaZaginionych.Models
{
    public class LostPeopleViewModel
    {
        [Required(ErrorMessage = "Proszę podać imie!")]
        [Display(Name ="Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="Proszę podać nazwisko!")]
        [Display(Name ="Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Proszę podać wiek")]
        [Display(Name ="Wiek")]
        public int Age { get; set; }

        [Required(ErrorMessage ="Wybierz płeć")]
        [Display(Name = "Płeć")]
        public string Gender { get; set; }

        [Required(ErrorMessage ="Proszę podać nazwę Miejscowości")]
        [Display(Name ="Ostatnio widziano w (miejscowość)")]
        public string LastSeenLocation { get; set; }

        [Required(ErrorMessage ="Proszę podać datę")]
        [Display(Name ="Ostatnio widziany (data):")]
        public DateTime LastSeenDate { get; set; }

        [Display(Name ="Zdjecie poszukiwanego")]
        public IFormFile Image { get; set; }

        public string ImagePath { get; set; }

        public int Id { get; set; }
    }
}
