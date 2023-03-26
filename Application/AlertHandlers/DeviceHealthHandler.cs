using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Models;

namespace CleanArchitecture.Application.AlertHandlers
{
    public sealed class DeviceHealthHandler : AlertHandler
    {
        protected override string GetAlertMessage(DeviceReading reading) =>
            $"Sensor {reading.DeviceSerialNumber} has reported device health issue ({reading.Health})";

        protected override AlertType GetAlertType() => AlertType.PoorHealth;

        protected override bool IsReadingNormal(DeviceReading reading) =>
            IsDeviceHealthy(reading);
        protected override decimal GetValue(DeviceReading reading) => 0;

        private bool IsDeviceHealthy(DeviceReading reading) => reading.Health == DeviceHealth.Ok;
    }
}
