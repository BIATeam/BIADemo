namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class FilesController : BiaControllerBase
    {
        private readonly IBiaFileDownloaderService biaFileDownloaderService;

        public FilesController(IBiaFileDownloaderService biaFileDownloaderService)
        {
            this.biaFileDownloaderService = biaFileDownloaderService;
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Download([FromQuery] string token)
        {
            var fileDownloadData = await this.biaFileDownloaderService.GetFileDownloadData(token);
            return this.File(fileDownloadData.FileContent, fileDownloadData.FileContentType, fileDownloadData.FileName);
        }

        [HttpGet("{guid}/[action]")]
        public async Task<IActionResult> GetDownloadToken([FromRoute] string guid)
        {
            return Ok();
        }
    }
}
