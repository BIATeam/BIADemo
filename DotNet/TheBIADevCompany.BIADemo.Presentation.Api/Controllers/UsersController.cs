// <copyright file="UsersController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The API controller used to manage users.
    /// </summary>
    public class UsersController : BiaControllerBase
    {
        /// <summary>
        /// The service user.
        /// </summary>
        private readonly IUserAppService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public UsersController(IUserAppService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Gets all users using the filter.
        /// </summary>
        /// <param name="filter">Used to filter on lastname, firstname or login.</param>
        /// <returns>The list of users.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Rights.Users.List)]
        public async Task<IActionResult> GetAll(string filter)
        {
            var results = await this.userService.GetAllAsync(filter);
            return this.Ok(results);
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of user DTO.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Rights.Users.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] LazyLoadDto filters)
        {
            var (results, total) = await this.userService.GetAllAsync(filters);

            this.HttpContext.Response.Headers.Add(BIAConstants.HttpHeaders.TotalCount, total.ToString());

            return this.Ok(results);
        }

        /// <summary>
        /// Gets all users in AD using the filter.
        /// </summary>
        /// <param name="filter">Used to filter on lastname, firstname or login.</param>
        /// <param name="ldapName">Name of the ldap to search in.</param>
        /// <returns>The list of users.</returns>
        [HttpGet("fromAD")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Rights.Users.ListAD)]
        public async Task<IActionResult> GetAllFromAD(string filter, string ldapName = null)
        {
            var results = await this.userService.GetAllADUserAsync(filter, ldapName);

            this.HttpContext.Response.Headers.Add(BIAConstants.HttpHeaders.TotalCount, results.Count().ToString());

            return this.Ok(results);
        }

        /// <summary>
        /// Gets all users in AD using the filter.
        /// </summary>
        /// <param name="filter">Used to filter on lastname, firstname or login.</param>
        /// <returns>The list of users.</returns>
        [HttpGet("ldapDomains")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Rights.Users.ListAD)]
        public async Task<IActionResult> GetAllLdapUsersDomains()
        {
            var results = await this.userService.GetAllLdapUsersDomains();

            this.HttpContext.Response.Headers.Add(BIAConstants.HttpHeaders.TotalCount, results.Count.ToString());

            return this.Ok(results);
        }

        /// <summary>
        /// Add some users in a group.
        /// </summary>
        /// <param name="users">The list of user.</param>
        /// <returns>The result code.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status303SeeOther)]
        [Authorize(Roles = Rights.Users.Add)]
        public async Task<IActionResult> AddInGroup([FromBody] IEnumerable<UserFromDirectoryDto> users)
        {
            string errors = await this.userService.AddInGroupAsync(users);
            if (!string.IsNullOrEmpty(errors))
            {
                return this.StatusCode(303, errors);
            }

            return this.Ok();
        }

#pragma warning disable SA1005, S125
        ///// <summary>
        ///// Add some users in db.
        ///// </summary>
        ///// <param name="users">The list of user.</param>
        ///// <returns>The result code.</returns>
        //[HttpPost]

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[Authorize(Roles = Rights.Users.Add)]
        //public async Task<IActionResult> AddInDB([FromBody]IEnumerable<UserFromADDto> users)
        //{
        //    await this.userService.AddInDBAsync(users);
        //    return this.Ok();
        //}
#pragma warning restore SA1005, S125

        /// <summary>
        /// Remove some users in a group.
        /// </summary>
        /// <param name="id">The identifier of the user.</param>
        /// <returns>The result code.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Rights.Users.Delete)]
        public async Task<IActionResult> RemoveInGroup(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            var error = await this.userService.RemoveInGroupAsync(id);
            if (error != string.Empty)
            {
                return this.Problem(error);
            }

            return this.Ok();
        }

        /// <summary>
        /// Removes the specified user ids.
        /// </summary>
        /// <param name="ids">The identifiers of the user.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Users.Delete)]
        public async Task<IActionResult> Remove([FromQuery] List<int> ids)
        {
            if (ids?.Any() != true)
            {
                return this.BadRequest();
            }

            StringBuilder sb = new StringBuilder();

            foreach (int id in ids)
            {
                var error = await this.userService.RemoveInGroupAsync(id);
                if (error != string.Empty)
                {
                    sb.Append(error);
                }
            }

            var errors = sb.ToString();
            if (!string.IsNullOrEmpty(errors))
            {
                return this.Problem(errors);
            }

            return this.Ok();
        }

        /// <summary>
        /// Synchronize all the users with the AD.
        /// </summary>
        /// <param name="fullSynchro">If true resynchronize existing user.</param>
        /// <returns>The OK result.</returns>
        [HttpGet("synchronize")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Rights.Users.Sync)]
        public async Task<IActionResult> Synchronize(bool fullSynchro = false)
        {
            await this.userService.SynchronizeWithADAsync(fullSynchro);

            return this.Ok();
        }
    }
}