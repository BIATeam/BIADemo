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
    public class BIADtoFieldAttribute(string type = null, bool required = false) : Attribute
    {
        /// <summary>
        /// The Dto field type.
        /// </summary>
        public string Type { get; set; } = type;

        /// <summary>
        /// "Is required" field value.
        /// </summary>
        public bool Required { get; set; } = required;
    }
}
