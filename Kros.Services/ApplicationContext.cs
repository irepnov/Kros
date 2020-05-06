using Microsoft.EntityFrameworkCore;

namespace Kros.Services
{

    //https://docs.microsoft.com/ru-ru/ef/core/get-started/?tabs=netcore-cli
    //https://metanit.com/sharp/uwp/16.1.php
    public sealed class ApplicationContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite("Data Source=mobiles.db");
        public DbSet<Phone> Phones { get; set; }
    }
}
