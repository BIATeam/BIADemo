// BIADemo only
// <copyright file="PlaneTypeMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Plane.Mappers
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;

    /// <summary>
    /// The mapper used for PlaneType.
    /// </summary>
    public class PlaneTypeMapper : BaseMapper<PlaneTypeDto, PlaneType, int>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<PlaneType> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<PlaneType>
                {
                    { HeaderName.Id, planeType => planeType.Id },
                    { HeaderName.Title, planeType => planeType.Title },
                    { HeaderName.CertificationDate, planeType => planeType.CertificationDate },
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(PlaneTypeDto dto, PlaneType entity)
        {
            entity ??= new PlaneType();
            entity.Id = dto.Id;
            entity.Title = dto.Title;
            entity.CertificationDate = dto.CertificationDate;
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<PlaneType, PlaneTypeDto>> EntityToDto()
        {
            return entity => new PlaneTypeDto
            {
                Id = entity.Id,
                Title = entity.Title,
                CertificationDate = entity.CertificationDate,
                RowVersion = Convert.ToBase64String(entity.RowVersion),
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<PlaneTypeDto, object[]> DtoToRecord(List<string> headerNames = null)
        {
            return x =>
            {
                List<object> records = [];

                if (headerNames != null && headerNames.Count > 0)
                {
                    foreach (string headerName in headerNames)
                    {
                        if (string.Equals(headerName, HeaderName.Id, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.Id));
                        }

                        if (string.Equals(headerName, HeaderName.Title, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.Title));
                        }

                        if (string.Equals(headerName, HeaderName.CertificationDate, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVDateTime(x.CertificationDate));
                        }
                    }
                }

                return [.. records];
            };
        }

        /// <summary>
        /// Header names.
        /// </summary>
        public struct HeaderName
        {
            /// <summary>
            /// Header name for id.
            /// </summary>
            public const string Id = "id";

            /// <summary>
            /// Header name for title.
            /// </summary>
            public const string Title = "title";

            /// <summary>
            /// Header name for certification date.
            /// </summary>
            public const string CertificationDate = "certificationDate";
        }
    }
}