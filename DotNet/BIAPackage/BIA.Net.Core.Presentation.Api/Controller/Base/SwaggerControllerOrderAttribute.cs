// <copyright file="SwaggerControllerOrderAttribute.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.Controller.Base
{
    using System;

    /// <summary>
    /// Annotates a controller with a Swagger sorting order that is used when generating.
    /// the Swagger documentation to order the controllers in a specific desired order.
    /// </summary>
    /// <remarks>
    /// Ref: http://blog.robiii.nl/2018/08/swashbuckle-custom-ordering-of.html modified for AddSwaggerGen() extension OrderActionsBy().
    /// https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md#change-operation-sort-order-eg-for-ui-sorting.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SwaggerControllerOrderAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerControllerOrderAttribute"/> class.
        /// </summary>
        /// <param name="order">Sets the sorting order of the controller.</param>
        public SwaggerControllerOrderAttribute(uint order)
        {
            this.Order = order;
        }

        /// <summary>
        /// Gets the sorting order of the controller.
        /// </summary>
        public uint Order { get; private set; }
    }
}
