// <copyright file="BiaDtoFieldAttribute.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.CustomAttribute
{
    using System;

    /// <summary>
    /// The custom attibute class for DTO fields.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class BiaDtoFieldAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiaDtoFieldAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="required">if set to <c>true</c> [required].</param>
        /// <param name="isParent">if set to <c>true</c> [is parent].</param>
        public BiaDtoFieldAttribute(string type = null, bool required = false, bool isParent = false)
        {
            this.Type = type;
            this.Required = required;
            this.IsParent = isParent;
        }

        /// <summary>
        /// The Dto field type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// "Is required" field value.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// "Block generation" field value.
        /// </summary>
        public bool IsParent { get; set; }
    }
}
