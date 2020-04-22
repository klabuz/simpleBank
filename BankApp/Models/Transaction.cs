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
        public int AccountId { get; set; }
        public int ToAccountId { get; set; }
        public bool isSelfTransfer { get; set; }

        public Transaction()
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
            isSelfTransfer = false;
        }
    }

    public class TransactionsHistory : BaseEntity
    {
        public int ToAccountId { get; set; }
        public string ToAccount { get; set; }
        public int FromAccountId { get; set; }
        public string FromAccount { get; set; }
        public int Amount { get; set; }
        public int SenderId { get; set; }
        public string Sender { get; set; }
        public int ReceiverId { get; set; }
        public string Receiver { get; set; }
        public bool isSelfTransfer { get; set; }

        public TransactionsHistory(){}
    }

}