using CleanArchitecture.Core.IRepositories;
using CleanArchitecture.Core.Models;
using CleanArchitecture.DataAccess.Database;

namespace CleanArchitecture.DataAccess.Repositories
{
    public sealed class DeviceReadingsRepository : Repository<DeviceReading, int>, IDeviceReadingsRepository
    {
        public DeviceReadingsRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
