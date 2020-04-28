using Microsoft.EntityFrameworkCore;

namespace SimpleBank.Models
{
    public class SimpleBankContext : DbContext
    {
        public SimpleBankContext(DbContextOptions<SimpleBankContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<Stock> Stocks { get; set; }
    }
}
