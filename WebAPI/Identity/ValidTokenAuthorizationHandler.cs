using CleanArchitecture.Core.IRepositories;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace CleanArchitecture.WebAPI.Identity
{
    public class ValidTokenAuthorizationHandler : AuthorizationHandler<ValidTokenRequirement>
    {
        private readonly IDeviceRegistrationsRepository _registrationsRepository;

        public ValidTokenAuthorizationHandler(IDeviceRegistrationsRepository registrationsRepository)
        {
            _registrationsRepository = registrationsRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ValidTokenRequirement requirement)
        {
            var tokenId = context.User.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti)?.Value ?? string.Empty;
            var deviceSerialNumber = context.User.Identity?.Name ?? string.Empty;

            var isTokenValid = await _registrationsRepository.IsAccessAuthorizedAsync(deviceSerialNumber, tokenId);

            if (isTokenValid) context.Succeed(requirement);
        }
    }
}
