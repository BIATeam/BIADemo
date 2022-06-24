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
        /// <summary>
        /// Header Name.
        /// </summary>
        public enum HeaderName
        {
            /// <summary>
            /// header name Id.
            /// </summary>
            Id,

            /// <summary>
            /// header name Title.
            /// </summary>
            Title,

            /// <summary>
            /// header name Certification Date.
            /// </summary>
            CertificationDate,
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<PlaneType> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<PlaneType>
                {
                    { HeaderName.Id.ToString(), planeType => planeType.Id },
                    { HeaderName.Title.ToString(), planeType => planeType.Title },
                    { HeaderName.CertificationDate.ToString(), planeType => planeType.CertificationDate },
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(PlaneTypeDto dto, PlaneType entity)
        {
            if (entity == null)
            {
                entity = new PlaneType();
            }

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
                List<object> records = new List<object>();

                if (headerNames?.Any() == true)
                {
                    foreach (string headerName in headerNames)
                    {
                        if (string.Equals(headerName, HeaderName.Title.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.Title));
                        }

                        if (string.Equals(headerName, HeaderName.CertificationDate.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVDateTime(x.CertificationDate));
                        }
                    }
                }

                return records.ToArray();
            };
        }
    }
}