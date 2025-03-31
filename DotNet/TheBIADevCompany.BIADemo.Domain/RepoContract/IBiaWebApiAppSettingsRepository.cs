namespace TheBIADevCompany.BIADemo.Domain.RepoContract
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Option;

    public interface IBiaWebApiAppSettingsRepository
    {
        Task<AppSettingsDto> GetAsync(string baseAddress, string url = "/api/AppSettings");
    }
}