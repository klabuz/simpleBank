using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleBank.Models
{
    public class AccountViewModel : BaseEntity
    {
        public Create Cre { get; set; }

        public Edit Edi { get; set; }

        public Statement Stat { get; set; }
    }

    public class Create : BaseEntity
    {
        [Required]
        [Display(Name = "account type")]
        public string Type { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Your account name must contain at least 2 characters.")]
        [Display(Name = "account name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "balance")]
        public int Balance { get; set; }
    }

    public class Edit : BaseEntity
    {
        [MinLength(2, ErrorMessage = "Your account name must contain at least 2 characters.")]
        [Display(Name = "account name")]
        public string Name { get; set; }

        [MinLength(4, ErrorMessage = "Your account name must contain 4 characters.")]
        [MaxLength(4, ErrorMessage = "Your account name must contain 4 characters.")]
        [Display(Name = "pin number")]
        [Compare("PINConfirm", ErrorMessage = "PIN numbers don't match.")]

        public string PIN { get; set; }

        [MinLength(4, ErrorMessage = "Your account name must contain 4 characters.")]
        [MaxLength(4, ErrorMessage = "Your account name must contain 4 characters.")]
        [Display(Name = "confirm pin number")]
        [Compare("PIN", ErrorMessage = "PIN numbers don't match.")]

        public string PINConfirm { get; set; }
    }

    public class Statement : BaseEntity
    {
        public int AccountId { get; set; }

        [Required]
        [Display(Name = "from")]
        public DateTime startDate { get; set; }

        [Required]
        [Display(Name = "to")]
        public DateTime endDate { get; set; }

    }
}