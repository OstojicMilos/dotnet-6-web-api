using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Models;

namespace CleanArchitecture.Application.AlertHandlers
{
    public sealed class CoOutOfRangeHandler : AlertHandler
    {
        protected override string GetAlertMessage(DeviceReading reading) =>
            $"Sensor {reading.DeviceSerialNumber} has reported CO out of range value.";

        protected override AlertType GetAlertType() => AlertType.CoOutOfRange;

        protected override bool IsReadingNormal(DeviceReading reading) =>
            !IsCoOutOfRange(reading);
        protected override decimal GetValue(DeviceReading reading) => reading.CarbonMonoxide;

        private bool IsCoOutOfRange(DeviceReading reading) =>
            reading.CarbonMonoxide < settings.MIN_CO ||
            reading.CarbonMonoxide > settings.MAX_CO;

    }
}
