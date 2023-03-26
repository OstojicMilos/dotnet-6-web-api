using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.IServices
{
    public interface IDevicesService
    {
        public Task RegisterAsync(DeviceRegistrationDto dto);
        public Task HandleReadingsAsync(IEnumerable<DeviceReadingDto> readingDtos, string serialNumber);
    }
}
