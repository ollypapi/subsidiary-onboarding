using System;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence
{


    public class AppDbContext : DbContext, IOnboardingDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers {get;set;}
        public DbSet<Document> Documents {get;set;}
        public DbSet<ApplicationSetting> Settings { get; set; }
        public DbSet<DeviceHistory> DeviceHistories { get; set; }
        public DbSet<CustomerDevice> CustomerDevices { get; set; }
        public DbSet<Verification> Verifications {get;set;}
        public DbSet<SecurityQuestion> SecurityQuestions { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<SchemeCodeSettingPermission> SchemeCodeSettingPermissions { get; set; }
        public DbSet<SchemeCode> SchemeCodes { get ; set ; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customer>().Property(x => x.Stage)
                .IsRequired()
                .HasMaxLength(20)
                .HasConversion(new EnumToStringConverter<RegistrationStage>());
            modelBuilder.Entity<DeviceHistory>().Property(x => x.Description).HasConversion(new EnumToStringConverter<DeviceHistoryEnum>());
            modelBuilder.Entity<ApplicationSetting>().Property(x => x.SettingType).HasConversion(new EnumToStringConverter<SettingEnum>());
            modelBuilder.Entity<Document>().Property(x => x.Identification).HasConversion(new EnumToStringConverter<Identification>());
            modelBuilder.Entity<Document>().Property(x => x.DocumentType).HasConversion(new EnumToStringConverter<DocumentType>());
            modelBuilder.Entity<Verification>().Property(x => x.Status).HasConversion(new EnumToStringConverter<OtpStatus>());
            modelBuilder.Entity<SchemeCodeSettingPermission>().Property(x => x.Permission).HasConversion(new EnumToStringConverter<PermissionType>());
        }
    }
}
