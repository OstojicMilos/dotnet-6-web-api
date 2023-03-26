using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.IServices;
using CleanArchitecture.Application.Mapping;
using CleanArchitecture.Core.IRepositories;
using CleanArchitecture.Core.Models;
using Mapster;

namespace CleanArchitecture.Application.Services
{
    public sealed class DevicesService : IDevicesService
    {
        private IDevicesRepository _devicesRepository;
        private IDeviceRegistrationsRepository _deviceRegistrationsRepository;
        private IDeviceReadingsRepository _readingsRepository;
        private IAlertsService _alertsService;

        static DevicesService() => MapperConfig.ConfigureReadingsMapping();

        public DevicesService(IDevicesRepository devicesRepository,
            IDeviceRegistrationsRepository deviceRegistrationsRepository,
            IDeviceReadingsRepository readingsRepository,
            IAlertsService alertsService)
        {
            _devicesRepository = devicesRepository;
            _deviceRegistrationsRepository = deviceRegistrationsRepository;
            _readingsRepository = readingsRepository;
            _alertsService = alertsService;
        }

        public async Task HandleReadingsAsync(IEnumerable<DeviceReadingDto> readingDtos, string serialNumber)
        {
            var dbReadings = MapToDeviceReadings(readingDtos, serialNumber);
            await _readingsRepository.AddRangeAsync(dbReadings);

            await _alertsService.AddOrUpdateAlertsAsync(dbReadings, serialNumber);
        }

        public async Task RegisterAsync(DeviceRegistrationDto dto)
        {
            await _devicesRepository.DeactivateRegistrationsAsync(dto.SerialNumber);

            var device = await _devicesRepository.GetAsync(dto.SerialNumber, dto.SharedSecret);
            var deviceRegistration = await AddDeviceRegistrationAsync(device, dto.TokenId);

            device.FirmwareVersion = dto.FirmwareVersion;
            device.FirstRegistrationDate ??= deviceRegistration.RegistrationDate;
            device.LastRegistrationDate = deviceRegistration.RegistrationDate;

            await _devicesRepository.SaveChangesAsync();
        }

        private IEnumerable<DeviceReading> MapToDeviceReadings(IEnumerable<DeviceReadingDto> readings, string serialNumber)
        {
            using (var scope = new MapContextScope())
            {
                scope.Context.Parameters[nameof(DeviceReading.DeviceSerialNumber)] = serialNumber;
                return readings.AsQueryable().ProjectToType<DeviceReading>().ToList();
            }
        }

        private async Task<DeviceRegistration> AddDeviceRegistrationAsync(Device device, string tokenId)
        {
            var deviceRegistration = new DeviceRegistration
            {
                Device = device,
                TokenId = tokenId
            };

            return await _deviceRegistrationsRepository.AddAsync(deviceRegistration);
        }
    }
}
