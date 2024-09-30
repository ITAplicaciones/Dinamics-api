namespace DataverseAPI.Configuration
{
    public class AzureAdOptions
    {
        public string Instance { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public string DataverseUrl { get; set; }
    }
}
