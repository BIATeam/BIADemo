// <copyright file="IViewAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.View
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.Dto.SiteView;
    using TheBIADevCompany.BIADemo.Domain.Dto.View;

    /// <summary>
    /// The interface defining the view application service.
    /// </summary>
    public interface IViewAppService
    {
        /// <summary>
        /// Adds the site view asynchronous.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>SiteViewDto.</returns>
        Task<SiteViewDto> AddSiteViewAsync(SiteViewDto dto);

        /// <summary>
        /// Adds the user view asynchronous.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>ViewDto.</returns>
        Task<ViewDto> AddUserViewAsync(ViewDto dto);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>List of ViewDto.</returns>
        Task<IEnumerable<ViewDto>> GetAllAsync();

        /// <summary>
        /// Removes the site view asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RemoveSiteViewAsync(int id);

        /// <summary>
        /// Removes the user view asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RemoveUserViewAsync(int id);

        /// <summary>
        /// Sets the default site view asynchronous.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SetDefaultSiteViewAsync(DefaultSiteViewDto dto);

        /// <summary>
        /// Sets the default user view asynchronous.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SetDefaultUserViewAsync(DefaultViewDto dto);

        /// <summary>
        /// Updates the view asynchronous.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>A ViewDto.</returns>
        Task<ViewDto> UpdateViewAsync(ViewDto dto);

        /// <summary>
        /// Assigns the view to site asynchronous.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task AssignViewToSiteAsync(AssignViewToSiteDto dto);
    }
}