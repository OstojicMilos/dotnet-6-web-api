using CleanArchitecture.Application.AlertHandlers;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.IServices;
using CleanArchitecture.Core.IRepositories;
using CleanArchitecture.Core.Models;
using CleanArchitecture.Core.Settings;
using Mapster;

namespace CleanArchitecture.Application.Services
{
    public sealed class AlertsService : IAlertsService
    {
        private IAlertsRepository _alertsRepository;

        public AlertsService(IAlertsRepository alertsRepository)
        {
            _alertsRepository = alertsRepository;
        }

        public async Task<IEnumerable<AlertDto>> GetAlertsAsync(string serialNumber, AlertQuery query)
        {
            var alerts = await _alertsRepository.GetAsync(serialNumber, query);

            return alerts.AsQueryable().ProjectToType<AlertDto>();
        }

        public async Task AddOrUpdateAlertsAsync(IEnumerable<DeviceReading> readings, string serialNumber)
        {
            var preexistingAlerts = await _alertsRepository.GetActiveOrRecentlyClosedAsync(serialNumber);

            var allAlerts = HandleReadings(readings, preexistingAlerts);

            await _alertsRepository.AddRangeAsync(allAlerts.Where(alert => alert.AlertId == 0));
            _alertsRepository.UpdateRange(allAlerts.Where(alert => alert.AlertId != 0));

            await _alertsRepository.SaveChangesAsync();
        }

        private IEnumerable<Alert> HandleReadings(IEnumerable<DeviceReading> readings, IEnumerable<Alert> preexistingAlerts)
        {
            var handlers = AlertHandlerFactory.GetHandlers();

            foreach (var alert in preexistingAlerts) handlers[alert.Type].AddAlert(alert);

            return handlers.Values.SelectMany(handler => handler.HandleReadings(readings));
        }
    }
}
