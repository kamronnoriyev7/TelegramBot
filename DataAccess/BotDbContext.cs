using DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace TelegramBot.Server
{

    public class BotDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // MSSQL uchun connection string
            string connectionString = "Server=.\\SQLEXPRESS;Database=RamadanDbBot;User Id=sa;Password=1;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Bu yerda jadval tuzilmalari sozlanadi
            modelBuilder.ApplyConfiguration(new Userconfiguration());
        }
    }


}