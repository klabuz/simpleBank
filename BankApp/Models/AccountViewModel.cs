using System.ComponentModel.DataAnnotations;

namespace SimpleBank.Models
{
    public class AccountViewModel : BaseEntity
    {
        public Create Cre { get; set; }

        public Transfer Tra { get; set; }
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
}