using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SportyWebApp.Models
{
    public class UserRegisterModel
    {
        [Display(Name = "Ime")]
        [Required(ErrorMessage = "Unesite ime!")]
        public string FirstName { get; set; }

        [Display(Name = "Prezime")]
        [Required(ErrorMessage = "Unesite prezime!")]
        public string LastName { get; set; }

        [Display(Name = "Korisničko ime")]
        [Required(ErrorMessage = "Unesite korisničko ime!")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Unesite ispravan email!")]
        [Required(ErrorMessage = "Unesite email!")]
        public string Email { get; set; }

        //[Display(Name = "Broj mobitela")]
        //[Required(ErrorMessage = "Unesite broj mobitela!")]
        //public string PhoneNumber { get; set; }

        [Display(Name = "Grad")]
        [Required(ErrorMessage = "Unesite grad!")]
        public string City { get; set; }

        //[Display(Name = "Spol")]
        //[Required(ErrorMessage = "Odaberite spol!")]
        //public string Gender { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Unesite lozinku!")]
        [StringLength(255, ErrorMessage = "Lozinka mora imati najmanje 6 znakova!", MinimumLength = 6)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Unesite lozinku opet!")]
        [StringLength(255, ErrorMessage = "Lozinka mora imati najmanje 6 znakova!", MinimumLength = 6)]
        [Display(Name = "Ponovljena ozinka")]
        [Compare(nameof(Password), ErrorMessage = "Lozinke se ne podudaraju!")]
        public string ConfirmPassword { get; set; }

        public bool IsChecked { get; set; }
        public bool PasswordMatch { get; set; }

        public bool EmptyCheck()
        {
            if (FirstName == null || LastName == null || UserName == null || Email == null || City == null || Password == null || ConfirmPassword == null)
            {
                IsChecked = false;
                return false;
            }
            IsChecked = true;
            return true;
        }
        public bool PasswordCheck()
        {
            if (Password.Equals(ConfirmPassword))
            {
                PasswordMatch = true;
                return true;
            }
            PasswordMatch = false;
            return false;
        }
    }
}