using CleanArchitecture.Core.IRepositories;
using CleanArchitecture.Core.Models;
using CleanArchitecture.DataAccess.Database;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.DataAccess.Repositories
{
    public sealed class DeviceRegistrationsRepository : Repository<DeviceRegistration, int>, IDeviceRegistrationsRepository
    {
        public DeviceRegistrationsRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<bool> IsAccessAuthorizedAsync(string serialNumber, string tokenId)
        {
            return await _context.DeviceRegistrations.AnyAsync(
                registration => registration.DeviceSerialNumber == serialNumber &&
                                registration.TokenId == tokenId &&
                                registration.Active);
        }
    }
}
