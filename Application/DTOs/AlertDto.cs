using CleanArchitecture.Core.Enums;

namespace CleanArchitecture.Application.DTOs
{
    public sealed class AlertDto
    {
        public int AlertId { get; set; }
        public AlertType Type { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset FirstSensorReadingDate { get; set; }
        public DateTimeOffset LastSensorReadingDate { get; set; }
        public string Message { get; set; } = null!;
        public AlertState State { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }

        public string DeviceSerialNumber { get; set; } = string.Empty!;
    }
}
