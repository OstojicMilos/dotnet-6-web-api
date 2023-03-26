using CleanArchitecture.Core.Enums;

namespace CleanArchitecture.Application.DTOs
{
    public sealed class DeviceReadingDto
    {
        public DateTimeOffset RecordedDateTime { get; set; }
        public decimal Temperature { get; set; }
        public decimal Humidity { get; set; }
        public decimal CarbonMonoxide { get; set; }
        public DeviceHealth Health { get; set; }
    }
}
