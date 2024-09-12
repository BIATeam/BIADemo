// <copyright file="INotificationDomainService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.NotificationModule.Service
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;

    /// <summary>
    /// The interface defining the notification domain service.
    /// </summary>
    public interface INotificationDomainService : IFilteredServiceBase<Notification, int>
    {
        /// <summary>
        /// Transform the source object into a Notification entity and add it to the DB.
        /// </summary>
        /// <typeparam name="TOtherObject">The type of the source object.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper transforming source object to Notification entity.</typeparam>
        /// <param name="dto">The source object.</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>The source object with id updated.</returns>
        new Task<TOtherObject> AddAsync<TOtherObject, TOtherMapper>(TOtherObject dto, string mapperMode = null)
            where TOtherObject : BaseDto<int>, new()
            where TOtherMapper : BaseMapper<TOtherObject, Notification, int>;
    }
}