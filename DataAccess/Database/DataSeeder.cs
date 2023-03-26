using CleanArchitecture.Core.Models;

namespace CleanArchitecture.DataAccess.Database
{
    public sealed class DataSeeder
    {
        public static void Seed(ApplicationContext dbContext)
        {
            if (dbContext.Devices.Any()) return;

            var testData = Enumerable.Range(1, 5).Select(i => new Device
            {
                SerialNumber = $"test-ABC-123-XYZ-00{i}",
                SharedSecret = $"secret-ABC-123-XYZ-00{i}"
            });

            dbContext.Devices.AddRange(testData);
            dbContext.SaveChanges();
        }
    }
}
