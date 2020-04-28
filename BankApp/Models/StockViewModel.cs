using System.ComponentModel.DataAnnotations;

namespace SimpleBank.Models
{
    public class StockViewModel : BaseEntity
    {
        public StockSymbol SS { get; set; }
    }

    public class StockSymbol : BaseEntity
    {
        [Required]
        [Display(Name = "stock symbol")]
        public string Symbol { get; set; }

    }
}