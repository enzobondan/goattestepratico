// ApplicationDBContext.cs
using financeControl.Models;
using Microsoft.EntityFrameworkCore;

namespace financeControl.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTenantRole> UserTenantRoles { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<CostCenter> CostCenters { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<FinancialObligation> FinancialObligations { get; set; }
    
        public DbSet<AccountRole> AccountRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureUserTenantRole(builder);
            ConfigureIndexes(builder);
            ConfigureFinancialObligationRelationships(builder);
            ConfigureDecimalPrecision(builder);
            
            ConfigureAccountRole(builder);
        }
        private void ConfigureAccountRole(ModelBuilder builder)
        {
            builder.Entity<AccountRole>()
                .HasKey(ar => ar.Id);

            builder.Entity<AccountRole>()
                .HasOne(ar => ar.User)
                .WithOne(u => u.AccountRole)
                .HasForeignKey<AccountRole>(ar => ar.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AccountRole>()
                .HasOne(ar => ar.Account)
                .WithMany(a => a.AccountRoles)
                .HasForeignKey(ar => ar.AccountId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AccountRole>()
                .HasIndex(ar => ar.UserId)
                .IsUnique();

            builder.Entity<AccountRole>()
                .HasIndex(ar => ar.AccountId);
        }

        private void ConfigureUserTenantRole(ModelBuilder builder)
        {
            builder.Entity<UserTenantRole>()
                .HasKey(utr => new { utr.UserId, utr.TenantId });

            builder.Entity<UserTenantRole>()
                .HasOne(utr => utr.User)
                .WithMany(u => u.UserTenantRoles)
                .HasForeignKey(utr => utr.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserTenantRole>()
                .HasOne(utr => utr.Tenant)
                .WithMany(t => t.UserTenantRoles)
                .HasForeignKey(utr => utr.TenantId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private void ConfigureDecimalPrecision(ModelBuilder builder)
        {
            builder.Entity<Tenant>()
                .Property(t => t.LimiteAprovacaoAutomatica)
                .HasPrecision(18, 2);
        }

        private void ConfigureIndexes(ModelBuilder builder)
        {
            builder.Entity<Tenant>().HasIndex(t => t.Cnpj).IsUnique();
            builder.Entity<Account>().HasIndex(a => a.Cnpj).IsUnique();
            builder.Entity<Vendor>().HasIndex(v => new { v.CnpjCpf, v.TenantId }).IsUnique();
            builder.Entity<CostCenter>().HasIndex(cc => new { cc.Codigo, cc.TenantId }).IsUnique();
            builder.Entity<ExpenseCategory>().HasIndex(ec => new { ec.Nome, ec.TenantId }).IsUnique();
            
            builder.Entity<AccountRole>().HasIndex(ar => ar.UserId);
            builder.Entity<AccountRole>().HasIndex(ar => ar.AccountId);
        }

        private void ConfigureFinancialObligationRelationships(ModelBuilder builder)
        {
            var financialObligationBuilder = builder.Entity<FinancialObligation>();

            financialObligationBuilder
                .HasOne(fo => fo.Tenant)
                .WithMany(t => t.FinancialObligations)
                .HasForeignKey(fo => fo.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            financialObligationBuilder
                .HasOne(fo => fo.Vendor)
                .WithMany(v => v.FinancialObligations)
                .HasForeignKey(fo => fo.VendorId)
                .OnDelete(DeleteBehavior.NoAction);

            financialObligationBuilder
                .HasOne(fo => fo.CostCenter)
                .WithMany(cc => cc.FinancialObligations)
                .HasForeignKey(fo => fo.CostCenterId)
                .OnDelete(DeleteBehavior.NoAction);

            financialObligationBuilder
                .HasOne(fo => fo.ExpenseCategory)
                .WithMany(ec => ec.FinancialObligations)
                .HasForeignKey(fo => fo.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}