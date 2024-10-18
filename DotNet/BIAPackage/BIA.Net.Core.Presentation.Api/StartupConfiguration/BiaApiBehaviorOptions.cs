namespace BIA.Net.Core.Presentation.Api.StartupConfiguration
{
    using System;
    using System.Linq;
    using System.Text;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class BiaApiBehaviorOptions : IConfigureOptions<ApiBehaviorOptions>
    {
        private readonly ILogger<BiaApiBehaviorOptions> logger;

        public BiaApiBehaviorOptions(ILogger<BiaApiBehaviorOptions> logger)
        {
            this.logger = logger;
        }

        public void Configure(ApiBehaviorOptions options)
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errorBuilder = new StringBuilder();
                errorBuilder.Append($"HTTP {context.HttpContext.Request.Method} {context.HttpContext.Request.GetDisplayUrl()}: invalid model state: ");
                var validationErrors = context.ModelState.Where(m => m.Value.Errors.Count > 0);
                if (validationErrors.Count() == 1)
                {
                    errorBuilder.Append(string.Join(";", validationErrors.First().Value.Errors.Select(e => e.ErrorMessage)));
                }
                else
                {
                    errorBuilder.AppendLine();
                    foreach (var error in validationErrors)
                    {
                        errorBuilder.AppendLine($"\t{error.Key}: {string.Join(";", error.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }

                this.logger.LogError(errorBuilder.ToString());

                return new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState));
            };
        }
    }
}
