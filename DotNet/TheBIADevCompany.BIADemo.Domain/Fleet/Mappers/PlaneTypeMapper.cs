// BIADemo only
// <copyright file="PlaneTypeMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Mappers
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Mapper;
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

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
                return new ExpressionCollection<PlaneType>(base.ExpressionCollection)
                {
                    { HeaderName.Title, planeType => planeType.Title },
                    { HeaderName.CertificationDate, planeType => planeType.CertificationDate },
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(PlaneTypeDto dto, ref PlaneType entity)
        {
            base.DtoToEntity(dto, ref entity);

            entity.Title = dto.Title;
            entity.CertificationDate = dto.CertificationDate;
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<PlaneType, PlaneTypeDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new PlaneTypeDto
            {
                Title = entity.Title,
                CertificationDate = entity.CertificationDate,
            });
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToCell"/>
        public override string DtoToCell(PlaneTypeDto dto, string headerName)
        {
            switch (headerName)
            {
                case HeaderName.Title:
                    return CSVString(dto.Title);
                case HeaderName.CertificationDate:
                    return CSVDateTime(dto.CertificationDate);
                default:
                    return base.DtoToCell(dto, headerName);
            }
        }

        /// <summary>
        /// Header names.
        /// </summary>
        public struct HeaderName
        {
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