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
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Your last name must contain at least 2 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Your username name must contain at least 2 characters.")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Your password must contain at least 8 characters.")]
        [Compare("cw_password", ErrorMessage = "Passwords don't match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string PasswordHash { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }
    }

    public class Login : BaseEntity
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Your password must contain at least 8 characters.")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
    }
}