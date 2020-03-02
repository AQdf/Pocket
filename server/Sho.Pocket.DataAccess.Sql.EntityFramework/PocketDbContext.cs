using Microsoft.EntityFrameworkCore;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.EntityFramework
{
    public class PocketDbContext : DbContext
    {
        public PocketDbContext(DbContextOptions<PocketDbContext> options)
            : base(options)
        {
        }

        public DbSet<Asset> Assets { get; set; }

        public DbSet<Balance> Balances { get; set; }

        public DbSet<BalanceNote> BalanceNotes { get; set; }

        public DbSet<AssetBankAccount> BankAccounts { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<ExchangeRate> ExchangeRates { get; set; }

        public DbSet<UserCurrency> UserCurrencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>().ToTable("Asset");
            modelBuilder.Entity<Balance>().ToTable("Balance");
            modelBuilder.Entity<BalanceNote>().ToTable("BalanceNote");
            modelBuilder.Entity<AssetBankAccount>().ToTable("AssetBankAccount");
            modelBuilder.Entity<Currency>().ToTable("Currency");
            modelBuilder.Entity<ExchangeRate>().ToTable("ExchangeRate");
            modelBuilder.Entity<UserCurrency>().ToTable("UserCurrency");

            modelBuilder.Entity<Asset>().HasKey(c => c.Id);
            modelBuilder.Entity<Asset>().HasOne<Currency>().WithMany().HasForeignKey(c => c.Currency).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Asset>().HasMany(a => a.Balances).WithOne(b => b.Asset).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Asset>(eb =>
            {
                eb.Property(a => a.Name).HasColumnType("nvarchar(50)").IsRequired();
                eb.Property(a => a.Currency).HasColumnType("char(3)").IsRequired();
                eb.Property(a => a.IsActive).HasColumnType("bit").IsRequired();
                eb.Property(a => a.UserId).HasColumnType("uniqueidentifier").IsRequired();
                eb.Property(a => a.Value).HasColumnType("money").IsRequired().HasDefaultValue(0);
                eb.Property(a => a.UpdatedOn).HasColumnType("datetime2(7)").IsRequired().HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<Balance>().HasKey(c => c.Id);
            modelBuilder.Entity<Balance>().HasOne(b => b.Asset).WithMany(a => a.Balances).HasForeignKey(c => c.AssetId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Balance>().HasOne(b => b.ExchangeRate).WithMany().HasForeignKey(c => c.ExchangeRateId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Balance>(eb =>
            {
                eb.Property(i => i.EffectiveDate).HasColumnType("date").IsRequired();
                eb.Property(i => i.Value).HasColumnType("money").IsRequired();
                eb.Property(i => i.UserId).HasColumnType("uniqueidentifier").IsRequired();
            });

            modelBuilder.Entity<BalanceNote>().HasKey(c => c.Id);
            modelBuilder.Entity<BalanceNote>(eb =>
            {
                eb.Property(i => i.EffectiveDate).HasColumnType("date").IsRequired();
                eb.Property(i => i.Content).HasColumnType("nvarchar(max)").IsRequired();
            });

            modelBuilder.Entity<AssetBankAccount>().HasKey(c => c.Id);
            modelBuilder.Entity<AssetBankAccount>().HasOne(a => a.Asset).WithMany().HasForeignKey(c => c.AssetId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AssetBankAccount>(eb =>
            {
                eb.Property(c => c.BankName).HasColumnType("varchar(50)").IsRequired();
                eb.Property(c => c.BankAccountId).HasColumnType("varchar(200)").IsRequired(false);
                eb.Property(c => c.BankAccountName).HasColumnType("nvarchar(200)").IsRequired(false);
                eb.Property(c => c.Token).HasColumnType("nvarchar(max)").IsRequired(false);
                eb.Property(c => c.BankClientId).HasColumnType("varchar(50)").IsRequired(false);
                eb.Property(c => c.LastSyncDateTime).HasColumnType("datetime2(7)").IsRequired(false);
            });

            modelBuilder.Entity<Currency>().HasKey(c => c.Name);
            modelBuilder.Entity<Currency>(eb =>
            {
                eb.Property(c => c.Name).HasColumnType("char(3)");
            });

            modelBuilder.Entity<ExchangeRate>().HasKey(c => c.Id);
            modelBuilder.Entity<ExchangeRate>().HasOne<Currency>().WithMany().HasForeignKey(c => c.BaseCurrency).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ExchangeRate>().HasOne<Currency>().WithMany().HasForeignKey(c => c.CounterCurrency).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ExchangeRate>(eb =>
            {
                eb.Property(c => c.EffectiveDate).HasColumnType("date").IsRequired();
                eb.Property(i => i.BaseCurrency).HasColumnType("char(3)").IsRequired();
                eb.Property(i => i.CounterCurrency).HasColumnType("char(3)").IsRequired();
                eb.Property(i => i.BuyRate).HasColumnType("money").IsRequired();
                eb.Property(i => i.SellRate).HasColumnType("money").IsRequired();
                eb.Property(i => i.Provider).HasColumnType("varchar(50)").IsRequired();
            });

            modelBuilder.Entity<UserCurrency>().HasKey(c => new { c.UserId, c.Currency });
            modelBuilder.Entity<UserCurrency>().HasOne<Currency>().WithMany().HasForeignKey(c => c.Currency).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserCurrency>(eb =>
            {
                eb.Property(c => c.UserId).HasColumnType("uniqueidentifier").IsRequired();
                eb.Property(c => c.Currency).HasColumnType("char(3)").IsRequired();
                eb.Property(i => i.IsPrimary).HasColumnType("bit").IsRequired();
            });
        }
    }
}
