// <copyright file="IFileDownloadDataAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.File
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.File;
    using BIA.Net.Core.Domain.File.Entities;

    /// <summary>
    /// The interface for the file download data application service.
    /// </summary>
    public interface IFileDownloadDataAppService : ICrudAppServiceBase<FileDownloadDataDto, FileDownloadData, Guid, PagingFilterFormatDto>
    {
    }
}
