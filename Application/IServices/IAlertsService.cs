using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Core.Models;
using CleanArchitecture.Core.Settings;

namespace CleanArchitecture.Application.IServices
{
    public interface IAlertsService
    {
        public Task AddOrUpdateAlertsAsync(IEnumerable<DeviceReading> readings, string serialNumber);
        public Task<IEnumerable<AlertDto>> GetAlertsAsync(string serialNumber, AlertQuery query);
    }
}
