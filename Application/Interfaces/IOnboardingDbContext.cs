using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOnboardingDbContext
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<Document> Documents { get; set; }
        DbSet<Verification> Verifications { get; set; }
        DbSet<ApplicationSetting> Settings { get; set; }
        DbSet<SchemeCodeSettingPermission> SchemeCodeSettingPermissions { get; set; }
        DbSet<CustomerDevice> CustomerDevices { get; set; }
        DbSet<SecurityQuestion> SecurityQuestions { get; set; }
        DbSet<DeviceHistory> DeviceHistories { get; set; }
        DbSet<UserActivity> UserActivities { get; set; }
        DbSet<SchemeCode> SchemeCodes { get; set; }
        
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
