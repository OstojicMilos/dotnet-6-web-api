using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.IServices;
using CleanArchitecture.Core.Settings;
using CleanArchitecture.WebAPI.Bindings;
using CleanArchitecture.WebAPI.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/devices")]
    [Authorize("DeviceIngestion")]
    public class DeviceIngestionController : ControllerBase
    {
        private const string FIRMWARE_VERSION_REGEX = "^(0|[1-9]\\d*)\\.(0|[1-9]\\d*)\\.(0|[1-9]\\d*)(?:-((?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\\.(?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\\+([0-9a-zA-Z-]+(?:\\.[0-9a-zA-Z-]+)*))?$";
        private const string FIRMWARE_ERROR_MESSAGE = "The firmware value does not match semantic versioning format.";

        private readonly IDevicesService _devicesService;
        private readonly JwtService _jwtService;
        private readonly ILogger<DeviceIngestionController> _logger;
        private readonly IAlertsService _alertsService;

        public DeviceIngestionController(
            IDevicesService devicesService,
            JwtService smartAcJwtService,
            ILogger<DeviceIngestionController> logger,
            IAlertsService alertsService)
        {
            _devicesService = devicesService;
            _jwtService = smartAcJwtService;
            _logger = logger;
            _alertsService = alertsService;
        }

        /// <summary>
        /// Allow smart ac devices to register themselves  
        /// and get a jwt token for subsequent operations
        /// </summary>
        /// <param name="serialNumber">Unique device identifier burned into ROM</param>
        /// <param name="sharedSecret">Unique device shareable secret burned into ROM</param>
        /// <param name="firmwareVersion">Device firmware version at the moment of registering</param>
        /// <returns>A jwt token</returns>
        /// <response code="400">If any of the required data is not pressent or is invalid.</response>
        /// <response code="401">If something is wrong on the information provided.</response>
        /// <response code="200">If the registration has sucesfully generated a new jwt token.</response>
        [HttpPost("{serialNumber}/registration")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterDevice(
            [Required][FromRoute] string serialNumber,
            [Required][FromHeader(Name = "x-device-shared-secret")] string sharedSecret,
            [Required][FromQuery][RegularExpression(FIRMWARE_VERSION_REGEX, ErrorMessage = FIRMWARE_ERROR_MESSAGE)] string firmwareVersion)
        {
            var (tokenId, jwtToken) = _jwtService.GenerateJwtFor(serialNumber, JwtService.JwtScopeDeviceIngestionService);

            var registrationDto = new DeviceRegistrationDto
            {
                SerialNumber = serialNumber,
                SharedSecret = sharedSecret,
                FirmwareVersion = firmwareVersion,
                TokenId = tokenId
            };
            await _devicesService.RegisterAsync(registrationDto);

            _logger.LogDebug(
                "A new registration record with tokenId \"{tokenId}\" has been created for the device \"{serialNumber}\"",
                serialNumber, tokenId);

            return Ok(jwtToken);
        }

        /// <summary>
        /// Allow smart ac devices to send sensor readings in batch
        /// 
        /// This will additionally trigger analysis over the sensor readings
        /// to generate alerts based on it
        /// </summary>
        /// <param name="serialNumber">Unique device identifier burned into ROM.</param>
        /// <param name="sensorReadings">Collection of sensor readings send by a device.</param>
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="202">If sensor readings has sucesfully accepted.</response>
        /// <returns>No Content.</returns>
        [HttpPost("readings/batch")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> AddSensorReadings(
            [ModelBinder(BinderType = typeof(DeviceInfoBinder))] string serialNumber,
            [FromBody] IEnumerable<DeviceReadingDto> sensorReadings)
        {
            await _devicesService.HandleReadingsAsync(sensorReadings, serialNumber);

            return Accepted();
        }

        /// <summary>
        /// Allow smart ac devices to read paginated and filtered alerts
        /// </summary>
        /// <param name="serialNumber">Unique device identifier burned into ROM.</param>
        /// <param name="query">Object used for filtering and paging.</param>
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="200">If alerts are returned.</response>
        /// <returns>List of filtered and paginated alerts.</returns>
        [HttpGet("{serialNumber}/alerts")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAlertsAsync(
            [ModelBinder(BinderType = typeof(DeviceInfoBinder))] string serialNumber,
            [FromQuery] AlertQuery query)
        {
            return Ok(await _alertsService.GetAlertsAsync(serialNumber, query));
        }
    }
}
