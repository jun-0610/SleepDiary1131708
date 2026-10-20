using Microsoft.EntityFrameworkCore;

namespace SleepDiary1131708.Models
{
    public class SleepDiaryDbContext : DbContext
    {
        public SleepDiaryDbContext(DbContextOptions<SleepDiaryDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SleepLog> SleepRecords { get; set; }

        public DbSet<SleepLog> SleepLogs => SleepRecords;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("TableUsers1131708");
                entity.HasKey(e => e.UserID);
                entity.Property(e => e.UserID).HasColumnName("UserID");
                entity.Property(e => e.Account).HasColumnName("Account").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Password).HasColumnName("Password").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Name).HasColumnName("Name").HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<SleepLog>(entity =>
            {
                entity.ToTable("TableSleepRecords1131708");
                entity.HasKey(e => e.RecordID);
                entity.Property(e => e.RecordID).HasColumnName("RecordID");
                entity.Property(e => e.UserID).HasColumnName("UserID");
                entity.Property(e => e.SleepDate).HasColumnName("SleepDate").HasColumnType("date");
                entity.Property(e => e.SleepHours).HasColumnName("SleepHours").HasColumnType("decimal(4,1)");
                entity.Property(e => e.QualityScore).HasColumnName("QualityScore");
                entity.Property(e => e.Reason).HasColumnName("Reason").HasMaxLength(200);
                entity.Property(e => e.Remark).HasColumnName("Remark").HasMaxLength(200);

                entity.HasOne(e => e.User)
                    .WithMany(e => e.SleepRecords)
                    .HasForeignKey(e => e.UserID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}
