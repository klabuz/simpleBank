using System.ComponentModel.DataAnnotations;

namespace SimpleBank.Models
{
    public class SelfTransferViewModel : BaseEntity
    {
        public SelfTransfer Tra { get; set; }
    }

    public class SelfTransfer : BaseEntity
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