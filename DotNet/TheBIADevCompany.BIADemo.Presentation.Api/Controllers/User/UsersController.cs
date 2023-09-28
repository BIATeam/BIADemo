// <copyright file="UsersController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

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
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="configuration">The configuration.</param>
        public UsersController(IUserAppService userService, IOptions<BiaNetSection> configuration)
        {
            this.userService = userService;
            this.configuration = configuration.Value;
        }

        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// <param name="filter">Used to filter on lastname, firstname or login.</param>
        /// <returns>The list of production sites.</returns>
        [HttpGet("allOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Users.Options)]
        public async Task<IActionResult> GetAllOptions(string filter = null)
        {
            var results = await this.userService.GetAllOptionsAsync(filter);
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
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.userService.GetRangeAsync<UserDto, UserMapper, PagingFilterFormatDto>(filters);
            this.HttpContext.Response.Headers.Add(BIAConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Gets all users in AD using the filter.
        /// </summary>
        /// <param name="filter">Used to filter on lastname, firstname or login.</param>
        /// <param name="ldapName">Name of the ldap to search in.</param>
        /// <param name="returnSize">The max number of items to return.</param>
        /// <returns>The list of users.</returns>
        [HttpGet("fromAD")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Rights.Users.ListAD)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllFromAD(string filter, string ldapName = null, int returnSize = 10)
        {
            if (filter.Contains('\n') || (ldapName != null && ldapName.Contains('\n')))
            {
                return this.BadRequest();
            }

            IEnumerable<UserFromDirectoryDto> results = default;

            if (this.configuration?.Authentication?.Keycloak?.IsActive == true)
            {
                results = await this.userService.GetAllIdpUserAsync(filter: filter, first: 0, max: returnSize);
            }
            else
            {
                results = await this.userService.GetAllADUserAsync(filter, ldapName, returnSize);
            }

            int resultCount = results.Count();

            this.HttpContext.Response.Headers.Add(BIAConstants.HttpHeaders.TotalCount, resultCount.ToString());

            return this.Ok(results);
        }

        /// <summary>
        /// Get a user by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The user.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Users.Read)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.userService.GetAsync<UserDto, UserMapper>(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Add some users in a group.
        /// </summary>
        /// <param name="users">The list of user.</param>
        /// <returns>The result code.</returns>
        [HttpPost("addFromDirectory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status303SeeOther)]
        [Authorize(Roles = Rights.Users.Add)]
        public async Task<IActionResult> Add([FromBody] IEnumerable<UserFromDirectoryDto> users)
        {
            ResultAddUsersFromDirectoryDto result = await this.userService.AddFromDirectory(users);
            if (result.Errors.Any())
            {
                return this.StatusCode(303, result.Errors);
            }

            return this.Ok(result.UsersAddedDtos);
        }

        /// <summary>
        /// Add some users in a group.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <param name="dto">The user with roles added or deleted.</param>
        /// <returns>The result code.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Users.UpdateRoles)]
        public async Task<IActionResult> Update(int id, [FromBody] UserDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.userService.UpdateAsync<UserDto, UserMapper>(dto, mapperMode: "Roles");
#if UseHubForClientInUser
                _ = this.clientForHubService.SendTargetedMessage(updatedDto.SiteId.ToString(), "users", "refresh-users");
#endif
                return this.Ok(updatedDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
            }
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
        //public async Task<IActionResult> AddInDB([FromBody]IEnumerable<UserFromDirectoryDto> users)
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
            if (ids == null || ids?.Any() != true)
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
            try
            {
                await this.userService.SynchronizeWithADAsync(fullSynchro);
            }
            catch (Exception)
            {
                return this.Problem("Error during synchronize. Retry Synchronize.");
            }

            return this.Ok();
        }

        /// <summary>
        /// Generates a csv file according to the filters.
        /// </summary>
        /// <param name="filters">filters ( <see cref="PagingFilterFormatDto"/>).</param>
        /// <returns>a csv file.</returns>
        [HttpPost("csv")]
        [Authorize(Roles = Rights.Users.ListAccess)]
        public virtual async Task<IActionResult> GetFile([FromBody] PagingFilterFormatDto filters)
        {
            byte[] buffer = await this.userService.GetCsvAsync(filters);
            return this.File(buffer, BIAConstants.Csv.ContentType + ";charset=utf-8", $"Planes{BIAConstants.Csv.Extension}");
        }
    }
}