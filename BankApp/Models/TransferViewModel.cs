using System.ComponentModel.DataAnnotations;

namespace SimpleBank.Models
{
    public class TransferViewModel : BaseEntity
    {
        public Transfer Tra { get; set; }
    }

    public class Transfer : BaseEntity
    {
        [Required]
        [Display(Name = "to account")]
        public int ToAccount { get; set; }

        [Required]
        [Display(Name = "from account")]
        public int FromAccount { get; set; }

        [Required]
        [Display(Name = "amount")]
        public int Amount { get; set; }
    }
}