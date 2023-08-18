// BIADemo only
// <copyright file="PlaneTypeMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;

    /// <summary>
    /// The mapper used for plane.
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
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<PlaneTypeDto, object[]> DtoToRecord(List<string> headerNames = null)
        {
            return x =>
            {
                List<object> records = new();

                if (headerNames != null && headerNames?.Any() == true)
                {
                    foreach (string headerName in headerNames)
                    {
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

                return records.ToArray();
            };
        }

        /// <summary>
        /// Header Name.
        /// </summary>
        public struct HeaderName
        {
            /// <summary>
            /// header name Id.
            /// </summary>
            public const string Id = "id";

            /// <summary>
            /// header name Title.
            /// </summary>
            public const string Title = "title";

            /// <summary>
            /// header name Certification Date.
            /// </summary>
            public const string CertificationDate = "certificationDate";
        }
    }
}