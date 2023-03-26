using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.Settings
{
    public sealed class AlertQuery
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 50;
        public AlertStateFilter AlertState { get; set; } = AlertStateFilter.New;
    }
}
