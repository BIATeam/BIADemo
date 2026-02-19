// <copyright file="FileDownloadDataMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.File.Mappers
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain.Dto.File;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.File.Entities;
    using BIA.Net.Core.Domain.Mapper;

    /// <summary>
    /// The mapper used for file download data.
    /// </summary>
    public class FileDownloadDataMapper : BaseMapper<FileDownloadDataDto, FileDownloadData, Guid>
    {
        /// <summary>
        /// Create a file download data DTO from an entity.
        /// </summary>
        /// <returns>The file download data DTO.</returns>
        public override Expression<Func<FileDownloadData, FileDownloadDataDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new FileDownloadDataDto
            {
                FileName = entity.FileName,
                FileContentType = entity.FileContentType,
                FilePath = entity.FilePath,
                RequestDateTime = entity.RequestDateTime,
                RequestByUser = new OptionDto
                {
                    Id = entity.RequestByUser.Id,
                    Display = entity.RequestByUser.FirstName + " " + entity.RequestByUser.LastName,
                },
            });
        }

        /// <summary>
        /// Create a file download data entity from a DTO.
        /// </summary>
        /// <param name="dto">The DTO.</param>
        /// <param name="entity">The entity to update with the DTO values.</param>
        public override void DtoToEntity(FileDownloadDataDto dto, ref FileDownloadData entity)
        {
            entity ??= new FileDownloadData();
            entity.Id = dto.Id;
            entity.FileName = dto.FileName;
            entity.FileContentType = dto.FileContentType;
            entity.FilePath = dto.FilePath;
            entity.RequestDateTime = dto.RequestDateTime;
            entity.RequestByUserId = dto.RequestByUser.Id;
        }
    }
}
