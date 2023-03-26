using CleanArchitecture.Core.Models;
using CleanArchitecture.Core.Settings;

namespace CleanArchitecture.Core.IRepositories
{
    public interface IAlertsRepository : IRepository<Alert, int>
    {
        public Task<IEnumerable<Alert>> GetAsync(string serialNumber, AlertQuery query);
        public Task<IEnumerable<Alert>> GetActiveOrRecentlyClosedAsync(string serialNumber);
    }
}
