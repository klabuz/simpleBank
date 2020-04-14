using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleBank.Models
{
    public class Transaction : BaseEntity
    {
        [Key]
        public int TransactionId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int Amount { get; set; }
        public bool isPending { get; set; }
        public bool isFinished { get; set; }

        public Transaction()
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }
    }
}