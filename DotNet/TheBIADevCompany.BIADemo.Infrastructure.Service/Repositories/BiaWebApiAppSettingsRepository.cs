namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration.AuthenticationSection;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    public class BiaWebApiAppSettingsRepository : WebApiRepository, IBiaWebApiAppSettingsRepository
    {
        public BiaWebApiAppSettingsRepository(
            HttpClient httpClient,
            ILogger<BiaWebApiAppSettingsRepository> logger,
            IBiaDistributedCache distributedCache,
            AuthenticationConfiguration configurationSection = null)
            : base(httpClient, logger, distributedCache, configurationSection)
        {
        }

        public async Task<AppSettingsDto> GetAsync(string baseAddress, string url = "/api/AppSettings")
        {
            var result = await this.GetAsync<AppSettingsDto>($"{baseAddress}{url}");
            return result.Result;
        }
    }
}
