using CleanArchitecture.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CleanArchitecture.DataAccess.Database
{
    public sealed class ApplicationContext : DbContext
    {
        public DbSet<Device> Devices => Set<Device>();
        public DbSet<DeviceRegistration> DeviceRegistrations => Set<DeviceRegistration>();
        public DbSet<DeviceReading> DeviceReadings => Set<DeviceReading>();
        public DbSet<Alert> Alerts => Set<Alert>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<DateTimeOffset>()
                .HaveConversion<DateTimeOffsetToStringConverter>();

            configurationBuilder
                .Properties<decimal>()
                .HaveConversion<double>();
        }
    }
}
