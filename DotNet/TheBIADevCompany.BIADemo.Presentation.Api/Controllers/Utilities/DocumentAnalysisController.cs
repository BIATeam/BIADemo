using Microsoft.AspNetCore.Http;

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Utilities
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Utilities;
    using TheBIADevCompany.BIADemo.Domain.Utilities.DocumentAnalysis;

    public class DocumentAnalysisController : BiaControllerBase
    {
        private readonly IDocumentAnalysisService documentAnalysisService;

        public DocumentAnalysisController(IDocumentAnalysisService documentAnalysisService)
        {
            this.documentAnalysisService = documentAnalysisService;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(DocumentContent), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetContent(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return this.BadRequest("File cannot be empty.");
            }

            try
            {
                using var fileStream = file.OpenReadStream();
                var documentContent = this.documentAnalysisService.GetContent(file.FileName, file.ContentType, fileStream);
                return this.Ok(documentContent);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
