using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace TelegramBot.Server
{

    public class MainContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // MSSQL uchun connection string
            string connectionString = "Server=.\\SQLEXPRESS;Database=RamadanDbBot;User Id=sa;Password=1;";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

}