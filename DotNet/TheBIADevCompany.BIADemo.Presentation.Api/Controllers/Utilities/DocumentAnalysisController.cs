// BIADemo only
// <copyright file="DocumentAnalysisController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Utilities
{
    using System;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Utilities;
    using TheBIADevCompany.BIADemo.Domain.Utilities.DocumentAnalysis;

    /// <summary>
    /// Controller for document analysis.
    /// </summary>
    public class DocumentAnalysisController : BiaControllerBase
    {
        private readonly IDocumentAnalysisService documentAnalysisService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentAnalysisController"/> class.
        /// </summary>
        /// <param name="documentAnalysisService">Document analysis service.</param>
        public DocumentAnalysisController(IDocumentAnalysisService documentAnalysisService)
        {
            this.documentAnalysisService = documentAnalysisService;
        }

        /// <summary>
        /// Retrieve the content of a document.
        /// </summary>
        /// <param name="file"><see cref="IFormFile"/> of the document.</param>
        /// <returns>Document content.</returns>
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
