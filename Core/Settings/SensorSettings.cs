namespace CleanArchitecture.Core.Settings
{
    public class SensorSettings
    {
        public decimal MIN_TEMP { get; set; } = -30;
        public decimal MAX_TEMP { get; set; } = 100;

        public decimal MIN_HUMIDITY { get; set; } = 0;
        public decimal MAX_HUMIDITY { get; set; } = 100;

        public decimal MIN_CO { get; set; } = 0;
        public decimal MAX_CO { get; set; } = 100;
        public decimal DANGEROUS_CO { get; set; } = 9;

        public int ALERT_MERGE_TIME_IN_MINUTES = 15;

        private static SensorSettings instance = null!;

        public static SensorSettings GetInstance() => instance ??= new SensorSettings();

        private SensorSettings() { }
    }
}
