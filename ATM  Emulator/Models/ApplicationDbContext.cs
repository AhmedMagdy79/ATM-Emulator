using Microsoft.EntityFrameworkCore;

namespace ATM__Emulator.Models
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
