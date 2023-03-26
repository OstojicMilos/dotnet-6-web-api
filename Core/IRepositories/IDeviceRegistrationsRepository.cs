using CleanArchitecture.Core.Models;

namespace CleanArchitecture.Core.IRepositories
{
    public interface IDeviceRegistrationsRepository : IRepository<DeviceRegistration, int>
    {
        public Task<bool> IsAccessAuthorizedAsync(string serialNumber, string tokenId);
    }
}
