// <copyright file="FileDownloadDataMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.File.Mappers
{
    using System;
    using System.Collections.Generic;
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
        /// <inheritdoc />
        public override ExpressionCollection<FileDownloadData> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<FileDownloadData>(base.ExpressionCollection)
                {
                    { HeaderName.FileName, entity => entity.FileName },
                    { HeaderName.FileContentType, entity => entity.FileContentType },
                    { HeaderName.FilePath, entity => entity.FilePath },
                    { HeaderName.RequestDateTime, entity => entity.RequestDateTime },
                    { HeaderName.RequestByUser, entity => entity.RequestByUser.FirstName + " " + entity.RequestByUser.LastName },
                    { HeaderName.AvailabilityDuration, entity => entity.ExpiredAtDateTime },
                };
            }
        }

        /// <inheritdoc />
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
                AvailabilityDuration = entity.ExpiredAtDateTime.HasValue ? entity.ExpiredAtDateTime.Value - entity.RequestDateTime : null,
            });
        }

        /// <inheritdoc />
        public override void DtoToEntity(FileDownloadDataDto dto, ref FileDownloadData entity)
        {
            base.DtoToEntity(dto, ref entity);
            entity.FileName = dto.FileName;
            entity.FileContentType = dto.FileContentType;
            entity.FilePath = dto.FilePath;
            entity.RequestDateTime = dto.RequestDateTime;
            entity.RequestByUserId = dto.RequestByUser.Id;
            entity.ExpiredAtDateTime = dto.AvailabilityDuration.HasValue ? dto.RequestDateTime.Add(dto.AvailabilityDuration.Value) : null;
        }

        /// <inheritdoc />
        public override Dictionary<string, Func<string>> DtoToCellMapping(FileDownloadDataDto dto)
        {
            return new Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                { HeaderName.FileName, () => CSVString(dto.FileName) },
                { HeaderName.FileContentType, () => CSVString(dto.FileContentType) },
                { HeaderName.FilePath, () => CSVString(dto.FilePath) },
                { HeaderName.RequestDateTime, () => CSVDateTime(dto.RequestDateTime) },
                { HeaderName.RequestByUser, () => CSVString(dto.RequestByUser?.Display) },
                { HeaderName.AvailabilityDuration, () => CSVTime(dto.AvailabilityDuration) },
            };
        }

        /// <summary>
        /// Header Name.
        /// </summary>
        public struct HeaderName
        {
            /// <summary>
            /// Header Name FileName.
            /// </summary>
            public const string FileName = "fileName";

            /// <summary>
            /// Header Name FileContentType.
            /// </summary>
            public const string FileContentType = "fileContentType";

            /// <summary>
            /// Header Name FilePath.
            /// </summary>
            public const string FilePath = "filePath";

            /// <summary>
            /// Header Name RequestDateTime.
            /// </summary>
            public const string RequestDateTime = "requestDateTime";

            /// <summary>
            /// Header Name RequestByUser.
            /// </summary>
            public const string RequestByUser = "requestByUser";

            /// <summary>
            /// Header Name AvailabilityDuration.
            /// </summary>
            public const string AvailabilityDuration = "availabilityDuration";
        }
    }
}
