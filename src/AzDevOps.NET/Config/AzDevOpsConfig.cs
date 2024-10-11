namespace AzDevOps.NET.Config
{
    public class AzDevOpsConfig
    {
        public string personalAccessToken { get; set; }
        public string url { get; internal set; } = "https://dev.azure.com";
        public string version { get; internal set; } = "7.1";
        public string organization { get; internal set; }

        public AzDevOpsConfig(string personalAcessToken, string organization)
        {
            this.personalAccessToken = personalAcessToken;
            this.organization = organization;
        }

        public AzDevOpsConfig(string personalAcessToken, string version, string organization)
        {
            this.personalAccessToken = personalAcessToken;
            this.version = version;
            this.organization = organization;
        }

        public AzDevOpsConfig(string personalAcessToken, string url, string version, string organization)
        {
            this.personalAccessToken = personalAcessToken;
            this.url = url;
            this.version = version;
            this.organization = organization;
        }
    }
}
