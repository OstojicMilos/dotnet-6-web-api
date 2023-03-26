using CleanArchitecture.Core.Enums;

namespace CleanArchitecture.Application.AlertHandlers
{
    public sealed class AlertHandlerFactory
    {
        public static Dictionary<AlertType, AlertHandler> GetHandlers()
        {
            return new Dictionary<AlertType, AlertHandler>
            {
                { AlertType.TempOutOfRange, new TempOutOfRangeHandler() },
                { AlertType.HumidityOutOfRange, new HumidityAlertHandler() },
                { AlertType.CoOutOfRange, new CoOutOfRangeHandler() },
                { AlertType.DangerousCo, new CoDangerousHandler() },
                { AlertType.PoorHealth, new DeviceHealthHandler() },
            };
        }
    }
}
