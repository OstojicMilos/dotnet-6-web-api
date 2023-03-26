using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Models;
using CleanArchitecture.Core.Settings;

namespace CleanArchitecture.Application.AlertHandlers
{
    public sealed class HumidityAlertHandler : AlertHandler
    {
        protected override string GetAlertMessage(DeviceReading reading) =>
            $"Sensor {reading.DeviceSerialNumber} has reported humidity out of range value.";

        protected override AlertType GetAlertType() => AlertType.HumidityOutOfRange;
        protected override decimal GetValue(DeviceReading reading) => reading.Humidity;

        protected override bool IsReadingNormal(DeviceReading reading) =>
            !IsHumidityOutOfRange(reading);

        private bool IsHumidityOutOfRange(DeviceReading reading)
        {
            var settings = SensorSettings.GetInstance();
            return reading.Humidity < settings.MIN_HUMIDITY ||
                   reading.Humidity > settings.MAX_HUMIDITY;
        }
    }
}
