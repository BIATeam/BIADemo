// <copyright file="HttpErrorReport.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api
{
    /// <summary>
    /// Represents a HTTP error report.
    /// </summary>
    /// <param name="ErrorCode">The error code.</param>
    /// <param name="ErrorMessage">The error message.</param>
    public record class HttpErrorReport(int ErrorCode,  string ErrorMessage)
    {
    }
}
