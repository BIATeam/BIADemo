namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Example
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Notification;
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;

    public class FileDownloadsController : BiaControllerBase
    {
        private readonly IBiaFileDownloaderService fileDownloaderService;

        public FileDownloadsController(IBiaFileDownloaderService fileDownloaderService)
        {
            this.fileDownloaderService = fileDownloaderService;
        }

        [HttpPost("start")]
        public IActionResult Start()
        {
            this.fileDownloaderService.Start();
            return this.Ok();
        }
    }
}
