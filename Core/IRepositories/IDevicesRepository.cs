using CleanArchitecture.Core.Models;

namespace CleanArchitecture.Core.IRepositories
{
    public interface IDevicesRepository : IRepository<Device, string>
    {
        public Task DeactivateRegistrationsAsync(string serialNumber);
        public Task<Device> GetAsync(string serialNumber, string sharedSecret);
    }
}
