namespace BIA.Net.Core.Common.Configuration.BiaWebApi
{
    using BIA.Net.Core.Common.Configuration.AuthenticationSection;

    public class BiaWebApi
    {
        public string BaseAddress { get; set; }

        public CredentialSource CredentialSource { get; set; }
    }
}
