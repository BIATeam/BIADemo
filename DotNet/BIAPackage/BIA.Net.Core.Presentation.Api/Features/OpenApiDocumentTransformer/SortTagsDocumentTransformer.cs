// <copyright file="SortTagsDocumentTransformer.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.Features.OpenApiDocumentTransformer
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.OpenApi;
    using Microsoft.OpenApi;

    /// <summary>
    /// Sorts the tags (controllers) alphabetically in the generated OpenAPI document.
    /// </summary>
    internal sealed class SortTagsDocumentTransformer : IOpenApiDocumentTransformer
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
            SwaggerControllerOrder<ControllerBase> swaggerControllerOrder = new SwaggerControllerOrder<ControllerBase>(Assembly.GetEntryAssembly());

            if (document.Tags != null && document.Tags.Count > 1)
            {
                // Convert the ordered enumerable to a List, then to a HashSet to match ISet<OpenApiTag>
                document.Tags = new HashSet<OpenApiTag>(document.Tags.OrderBy(t => swaggerControllerOrder.SortKey(t.Name)));
            }

            return Task.CompletedTask;
        }
    }
}
