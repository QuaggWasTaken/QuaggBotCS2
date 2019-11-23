using Microsoft.EntityFrameworkCore;

namespace QuaggBotCS2
{
    public class BotContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Server> Servers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DataHandler.ConnString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Server>(entity =>
            {
                entity.HasKey(e => e.ServerID);
                entity.Property(e => e.__ServerSnow).IsRequired();
                entity.HasMany(d => d.Users).WithOne(s => s.Guild);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserID);
                entity.Property(e => e.__UserSnow).IsRequired();
                entity.HasOne(d => d.Guild).WithMany(s => s.Users);
            });
        }
    }
}
