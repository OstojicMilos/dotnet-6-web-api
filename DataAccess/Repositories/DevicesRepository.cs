using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.IRepositories;
using CleanArchitecture.Core.Models;
using CleanArchitecture.DataAccess.Database;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.DataAccess.Repositories
{
    public sealed class DevicesRepository : Repository<Device, string>, IDevicesRepository
    {
        public DevicesRepository(ApplicationContext context) : base(context) { }

        public async Task DeactivateRegistrationsAsync(string serialNumber)
        {
            var registrations = _context.DeviceRegistrations
                .Where(registration => registration.DeviceSerialNumber == serialNumber && registration.Active);

            await foreach (var registration in registrations.AsAsyncEnumerable())
            {
                registration.Active = false;
            }
        }

        public async Task<Device> GetAsync(string serialNumber, string sharedSecret)
        {
            var device = await _context.Devices.FirstOrDefaultAsync(i => i.SerialNumber == serialNumber &&
                                                          i.SharedSecret == sharedSecret);

            if (device == null) throw new NonExistentDeviceException($"Device with serial number {serialNumber} and provided secret doesn't exist.");

            return device;
        }
    }
}
