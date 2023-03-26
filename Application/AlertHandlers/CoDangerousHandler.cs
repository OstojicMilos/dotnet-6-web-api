using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Models;

namespace CleanArchitecture.Application.AlertHandlers
{
    public sealed class CoDangerousHandler : AlertHandler
    {
        protected override string GetAlertMessage(DeviceReading reading) =>
            $"Sensor {reading.DeviceSerialNumber} has reported dangerous CO level.";

        protected override AlertType GetAlertType() => AlertType.DangerousCo;

        protected override bool IsReadingNormal(DeviceReading reading) =>
            !IsCoLevelDangerous(reading);
        protected override decimal GetValue(DeviceReading reading) => reading.CarbonMonoxide;

        private bool IsCoLevelDangerous(DeviceReading reading) =>
            reading.CarbonMonoxide > settings.DANGEROUS_CO;
    }
}
