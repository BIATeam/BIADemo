﻿// <copyright file="ComparisonHelper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Comparison Helper.
    /// </summary>
    public static class ComparisonHelper
    {
        /// <summary>
        /// Ares the equal.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>Returns true if they are equal.</returns>
        public static bool AreEqual(string a, string b)
        {
            if (string.IsNullOrWhiteSpace(a))
            {
                return string.IsNullOrWhiteSpace(b);
            }
            else
            {
                return string.Equals(a, b);
            }
        }
    }
}
