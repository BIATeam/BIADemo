// <copyright file="BIADtoFieldAttribute.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.CustomAttribute
{
    using System;

    /// <summary>
    /// The custom attibute class for DTO fields.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class BIADtoFieldAttribute : Attribute
    {
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

        /// <summary>
        /// Constructor.
        /// </summary>
        public BIADtoFieldAttribute(string type = null, bool required = false, bool isParent = false)
        {
            this.Type = type;
            this.Required = required;
            this.IsParent = isParent;
        }
    }
}
