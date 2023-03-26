using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Core.Models;
using Mapster;

namespace CleanArchitecture.Application.Mapping
{
    public static class MapperConfig
    {
        public static void ConfigureReadingsMapping()
        {
            TypeAdapterConfig<DeviceReadingDto, DeviceReading>
                .NewConfig()
                .Map(dest => dest.ReceivedDateTime,
                     src => DateTime.UtcNow)
                .Map(dest => dest.DeviceSerialNumber,
                     src => MapContext.Current.Parameters[nameof(DeviceReading.DeviceSerialNumber)],
                     src => MapContext.Current != null);

        }
    }
}
