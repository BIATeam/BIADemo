// <copyright file="IFileDownloadDataAppService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.File
{
    using System;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.File;
    using BIA.Net.Core.Domain.File.Entities;

    /// <summary>
    /// Interface for FileDownloadData application service.
    /// </summary>
    public interface IFileDownloadDataAppService : ICrudAppServiceBase<FileDownloadDataDto, FileDownloadData, Guid, PagingFilterFormatDto>
    {
    }
}
