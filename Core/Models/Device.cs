using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.Models
{
    public sealed class Device
    {
        [Key]
        [MaxLength(32)]
        public string SerialNumber { get; set; } = string.Empty;

        [MaxLength(256)]
        public string SharedSecret { get; set; } = string.Empty;

        public string FirmwareVersion { get; set; } = string.Empty;

        public DateTimeOffset? FirstRegistrationDate { get; set; }

        public DateTimeOffset? LastRegistrationDate { get; set; }
    }
}
