// <copyright file="MembersController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
#define UseHubForClientInMember
#define UseHubForClientInUser

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
#if UseHubForClientInMember
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
#endif
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
#if UseHubForClientInMember
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Options;
#endif
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Error;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Base;

    /// <summary>
    /// The API controller used to manage members.
    /// </summary>
    public class MembersController : TeamLinkedControllerBase
    {
        /// <summary>
        /// The member application service.
        /// </summary>
        private readonly IMemberAppService memberService;

        /// <summary>
        /// The member application service.
        /// </summary>
        private readonly IUserAppService userService;

        /// <summary>
        /// The user context for message translation.
        /// </summary>
        private readonly UserContext userContext;

#if UseHubForClientInMember
        /// <summary>
        /// the client for hub (signalR) service.
        /// </summary>
        private readonly IClientForHubRepository clientForHubService;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="MembersController"/> class.
        /// </summary>
        /// <param name="userService">The user application service.</param>
        /// <param name="memberService">The member application service.</param>
        /// <param name="teamAppService">The team service.</param>
        /// <param name="userContext">The user context.</param>
        /// <param name="clientForHubService">The hub for client.</param>
#if UseHubForClientInMember
        public MembersController(
            IUserAppService userService,
            IMemberAppService memberService,
            ITeamAppService teamAppService,
            UserContext userContext,
            IClientForHubRepository clientForHubService)
#else
        public MembersController(
            IUserAppService userService,
            IMemberAppService memberService,
            ITeamAppService teamAppService)
#endif
            : base(teamAppService)
        {
#if UseHubForClientInMember
            this.clientForHubService = clientForHubService;
#endif
            this.memberService = memberService;
            this.userService = userService;
            this.userContext = userContext;
        }

        /// <summary>
        /// Get all members with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of members.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            if (filters.ParentIds != null && filters.ParentIds.Length > 0 && filters.ParentIds[0] != null)
            {
                if (!this.IsAuthorizeForTeam(int.Parse(filters.ParentIds[0]), Rights.Members.ListAccessSuffix).Result)
                {
                    return this.StatusCode(StatusCodes.Status403Forbidden);
                }
            }
            else
            {
                return this.StatusCode(StatusCodes.Status403Forbidden);
            }

            var (results, total) = await this.memberService.GetRangeByTeamAsync(filters);
            this.HttpContext.Response.Headers.Add(BIAConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Get a member by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The member.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.memberService.GetAsync(id);
                if (!this.IsAuthorizeForTeam(dto.TeamId, Rights.Members.ReadSuffix).Result)
                {
                    return this.StatusCode(StatusCodes.Status403Forbidden);
                }

                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Add a member.
        /// </summary>
        /// <param name="dto">The member DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status303SeeOther)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Add([FromBody] MemberDto dto)
        {
            try
            {
                if (!this.IsAuthorizeForTeam(dto.TeamId, Rights.Members.CreateSuffix).Result)
                {
                    return this.StatusCode(StatusCodes.Status403Forbidden);
                }

                // Specific Code to add user if required
                var addUserResult = await this.AddUserIfRequired(dto);
                if (addUserResult != null)
                {
                    return addUserResult;
                }

                var createdDto = await this.memberService.AddAsync(dto);
#if UseHubForClientInMember
                await this.clientForHubService.SendTargetedMessage(createdDto.TeamId.ToString(), "members", "refresh-members");
#endif
                return this.CreatedAtAction("Get", new { id = createdDto.Id }, createdDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
            catch (InvalidOperationException ex)
            {
                return this.UnprocessableEntity(ErrorMessage.GetMessage(ErrorId.MemberAlreadyExists, this.userContext.LanguageId));
            }
        }

        private async Task<IActionResult> AddUserIfRequired(MemberDto dto)
        {
            if (dto.User.DtoState == DtoState.AddedNewChoice)
            {
                var existingUser = await this.userService.GetUserInfoAsync(dto.User.Display);
                if (existingUser != null && existingUser.IsActive)
                {
                    dto.User.Id = existingUser.Id;
                    return null;
                }

                if (!this.IsAuthorize(Rights.Users.Add))
                {
                    return this.StatusCode(StatusCodes.Status403Forbidden);
                }

                UserDto userDto = new UserDto();
                userDto.Login = dto.User.Display;
                ResultAddUsersFromDirectoryDto result = await this.userService.AddByIdentityKeyAsync(userDto);
#if UseHubForClientInUser
                _ = this.clientForHubService.SendTargetedMessage(string.Empty, "users", "refresh-users");
#endif
                if (result.Errors.Any())
                {
                    return this.StatusCode(StatusCodes.Status303SeeOther, result.Errors);
                }

                dto.User = result.UsersAddedDtos.FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Add a member.
        /// </summary>
        /// <param name="dtos">The members DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost("addMulti")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddMulti([FromBody] MembersDto dtos)
        {
            try
            {
                if (!this.IsAuthorizeForTeam(dtos.TeamId, Rights.Members.CreateSuffix).Result)
                {
                    return this.StatusCode(StatusCodes.Status403Forbidden);
                }

                List<MemberDto> dtoList = new List<MemberDto>();
                foreach (var user in dtos.Users)
                {
                    MemberDto dto = new MemberDto
                    {
                        User = user,
                        Roles = dtos.Roles,
                        TeamId = dtos.TeamId,
                        DtoState = DtoState.Added,
                    };
                    dtoList.Add(dto);
                }

#pragma warning disable S1481 // Unused local variables should be removed
                var savedDtos = await this.memberService.SaveAsync(dtoList);
#pragma warning restore S1481 // Unused local variables should be removed
#if UseHubForClientInMember
                _ = this.clientForHubService.SendTargetedMessage(dtos.TeamId.ToString(), "members", "refresh-members");
#endif
                return this.Ok();
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
        }

        /// <summary>
        /// Update a member.
        /// </summary>
        /// <param name="id">The member identifier.</param>
        /// <param name="dto">The member DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(int id, [FromBody] MemberDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                if (!this.IsAuthorizeForTeam(dto.TeamId, Rights.Members.UpdateSuffix).Result)
                {
                    return this.StatusCode(StatusCodes.Status403Forbidden);
                }

                // Specific Code to add user if required
                var addUserResult = await this.AddUserIfRequired(dto);
                if (addUserResult != null)
                {
                    return addUserResult;
                }

                var updatedDto = await this.memberService.UpdateAsync(dto);
#if UseHubForClientInMember
                await this.clientForHubService.SendTargetedMessage(updatedDto.TeamId.ToString(), "members", "refresh-members");
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
        }

        /// <summary>
        /// Remove a member.
        /// </summary>
        /// <param name="id">The member identifier.</param>
        /// <returns>The result of the Remove.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var toDeleteDto = await this.memberService.GetAsync(id);
                if (!this.IsAuthorizeForTeam(toDeleteDto.TeamId, Rights.Members.DeleteSuffix).Result)
                {
                    return this.StatusCode(StatusCodes.Status403Forbidden);
                }

#pragma warning disable S1481 // Unused local variables should be removed
                var deletedDto = await this.memberService.RemoveAsync(id);
#pragma warning restore S1481 // Unused local variables should be removed
#if UseHubForClientInMember
                await this.clientForHubService.SendTargetedMessage(deletedDto.TeamId.ToString(), "members", "refresh-members");
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Removes the specified member ids.
        /// </summary>
        /// <param name="ids">The member ids.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Remove([FromQuery] List<int> ids)
        {
            if (ids?.Any() != true)
            {
                return this.BadRequest();
            }

            try
            {
#pragma warning disable S1481 // Unused local variables should be removed
                foreach (var id in ids)
                {
                    var toDeleteDto = await this.memberService.GetAsync(id);
                    if (!this.IsAuthorizeForTeam(toDeleteDto.TeamId, Rights.Members.DeleteSuffix).Result)
                    {
                        return this.StatusCode(StatusCodes.Status403Forbidden);
                    }
                }

                var deletedDtos = await this.memberService.RemoveAsync(ids);
#pragma warning restore S1481 // Unused local variables should be removed
#if UseHubForClientInMember
                deletedDtos.Select(m => m.TeamId).Distinct().ToList().ForEach(parentId =>
                {
                    _ = this.clientForHubService.SendTargetedMessage(parentId.ToString(), "members", "refresh-members");
                });
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Save all members according to their state (added, updated or removed).
        /// </summary>
        /// <param name="dtos">The list of members.</param>
        /// <returns>The status code.</returns>
        [HttpPost("save")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Save(IEnumerable<MemberDto> dtos)
        {
            var dtoList = dtos.ToList();
            if (!dtoList.Any())
            {
                return this.BadRequest();
            }

            try
            {
                foreach (var dto in dtos)
                {
                    if (!this.IsAuthorizeForTeam(dto.TeamId, Rights.Members.SaveSuffix).Result)
                    {
                        return this.StatusCode(StatusCodes.Status403Forbidden);
                    }
                }

#pragma warning disable S1481 // Unused local variables should be removed
                var savedDtos = await this.memberService.SaveAsync(dtoList);
#pragma warning restore S1481 // Unused local variables should be removed
#if UseHubForClientInMember
                savedDtos.Select(m => m.TeamId).Distinct().ToList().ForEach(parentId =>
                {
                    _ = this.clientForHubService.SendTargetedMessage(parentId.ToString(), "members", "refresh-members");
                });
#endif
                return this.Ok();
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Generates a csv file according to the filters.
        /// </summary>
        /// <param name="filters">filters ( <see cref="LazyLoadDto"/>).</param>
        /// <returns>a csv file.</returns>
        [HttpPost("csv")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public virtual async Task<IActionResult> GetFileCSV([FromBody] PagingFilterFormatDto filters)
        {
            if (!this.IsAuthorizeForTeam(int.Parse(filters.ParentIds[0]), Rights.Members.ListAccessSuffix).Result)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden);
            }

            var buffer = await this.memberService.ExportCSV(filters);
            string fileName = $"Members-{DateTime.Now:MM-dd-yyyy-HH-mm}{BIAConstants.Csv.Extension}";
            return this.File(buffer, "text/csv;charset=utf-8", fileName);
        }
    }
}