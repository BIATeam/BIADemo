// <copyright file="IViewAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Bia.View
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.Dto.Bia.View;

    /// <summary>
    /// The interface defining the view application service.
    /// </summary>
    public interface IViewAppService
    {
        /// <summary>
        /// Adds the team view asynchronous.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>TeamViewDto.</returns>
        Task<TeamViewDto> AddTeamViewAsync(TeamViewDto dto);

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
        /// Removes the team view asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RemoveTeamViewAsync(int id);

        /// <summary>
        /// Removes the user view asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RemoveUserViewAsync(int id);

        /// <summary>
        /// Sets the default team view asynchronous.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SetDefaultTeamViewAsync(DefaultTeamViewDto dto);

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
        /// Assigns the view to team asynchronous.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task AssignViewToTeamAsync(AssignViewToTeamDto dto);
    }
}