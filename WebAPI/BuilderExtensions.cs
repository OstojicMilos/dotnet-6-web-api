using CleanArchitecture.Core.Settings;

namespace CleanArchitecture.WebAPI
{
    public static class BuilderExtensions
    {
        public static void SetSensoryRanges(this WebApplicationBuilder builder)
        {
            builder.Configuration.GetSection("SensorSettings")
                .Bind(SensorSettings.GetInstance());
        }
    }
}
