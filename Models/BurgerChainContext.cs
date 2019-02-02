using System;
using DigitalSignage.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DigitalSignage.Models
{
    public partial class BurgerChainContext : DbContext
    {
        private LoginManager _loginManager;

        public BurgerChainContext()
        {
        }

        public BurgerChainContext(
            DbContextOptions<BurgerChainContext> options,
            LoginManager loginManager
        )
            : base(options)
        {
            _loginManager = loginManager;
        }

        public virtual DbSet<ZcMenuDetail> ZcMenuDetail { get; set; }
        public virtual DbSet<ZcMenuHeader> ZcMenuHeader { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_loginManager.GetConnectionString());
        }

        public bool IsValid() {
            return _loginManager.IsConnected();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ZcMenuHeader>(entity =>
            {
                entity.HasKey(e => e.MenuheaderCode)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("ZC_MenuHeader");

                entity.Property(e => e.MenuheaderCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Background)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FromDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('17991231')");

                entity.Property(e => e.Storage)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tbguid)
                    .HasColumnName("TBGuid")
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.ToDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('17991231')");

                entity.Property(e => e.Week).HasDefaultValueSql("((0))");
            });
            modelBuilder.Entity<ZcMenuDetail>(entity =>
            {
                entity.HasKey(e => new { e.MenuheaderCode, e.Day, e.MenuId })
                    .ForSqlServerIsClustered(false);

                entity.ToTable("ZC_MenuDetail");

                entity.Property(e => e.MenuheaderCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MenuId).HasColumnName("MenuID");

                entity.Property(e => e.Background)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Estimatedquantity)
                    .HasColumnName("estimatedquantity")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ItemCode)
                    .HasMaxLength(21)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Picture)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SalesPrice).HasDefaultValueSql("((0))");
            });
        }
    }
}
