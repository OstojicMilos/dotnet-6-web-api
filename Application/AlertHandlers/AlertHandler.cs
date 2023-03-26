using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Models;
using CleanArchitecture.Core.Settings;

namespace CleanArchitecture.Application.AlertHandlers
{
    public abstract class AlertHandler
    {
        private List<Alert> _alerts = new List<Alert>();
        protected SensorSettings settings = SensorSettings.GetInstance();

        public void AddAlert(Alert alert) => _alerts.Add(alert);

        public IEnumerable<Alert> HandleReadings(IEnumerable<DeviceReading> readings)
        {
            foreach (var reading in readings) HandleReading(reading);

            return _alerts;
        }

        private void HandleReading(DeviceReading reading)
        {
            if (IsReadingNormal(reading))
            {
                ResolveLastAlertIfExists();
                return;
            }

            HandleDangerousReading(reading);
        }

        protected abstract bool IsReadingNormal(DeviceReading reading);

        private void ResolveLastAlertIfExists()
        {
            if (_alerts.Any()) _alerts.Last().State = AlertState.Resolved;
        }

        private void HandleDangerousReading(DeviceReading reading)
        {
            if (!_alerts.Any())
            {
                HandleFirstDangerousReading(reading);
                return;
            }

            HandleSubsequentDangerousReading(reading);
        }

        private void HandleFirstDangerousReading(DeviceReading reading)
        {
            AddNewAlert(reading);
        }

        private void HandleSubsequentDangerousReading(DeviceReading reading)
        {
            var lastAlert = _alerts.Last();
            if (IsAlertWithinMergeTime(lastAlert, reading))
            {
                UpdateAlertWithDangerousReading(lastAlert, reading);
            }
            else
            {
                lastAlert.State = AlertState.Resolved;
                AddNewAlert(reading);
            }
        }

        protected bool IsAlertWithinMergeTime(Alert alert, DeviceReading reading) =>
            alert.LastSensorReadingDate.AddMinutes(settings.ALERT_MERGE_TIME_IN_MINUTES) > reading.RecordedDateTime;

        private void UpdateAlertWithDangerousReading(Alert alert, DeviceReading reading)
        {
            alert.State = AlertState.New;
            alert.LastSensorReadingDate = reading.RecordedDateTime;

            var readingValue = GetValue(reading);
            alert.MinValue = Math.Min(alert.MinValue, readingValue);
            alert.MaxValue = Math.Max(alert.MaxValue, readingValue);

        }

        private void AddNewAlert(DeviceReading reading)
        {
            _alerts.Add(new Alert
            {
                DeviceSerialNumber = reading.DeviceSerialNumber,
                CreationDate = DateTimeOffset.UtcNow,
                FirstSensorReadingDate = reading.RecordedDateTime,
                LastSensorReadingDate = reading.RecordedDateTime,
                State = AlertState.New,
                Type = GetAlertType(),
                Message = GetAlertMessage(reading),
                MinValue = GetValue(reading),
                MaxValue = GetValue(reading)
            });
        }

        protected abstract AlertType GetAlertType();
        protected abstract string GetAlertMessage(DeviceReading reading);
        protected abstract decimal GetValue(DeviceReading reading);
    }
}
