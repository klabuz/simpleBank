using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBank.Models
{
    public class Account : BaseEntity
    {
        [Key]
        public int AccountId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int Balance { get; set; }
        //public int AccountNumber { get; set; }
        //public int RoutingNumber { get; set; }
        public string CardNumber { get; set; }
        public string PIN { get; set; }
        public bool isMain { get; set; }
        public int UserId { get; set; }  

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