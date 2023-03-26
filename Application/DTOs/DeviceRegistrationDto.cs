namespace CleanArchitecture.Application.DTOs
{
    public sealed class DeviceRegistrationDto
    {
        public string SerialNumber { get; set; } = null!;
        public string SharedSecret { get; set; } = null!;
        public string FirmwareVersion { get; set; } = null!;
        public string TokenId { get; set; } = null!;
    }
}
