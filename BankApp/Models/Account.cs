using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleBank.Models
{
    public class Account : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int Balance { get; set; }
        //public int AccountNumber { get; set; }
        //public int RoutingNumber { get; set; }
        public int CardNumber { get; set; }
        public int PIN { get; set; }
        public bool isMain { get; set; }

        public List<Transaction> Transactions { get; set; }

        public Account()
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
            //RoutingNumber = 19920705;
            Transactions = new List<Transaction>();
        }
    }
}