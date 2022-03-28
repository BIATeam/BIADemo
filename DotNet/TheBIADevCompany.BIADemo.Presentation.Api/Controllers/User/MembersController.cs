// <copyright file="MembersController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
// #define UseHubForClientInMember

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Base;
#if UseHubForClientInMember
    using BIA.Net.Core.Domain.RepoContract;
#endif
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
#if UseHubForClientInMember
    using Microsoft.AspNetCore.SignalR;
#endif
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The API controller used to manage members.
    /// </summary>
    public class MembersController : BiaControllerBase
    {
        /// <summary>
        /// The member application service.
        /// </summary>
        private readonly IMemberAppService memberService;

#if UseHubForClientInMember
        /// <summary>
        /// the client for hub (signalR) service.
        /// </summary>
        private readonly IClientForHubRepository clientForHubService;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="MembersController"/> class.
        /// </summary>
        /// <param name="memberService">The member application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
#if UseHubForClientInMember
        public MembersController(
            IMemberAppService memberService, IClientForHubRepository clientForHubService)
#else
        public MembersController(IMemberAppService memberService)
#endif
        {
#if UseHubForClientInMember
            this.clientForHubService = clientForHubService;
#endif
            this.memberService = memberService;
        }

        /// <summary>
        /// Get all members with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of members.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Members.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            try
            {
                var (results, total) = await this.memberService.GetRangeByTeamAsync(filters);
                this.HttpContext.Response.Headers.Add(BIAConstants.HttpHeaders.TotalCount, total.ToString());
                return this.Ok(results);
            }
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error ");
            }
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
        [Authorize(Roles = Rights.Members.Read)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.memberService.GetAsync(id);
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
        /// Add a member.
        /// </summary>
        /// <param name="dto">The member DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Members.Create)]
        public async Task<IActionResult> Add([FromBody] MemberDto dto)
        {
            try
            {
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
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
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
        [Authorize(Roles = Rights.Members.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] MemberDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
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
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
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
        [Authorize(Roles = Rights.Members.Delete)]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
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
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
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
        [Authorize(Roles = Rights.Members.Delete)]
        public async Task<IActionResult> Remove([FromQuery] List<int> ids)
        {
            if (ids?.Any() != true)
            {
                return this.BadRequest();
            }

            try
            {
#pragma warning disable S1481 // Unused local variables should be removed
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
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
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
        [Authorize(Roles = Rights.Members.Save)]
        public async Task<IActionResult> Save(IEnumerable<MemberDto> dtos)
        {
            var dtoList = dtos.ToList();
            if (!dtoList.Any())
            {
                return this.BadRequest();
            }

            try
            {
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
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Generates a csv file according to the filters.
        /// </summary>
        /// <param name="filters">filters ( <see cref="LazyLoadDto"/>).</param>
        /// <returns>a csv file.</returns>
        [HttpPost("csv")]
        [Authorize(Roles = Rights.Members.ListAccess)]
        public virtual async Task<IActionResult> GetFileCSV([FromBody] PagingFilterFormatDto filters)
        {
            var buffer = await this.memberService.ExportCSV(filters);
            string fileName = $"Members-{DateTime.Now:MM-dd-yyyy-HH-mm}{BIAConstants.Csv.Extension}";
            return this.File(buffer, "text/csv;charset=utf-8", fileName);
        }
    }
}