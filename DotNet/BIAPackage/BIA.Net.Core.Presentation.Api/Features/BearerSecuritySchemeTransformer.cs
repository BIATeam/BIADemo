// <copyright file="BearerSecuritySchemeTransformer.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.Features
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.OpenApi;
    using Microsoft.OpenApi;

    /// <summary>
    /// Bearer Security Scheme Transformer.
    /// </summary>
    internal sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
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
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

            string bearerScheme = "Bearer";

            document.Components.SecuritySchemes[bearerScheme] = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = bearerScheme.ToLower(),
            };

            // Iterate through each path & operation
            foreach (IOpenApiPathItem path in document.Paths.Values)
            {
                foreach (OpenApiOperation operation in path.Operations.Values)
                {
                    operation.Security ??= new List<OpenApiSecurityRequirement>();
                    operation.Security.Add(new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference(bearerScheme, document)] = [],
                    });
                }
            }

            return Task.CompletedTask;
        }
    }
}