using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBank.Models
{
    public class Stock : BaseEntity
    {
        [Key]
        public int StockId { get; set; }
        public string Price { get; set; }
        public string PreMarketPrice { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public List<StockTransaction> StockTransactions { get; set; }

        public Stock()
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
            StockTransactions = new List<StockTransaction>();
        }
    }
}