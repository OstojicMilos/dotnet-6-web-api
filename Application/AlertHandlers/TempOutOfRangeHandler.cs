using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Models;

namespace CleanArchitecture.Application.AlertHandlers
{
    public sealed class TempOutOfRangeHandler : AlertHandler
    {
        protected override string GetAlertMessage(DeviceReading reading) =>
            $"Sensor {reading.DeviceSerialNumber} has reported temperature out of range value.";

        protected override AlertType GetAlertType() => AlertType.TempOutOfRange;

        protected override bool IsReadingNormal(DeviceReading reading) => !IsTemperatureOutOfRange(reading);

        protected override decimal GetValue(DeviceReading reading) => reading.Temperature;

        private bool IsTemperatureOutOfRange(DeviceReading reading) =>
            reading.Temperature < settings.MIN_TEMP ||
            reading.Temperature > settings.MAX_TEMP;

    }
}
