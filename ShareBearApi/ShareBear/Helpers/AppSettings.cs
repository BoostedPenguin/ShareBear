namespace ShareBear.Helpers
{
    public class AppSettings
    {
        public string AzureStorageConnectionString { get; set; } = string.Empty;
        public string AzureStorageAccessKey { get; set; } = string.Empty;
        public string DebugAccessToken { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
    }
}
