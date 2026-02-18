namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class FilesController : BiaControllerBase
    {
        private readonly IBiaFileDownloaderService biaFileDownloaderService;
        private readonly IBiaClaimsPrincipalService biaClaimsPrincipal;

        public FilesController(IBiaFileDownloaderService biaFileDownloaderService, IBiaClaimsPrincipalService biaClaimsPrincipal)
        {
            this.biaFileDownloaderService = biaFileDownloaderService;
            this.biaClaimsPrincipal = biaClaimsPrincipal;
        }

        [HttpGet("{guid}/[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Download([FromRoute] string guid, [FromQuery] string token)
        {
            var fileDownloadData = this.biaFileDownloaderService.GetFileDownloadData(Guid.Parse(guid), token);
            var fileContent = await System.IO.File.ReadAllBytesAsync(fileDownloadData.FilePath);
            return this.File(fileContent, fileDownloadData.FileContentType, fileDownloadData.FileName);
        }

        [HttpGet("{guid}/[action]")]
        public IActionResult GetDownloadToken([FromRoute] string guid)
        {
            var token = this.biaFileDownloaderService.GenerateDownloadToken(Guid.Parse(guid), this.biaClaimsPrincipal.GetUserId());
            return this.Ok(token);
        }
    }
}
