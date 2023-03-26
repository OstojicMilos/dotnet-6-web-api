using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.IRepositories;
using CleanArchitecture.Core.Models;
using CleanArchitecture.Core.Settings;
using CleanArchitecture.DataAccess.Database;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.DataAccess.Repositories
{
    public sealed class AlertsRepository : Repository<Alert, int>, IAlertsRepository
    {
        public AlertsRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Alert>> GetAsync(string serialNumber, AlertQuery query)
        {
            var queryable = _context.Alerts.Where(alert => alert.DeviceSerialNumber == serialNumber);

            if (query.AlertState != AlertStateFilter.All)
                queryable = queryable.Where(alert => alert.State == (AlertState)query.AlertState);

            return await queryable.OrderByDescending(alert => alert.LastSensorReadingDate)
                                  .Skip((query.PageNumber - 1) * query.PageSize)
                                  .Take(query.PageSize)
                                  .ToListAsync();
        }

        public async Task<IEnumerable<Alert>> GetActiveOrRecentlyClosedAsync(string serialNumber)
        {
            var allowedTime = SensorSettings.GetInstance().ALERT_MERGE_TIME_IN_MINUTES;
            var utcTime = DateTimeOffset.UtcNow.AddMinutes(-allowedTime);

            return await _context.Alerts.Where(alert => alert.DeviceSerialNumber == serialNumber &&
                (alert.State == AlertState.New || alert.LastSensorReadingDate > utcTime))
                .ToListAsync();
        }
    }
}
