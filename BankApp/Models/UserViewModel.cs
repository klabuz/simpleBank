using System.ComponentModel.DataAnnotations;

namespace SimpleBank.Models
{
    public class UserViewModel : BaseEntity
    {
        public SignUp SUP { get; set; }
        public Login Log { get; set; }
    }

    public class SignUp : BaseEntity
    {
        [Required]
        [MinLength(2, ErrorMessage = "Your first name must contain at least 2 characters.")]
        [Display(Name = "first name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Your last name must contain at least 2 characters.")]
        [Display(Name = "last name")]
        public string LastName { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Your username name must contain at least 2 characters.")]
        [Display(Name = "username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "email address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "phone number")]
        public string Phone { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Your password must contain at least 8 characters.")]
        [Compare("ConfirmPassword", ErrorMessage = "Passwords don't match.")]
        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string PasswordHash { get; set; }

        [Required]
        [Compare("PasswordHash", ErrorMessage = "Passwords don't match.")]
        [DataType(DataType.Password)]
        [Display(Name = "confirm password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "state")]
        public string State { get; set; }

        [Required]
        [Display(Name = "zip code")]
        public string ZipCode { get; set; }

        [Required]
        [Display(Name = "street address")]
        public string StreetAddress { get; set; }

        [Required]
        [Display(Name = "city")]
        public string City { get; set; }

        [Required]
        [Display(Name = "country")]
        public string Country { get; set; }
    }

    public class Login : BaseEntity
    {
        [Required]
        [Display(Name = "username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "password")]
        [MinLength(8, ErrorMessage = "Your password must contain at least 8 characters.")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
    }
}