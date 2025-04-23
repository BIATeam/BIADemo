// <copyright file="BiaDtoClassAttribute.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.CustomAttribute
{
    using System;

    /// <summary>
    /// The custom attibute class for DTO class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class BiaDtoClassAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiaDtoClassAttribute"/> class.
        /// </summary>
        /// <param name="ancestorTeam">Specify the ancestor.</param>
        public BiaDtoClassAttribute(string ancestorTeam = null)
        {
            this.AncestorTeam = ancestorTeam;
        }

        /// <summary>
        /// The Dto field type.
        /// </summary>
        public string AncestorTeam { get; set; }
    }
}
