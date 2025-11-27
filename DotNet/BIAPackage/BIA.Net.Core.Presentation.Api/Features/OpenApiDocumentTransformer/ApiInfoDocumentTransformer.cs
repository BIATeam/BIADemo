// <copyright file="ApiInfoDocumentTransformer.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.Features.OpenApiDocumentTransformer
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.OpenApi;
    using Microsoft.OpenApi;

    /// <summary>
    /// Sets API Info (title, version) on the generated OpenAPI document.
    /// </summary>
    public sealed class ApiInfoDocumentTransformer : IOpenApiDocumentTransformer
    {
        /// <summary>
        /// Transforms the specified OpenAPI document.
        /// </summary>
        /// <param name="document">The <see cref="T:Microsoft.OpenApi.OpenApiDocument" /> to modify.</param>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.OpenApi.OpenApiDocumentTransformerContext" /> associated with the <see paramref="document" />.</param>
        /// <param name="cancellationToken">The cancellation token to use.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            document.Info = new OpenApiInfo
            {
                Title = "BIAApi",
                Version = "v1.0",
            };

            return Task.CompletedTask;
        }
    }
}
