// <copyright file="GenericLdapRepository.cs" company="BIA.Net">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.DirectoryServices;
    using System.DirectoryServices.AccountManagement;
    using System.DirectoryServices.ActiveDirectory;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Cryptography;
    using System.Security.Principal;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Ldap;
    using Meziantou.Framework.Win32;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Helper to get information from Ldap.
    /// </summary>
    abstract public class GenericLdapRepository<TUserFromDirectory> : IUserDirectoryRepository<TUserFromDirectory>
        where TUserFromDirectory : class, IUserFromDirectory, new()
    {

        private const string KeyPrefixCacheGroup = "BIAGroupSid:";

        private const string KeyPrefixCacheUserSid = "BIAUsersid:";

        private const string KeyPrefixCacheUserSidHistory = "BIAUsersidHistory:";

        private const string KeyPrefixCacheUserLogin = "BIAUserLogin:";

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<GenericLdapRepository<TUserFromDirectory>> logger;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly IEnumerable<LdapDomain> ldapDomains;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly IEnumerable<LdapDomain> ldapDomainsUsers;

        /// <summary>
        /// The sidResolver.
        /// </summary>
        private readonly LdapRepositoryHelper ldapRepositoryHelper;

        /// <summary>
        /// Duration of the cache for ldap Group Member List in ldap.
        /// </summary>
        private readonly int LdapCacheGroupDuration;

        /// <summary>
        /// Duration of the cache for user property in ldap.
        /// </summary>
        private readonly int LdapCacheUserDuration;


        /// <summary>
        /// Initializes a new instance of the <see cref="GenericLdapRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        public GenericLdapRepository(ILogger<GenericLdapRepository<TUserFromDirectory>> logger, IOptions<BiaNetSection> configuration, ILdapRepositoryHelper ldapRepositoryHelper)
        {
            this.logger = logger;
            this.configuration = configuration.Value;
            this.ldapRepositoryHelper = (LdapRepositoryHelper)ldapRepositoryHelper;

            this.ldapDomains = this.configuration.Authentication.LdapDomains;
            this.ldapDomainsUsers = this.ldapDomains?.Where(l => l.ContainsUser == true);
            this.LdapCacheGroupDuration = configuration.Value.Authentication.LdapCacheGroupDuration;
            this.LdapCacheUserDuration = configuration.Value.Authentication.LdapCacheUserDuration;
        }

        /// <summary>
        /// Return the value of the property.
        /// </summary>
        /// <typeparam name="T">type of the value</typeparam>
        /// <param name="property">the property.</param>
        /// <returns>the value of the property.</returns>
        protected static T LdapGetValue<T>(PropertyValueCollection property)
        {
            object value = null;
            foreach (object tmpValue in property)
            {
                value = tmpValue;
            }

            return (T)value;
        }

        /// <summary>
        /// Return the domain of the Entity
        /// </summary>
        /// <param name="entry">the entry.</param>
        /// <returns>the </returns>
        protected static string LdapGetDomainName(DirectoryEntry entry)
        {
            if (entry == null || entry.Parent == null)
            {
                return null;
            }

            using DirectoryEntry parent = entry.Parent;
            if (LdapGetValue<string>(parent.Properties["objectClass"]) == "domainDNS")
            {
                return LdapGetValue<string>(parent.Properties["dc"]);
            }
            else
            {
                return LdapGetDomainName(parent);
            }
        }

        /// <summary>
        /// Convert the Ad entry in a UserInfoDirectory Object.
        /// </summary>
        /// <param name="entry">Entry from AD.</param>
        /// <param name="domainKey">Domain Name in config file where domain found.</param>
        protected abstract TUserFromDirectory ConvertToUserDirectory(DirectoryEntry entry, string domainKey);


        /// <inheritdoc cref="IUserDirectoryRepository<TUserDirectory>.IsUserInGroup"/>
        private async Task<bool> IsUserSidInGroups(string sid, IEnumerable<LdapGroup> ldapGroups)
        {
            if (ldapGroups == null || ldapGroups.Count() == 0)
            {
                return false;
            }
            List<string> listUsersSid = new List<string>();
            List<string> listTreatedGroupSid = new List<string>();
            var getListTasks = new List<Task>();
            foreach (var ldapGroup in ldapGroups)
            {
                getListTasks.Add(GetAllUsersSidInLdapGroup(listUsersSid, listTreatedGroupSid, ldapGroup));
            }
            while (getListTasks.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(getListTasks);
                if (listUsersSid.Any(usid => usid == sid)) return true;
                getListTasks.Remove(finishedTask);
            }
            return false;
        }

        /// <inheritdoc cref="IUserDirectoryRepository<TUserDirectory>.SearchUsers"/>
        public List<TUserFromDirectory> SearchUsers(string search, string ldapName = null)
        {
            int max = 10;
            List<TUserFromDirectory> usersInfo = new List<TUserFromDirectory>();
            if (string.IsNullOrEmpty(search))
            {
                return usersInfo;
            }

            if (!string.IsNullOrEmpty(ldapName))
            {
                LdapDomain ldapDomain = ldapDomainsUsers.FirstOrDefault(e => e.LdapName == ldapName);
                if (ldapDomain != null)
                {
                    return SearchUsersInDomain(search, ldapDomain).Take(max).ToList();
                }
            }

            foreach (var ldapDomain in ldapDomainsUsers)
            {
                var results = SearchUsersInDomain(search, ldapDomain);
                if (results != null)
                {
                    usersInfo.AddRange(results.Take(max - usersInfo.Count()));

                    if (usersInfo.Count() >= max)
                    {
                        break;
                    }
                }
            }

            return usersInfo;
        }

        private IEnumerable<TUserFromDirectory> SearchUsersInDomain(string search, LdapDomain domain)
        {
            if (domain == null || string.IsNullOrEmpty(domain.LdapName))
            {
                return null;
            }

            List<DirectoryEntry> usersMatches = new List<DirectoryEntry>();
            try
            {
                if (PrepareCredential(domain))
                {
                    string ldapPath = $"LDAP://{domain.LdapName}";
                    if (!string.IsNullOrEmpty(domain.Filter))
                    {
                        ldapPath = $"LDAP://{domain.Filter}";
                    }

                    using var entry = new DirectoryEntry(ldapPath, domain.LdapServiceAccount, domain.LdapServicePass);
                    using var searcher = new DirectorySearcher(entry)
                    {
                        SearchScope = SearchScope.Subtree,
                        Filter = $"(&(objectCategory=person)(objectClass=user)(|(givenname=*{search}*)(sn=*{search}*)(SAMAccountName=*{search}*)))",
                        SizeLimit = 10
                    };
                    usersMatches.AddRange(searcher.FindAll().Cast<SearchResult>().Select(s => s.GetDirectoryEntry()).ToList());
                }

            }
            catch (Exception e)
            {
                this.logger.LogError("Could not join Domain :" + domain, e);
            }

            return usersMatches.Select((um) => ConvertToUserDirectory(um, domain.Name));
        }

        /// <inheritdoc cref="IUserDirectoryRepository<TUserDirectory>.AddUsersInGroup"/>
        public async Task<List<string>> AddUsersInGroup(IEnumerable<IUserFromDirectory> usersFromDirectory, string roleLabel)
        {
            List<string> listGroupCacheSidToRemove = new List<string>();
            List<string> errors = new List<string>();
            try
            {
                foreach (var user in usersFromDirectory)
                {
                    if (!AddUserInGroup(user, roleLabel, listGroupCacheSidToRemove))
                    {
                        errors.Add(user.Domain + "\\" + user.Login);
                    }
                }
                foreach (var cacheSidToRemove in listGroupCacheSidToRemove)
                {
                    await this.ldapRepositoryHelper.distributedCache.Remove(KeyPrefixCacheGroup + cacheSidToRemove);
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "An error occured while adding the users with GUID : " + string.Join(',', usersFromDirectory.Select(u => u.Domain + "\\" + u.Login + " (" + u.Guid + ")")));
            }
            return errors;
        }

        private bool AddUserInGroup(IUserFromDirectory user, string roleLabel, List<string> listGroupCacheSidToRemove)
        {

            try
            {
                PrincipalContext contextUser = PrepareDomainContext(user.Domain).Result;
                if (contextUser != null)
                {
                    UserPrincipal userToAdd = UserPrincipal.FindByIdentity(contextUser, IdentityType.Guid, user.Guid.ToString());

                    if (userToAdd != null)
                    {
                        this.logger.LogInformation("User GUID " + user.Guid.ToString() + " found  in domain " + user.Domain);
                        string domainWhereUserFound = user.Domain;

                        GroupPrincipal group = PrepareGroupOfRoleForUser(domainWhereUserFound, roleLabel);
                        if (group == null)
                        {
                            // TODO : just add in db IF mode fake
                            return false;
                        }
                        try
                        {
                            group.Members.Add(userToAdd);
                            group.Save();

                            if (!listGroupCacheSidToRemove.Contains(group.Sid.Value))
                            {
                                listGroupCacheSidToRemove.Add(group.Sid.Value);
                            }


                            return true;
                        }
                        catch (Exception e)
                        {
                            this.logger.LogError(e, "[AddUserInGroup] Error when adding user " + userToAdd.UserPrincipalName + " in group " + group.UserPrincipalName);
                            return false;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "[AddUserInGroup] user GUID " + user.Guid.ToString() + " problem with domain " + user.Domain);
            }

            return false;
        }

        private async Task<UserPrincipal> ResolveUserPrincipal(string domain, string login)
        {
            PrincipalContext contextUser = await PrepareDomainContext(domain);
            UserPrincipal userToAdd = null;

            if (contextUser != null)
            {
                userToAdd = UserPrincipal.FindByIdentity(contextUser, IdentityType.SamAccountName, login);
            }

            return userToAdd;
        }

        public async Task<TUserFromDirectory> ResolveUser(string domain, string login)
        {
            UserPrincipal userPrincipal = await ResolveUserPrincipal(domain, login);
            return GetUser(userPrincipal, domain);
        }

        private GroupPrincipal PrepareGroupOfRoleForUser(string domainWhereUserFound, string roleLabel)
        {
            var userLdapGroups = this.GetLdapGroupsForRole(roleLabel);
            if (userLdapGroups != null && userLdapGroups.Count() > 0)
            {
                LdapGroup ldapGroup;
                PrincipalContext context;
                ldapGroup = userLdapGroups.Where(g => g.AddUsersOfDomains.Any(d => d == domainWhereUserFound)).FirstOrDefault();
                if (ldapGroup == null)
                {
                    this.logger.LogError("[AddUserInGroup] LdapGroup not found for domain " + domainWhereUserFound + " of role " + roleLabel);
                    return null;
                }
                context = PrepareDomainContext(ldapGroup.Domain).Result;
                if (context != null)
                {
                    var group = GroupPrincipal.FindByIdentity(context, ldapGroup.LdapName);
                    if (group == null)
                    {
                        this.logger.LogError("[AddUserInGroup] Cannot find group " + ldapGroup.LdapName + " in domain " + ldapGroup.Domain);
                    }
                    return group;
                }
            }

            return null;
        }

        Dictionary<LdapDomain, PrincipalContext> PrincipalContextCache = new Dictionary<LdapDomain, PrincipalContext>();
        SemaphoreSlim mutex = new SemaphoreSlim(1);

        internal async Task<PrincipalContext> PrepareDomainContext(string domain)
        {

            LdapDomain adDomain = ldapDomains.Where(d => d.Name == domain).FirstOrDefault();
            PrincipalContext pc = await PrepareDomainContext(adDomain);
            return pc;
        }

        private async Task<PrincipalContext> PrepareDomainContext(LdapDomain domain)
        {
            await mutex.WaitAsync().ConfigureAwait(false);
            try
            {
                if (PrincipalContextCache.ContainsKey(domain))
                {
                    return PrincipalContextCache[domain];
                }
                if (PrepareCredential(domain))
                {
                    if (!string.IsNullOrEmpty(domain.LdapServiceAccount))
                    {
                        return new PrincipalContext(ContextType.Domain, domain.LdapName, domain.LdapServiceAccount, domain.LdapServicePass);
                    }
                    PrincipalContext pc;
                    //if (string.IsNullOrEmpty(domain.Filter))
                    {
                        pc = new PrincipalContext(ContextType.Domain, domain.LdapName);
                    }
                    /*else
                    {
                        pc = new PrincipalContext(ContextType.Domain, domain.LdapName, domain.Filter);
                    }*/
                    PrincipalContextCache.Add(domain, pc);
                    return pc;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error when PrepareDomainContext for domain :" + domain.LdapName);
            }
            finally
            {
                mutex.Release();
            }
            return null;
        }

        object syncPrepareCredential = new Object();
        /// <summary>
        /// Extract credential from vault if requiered
        /// </summary>
        /// <param name="domain"></param>
        /// <returns>true if ok</returns>
        private bool PrepareCredential(LdapDomain domain)
        {
            lock (syncPrepareCredential)
            {
                if (string.IsNullOrEmpty(domain.LdapServiceAccount) && !string.IsNullOrEmpty(domain.CredentialKeyInWindowsVault))
                {

                    try
                    {
                        var cred = CredentialManager.ReadCredential(applicationName: domain.CredentialKeyInWindowsVault);
                        if (cred != null)
                        {
                            domain.LdapServiceAccount = cred.UserName;
                            domain.LdapServicePass = cred.Password;
                            return true;
                        }
                        this.logger.LogError("[PrepareCredential] Credential " + domain.CredentialKeyInWindowsVault + " not found in Vault");
                        return false;
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError(ex, "[PrepareCredential] Error when search credential " + domain.CredentialKeyInWindowsVault);
                        return false;
                    }

                }
                return true;
            }
        }

        /// <inheritdoc cref="IUserDirectoryRepository<TUserDirectory>.RemoveUsersInGroup"/>
        public async Task<List<IUserFromDirectory>> RemoveUsersInGroup(List<IUserFromDirectory> usersFromRepositoryToRemove, string roleLabel)
        {
            List<string> listGroupCacheSidToRemove = new List<string>();

            List<IUserFromDirectory> notRemovedUser = new List<IUserFromDirectory>();

            try
            {

                foreach (var userToRemove in usersFromRepositoryToRemove)
                {
                    bool userRemoved = false;
                    foreach (var domain in ldapDomainsUsers)
                    {
                        PrincipalContext contextUser = await PrepareDomainContext(domain);
                        if (contextUser != null)
                        {
                            UserPrincipal userPrincipalToRemove = UserPrincipal.FindByIdentity(contextUser, IdentityType.SamAccountName, userToRemove.Login);

                            if (userPrincipalToRemove != null)
                            {
                                string domainWhereUserFound = domain.Name;
                                GroupPrincipal group = PrepareGroupOfRoleForUser(domainWhereUserFound, roleLabel);
                                if (group == null || !group.Members.Remove(userPrincipalToRemove/*context, IdentityType.Guid, userToRemove.Guid.ToString()*/))
                                {
                                }
                                else
                                {
                                    group.Save();
                                    userRemoved = true;
                                }
                                if (!listGroupCacheSidToRemove.Contains(group.Sid.Value))
                                {
                                    listGroupCacheSidToRemove.Add(group.Sid.Value);
                                }
                            }
                            this.logger.LogError("[RemoveUsersInGroup] user not find in all adDomains : ");
                        }
                    }
                    if (!userRemoved)
                    {
                        notRemovedUser.Add(userToRemove);
                        this.logger.LogError("[RemoveUsersInGroup] user not find in all adDomains : " + userToRemove.Login);
                    }
                }
                foreach (var cacheSidToRemove in listGroupCacheSidToRemove)
                {
                    await this.ldapRepositoryHelper.distributedCache.Remove(KeyPrefixCacheGroup + cacheSidToRemove);
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "An error occured while adding the user with GUID : " + string.Join(',', usersFromRepositoryToRemove.Select(u => u.Domain + "\\" + u.Login + " (" + u.Guid + ")")));
            }

            return notRemovedUser;
        }


        /// <summary>
        /// Check if a user is in a group.
        /// </summary>
        /// <param name="ldapGroups">The groups to search in.</param>
        /// <param name="login">The user login.</param>
        /// <returns>A boolean indicating whether the user is in the group.</returns>
        public async Task<bool> IsSidInGroups(IEnumerable<LdapGroup> ldapGroups, string sid)
        {
            return await IsUserSidInGroups(sid, ldapGroups);
        }

        /// <inheritdoc cref="IUserDirectoryRepository<TUserDirectory>.GetAllUsersInGroup"/>
        public async Task<IEnumerable<string>> GetAllUsersSidInRoleToSync(string role)
        {
            List<LdapGroup> userLdapGroups = this.GetLdapGroupsForRole(role);
            if (userLdapGroups.Count == 0)
            {
                // no ldap group defined
                return null;
            }
            List<string> listUsersSid = new List<string>();
            if (userLdapGroups == null || userLdapGroups.Count() == 0)
            {
                return listUsersSid;
            }

            List<string> listTreatedGroupSid = new List<string>();
            var resolveTasks = new List<Task>();
            foreach (var ldapGroup in userLdapGroups)
            {
                resolveTasks.Add(GetAllUsersSidInLdapGroup(listUsersSid, listTreatedGroupSid, ldapGroup));
            }
            await Task.WhenAll(resolveTasks);
            return listUsersSid;
        }

        /// <inheritdoc cref="IUserDirectoryRepository<TUserDirectory>.GetLdapGroupsForRole"/>
        public List<LdapGroup> GetLdapGroupsForRole(string roleLabel)
        {
            return this.configuration.Roles.Where(w => (w.Type == BIAConstants.RoleType.Ldap || w.Type == BIAConstants.RoleType.Synchro) && w.Label == roleLabel).Select(r => r.LdapGroups).SelectMany(x => x).ToList();
        }

        private async Task GetAllUsersSidInLdapGroup(List<string> listUsersSid, List<string> listTreatedGroupSid, LdapGroup ldapGroup)
        {
            DateTime start = DateTime.Now;
            this.logger.LogDebug("GetAllUsersSidInLdapGroup {0} Start -----------------------------------------------------------------------------------------", ldapGroup.LdapName);

            GroupPrincipal groupPrincipal = null;
            try
            {
                PrincipalContext ctx = PrepareDomainContext(ldapGroup.Domain).Result;
                if (ctx != null)
                {
                    groupPrincipal = GroupPrincipal.FindByIdentity(ctx, ldapGroup.LdapName);
                }
            }
            catch (Exception e)
            {
                this.logger.LogWarning(e, "Could not join Domain :" + ldapGroup.Domain);
            }
            if (groupPrincipal != null)
            {
                this.logger.LogInformation("Group " + ldapGroup.LdapName + " found in domain " + ldapGroup.Domain);

                await this.GetAllUsersSidFromGroupRecursivelyAsync( new GroupDomainSid() { Sid = groupPrincipal.Sid.Value, Domain = ldapGroup.Domain }, ldapGroup, listUsersSid, listTreatedGroupSid);
            }
            this.logger.LogDebug("GetAllUsersSidInLdapGroup {0} Finish : {1} ms -------------------------------------------------------------------------------", ldapGroup.LdapName, (DateTime.Now - start).TotalMilliseconds);
        }

        /// <summary>
        /// Get all user from a group.
        /// </summary>
        /// <param name="groupPrincipalSid">The group sid to search in.</param>
        /// <param name="rootLdapGroup">the root ldapGroup to limite the scope of the search (ldap users and ldap for groups).</param>
        /// <param name="listUsers">The users found.</param>
        /// <param name="listTreatedGroups">The group already treated.</param>
        private async Task GetAllUsersSidFromGroupRecursivelyAsync(GroupDomainSid groupSid, LdapGroup rootLdapGroup, List<string> listUsersSid, List<string> listTreatedGroupSid)
        {
            SidResolvedGroup resolvedGroup = await ResolveGroupMember(groupSid, rootLdapGroup);
            if (resolvedGroup != null)
            {
                foreach (string sid in resolvedGroup.MembersUserSid)
                {
                    if (!listUsersSid.Contains(sid))
                    {
                        listUsersSid.Add(sid);
                    }
                }

                var resolveTasks = new List<Task>();
                foreach (GroupDomainSid memberGroupSid in resolvedGroup.MembersGroupSid)
                {
                    if (listTreatedGroupSid.Contains(memberGroupSid.Sid))
                    {
                        continue;
                    }
                    listTreatedGroupSid.Add(memberGroupSid.Sid);
                    
                    resolveTasks.Add(GetAllUsersSidFromGroupRecursivelyAsync(memberGroupSid, rootLdapGroup, listUsersSid, listTreatedGroupSid));
                }
                await Task.WhenAll(resolveTasks);
            }
        }

        /// <summary>
        /// Get a user directory from a user principal.
        /// </summary>
        /// <param name="userPrincipal">The user principal.</param>
        /// <param name="domainKey">The domain Key (in config file).</param>
        /// <returns>The user AD.</returns>
        internal TUserFromDirectory GetUser(UserPrincipal userPrincipal, string domainKey)
        {
            return !(userPrincipal.GetUnderlyingObject() is DirectoryEntry entry) ? null : ConvertToUserDirectory(entry, domainKey);
        }

        /// <summary>
        /// Return the role from Ad and fake
        /// </summary>
        /// <param name="login">le login of the user</param>
        /// <param name="sid">the sid of the user</param>
        /// <param name="domain">domain of the user</param>
        /// <returns>list of roles</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<string>> GetUserRolesBySid(bool isUserInDB, string sid, string domain)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var rolesSection = this.configuration.Roles;

            var adRoles = new List<string>();

            Parallel.ForEach(rolesSection, role =>
            {
                switch (role.Type)
                {
                    case "Fake":
                        adRoles.Add(role.Label);
                        break;
                    case BIAConstants.RoleType.UserInDB:
                        if (isUserInDB)
                        {
                            adRoles.Add(role.Label);
                        }

                        break;
                    case "Ldap":
                    case "LdapWithSidHistory":
                        if (IsSidInGroups(role.LdapGroups, sid).Result)
                        {
                            adRoles.Add(role.Label);
                        }
                        else if (role.Type.Equals("LdapWithSidHistory"))
                        {
                            string sidHistory = GetSidHistory(sid, domain).Result;
                            if (!string.IsNullOrEmpty(sidHistory))
                            {
                                if (IsSidInGroups(role.LdapGroups, sidHistory).Result)
                                {
                                    adRoles.Add(role.Label);
                                }
                            }
                        }
                        break;
                }
            });

            return adRoles;
        }



        private async Task<string> GetSidHistory(string sid, string userDomain)
        {
            string sidHistory = (string)await this.ldapRepositoryHelper.localCache.Get(KeyPrefixCacheUserSidHistory + sid);
            if (sidHistory != null)
            {
                return sidHistory;
            }
            PrincipalContext searchContext = PrepareDomainContext(userDomain).Result;
            UserPrincipal user = UserPrincipal.FindByIdentity(searchContext, IdentityType.Sid, sid);
            DirectoryEntry up_de = (DirectoryEntry)user?.GetUnderlyingObject();
            if (up_de != null)
            {
                up_de.RefreshCache(new[] { "sIDHistory" });
                byte[] sIDHistory = up_de.Properties["sIDHistory"]?.Value as byte[];
                if (sIDHistory != null)
                {
                    var securityIdentifier = new System.Security.Principal.SecurityIdentifier((byte[])sIDHistory, 0);
                    sidHistory = securityIdentifier.ToString();
                }
            }
            await this.ldapRepositoryHelper.localCache.Add(KeyPrefixCacheUserSidHistory + sid, sidHistory, this.LdapCacheUserDuration);
            return sidHistory;
        }

        private async Task<SidResolvedGroup> ResolveGroupMember(GroupDomainSid groupDomainSid, LdapGroup rootLdapGroup)
        {
            SidResolvedGroup itemResolve;
            itemResolve = (SidResolvedGroup)await this.ldapRepositoryHelper.distributedCache.Get(KeyPrefixCacheGroup + groupDomainSid.Sid);
            if (itemResolve != null)
            {
                return itemResolve;
            }

            DateTime start = DateTime.Now;
            string groupName = "Name not found : " + groupDomainSid.Sid;

            bool ContainsOnlyUsers = false;
            bool IgnoreForeignSecurityPrincipal = false;
            if (rootLdapGroup.RecursiveGroupsOfDomains == null || rootLdapGroup.RecursiveGroupsOfDomains.Length == 0)
            {
                rootLdapGroup.RecursiveGroupsOfDomains = new string[] { rootLdapGroup.Domain };
                ContainsOnlyUsers = true;
                IgnoreForeignSecurityPrincipal = true;
            }
            else if (rootLdapGroup.RecursiveGroupsOfDomains.Count() == 1 && rootLdapGroup.RecursiveGroupsOfDomains[0] == rootLdapGroup.Domain)
            {
                IgnoreForeignSecurityPrincipal = true;
            }



            DomainGroupPrincipal subGroupPrincipal = await ResolveGroupPrincipal(new string[] { groupDomainSid.Domain }, groupDomainSid.Sid);

            if (subGroupPrincipal.groupPrincipal != null)
            {
                groupName = subGroupPrincipal.groupPrincipal.Name;

                this.logger.LogDebug("ResolveGroupMember {0} => {1}\\{2} Member to solve : {3} ms", groupDomainSid.Sid, subGroupPrincipal.domain, groupName, (DateTime.Now - start).TotalMilliseconds);
                start = DateTime.Now;

                ConcurrentBag<GroupDomainSid> MembersGroupSid = new ConcurrentBag<GroupDomainSid>();
                ConcurrentBag<string> MembersUserSid = new ConcurrentBag<string>();

                DirectoryEntry de = (DirectoryEntry)subGroupPrincipal.groupPrincipal.GetUnderlyingObject();

                List<string> listSDN = new List<string>();
                foreach (string sDN in de.Properties["member"])
                {
                    listSDN.Add(sDN);
                }

                this.logger.LogDebug("ResolveGroupMember {0} => {1}\\{2} Member resolve : {3} ms", groupDomainSid.Sid, subGroupPrincipal.domain, groupName, (DateTime.Now - start).TotalMilliseconds);
                start = DateTime.Now;

                // do not parrallelize else to much ldap request and risque of reject by ad.
                foreach (var sDN in listSDN)
                //Parallel.ForEach(listSDN, sDN =>
                {
                    bool isForeignSecurity = sDN.Contains("ForeignSecurityPrincipals");
                    bool isUser = true;
                    bool isGroup = false;

                    string memberSid = null;
                    GroupDomainSid memberGroupSid = null;

                    if (isForeignSecurity)
                    {
                        if (!IgnoreForeignSecurityPrincipal)
                        {

                            string pattern = @"S-\d-\d-\d+-\d+-\d+-\d+-\w+";
                            foreach (Match match in Regex.Matches(sDN, pattern))
                            {
                                if (match.Success && match.Groups.Count > 0)
                                {
                                    memberSid = match.Groups[0].Value;
                                    break;
                                }
                            }
                            if (memberSid != null)
                            {
                                if (!ContainsOnlyUsers)
                                {
                                    this.logger.LogDebug("ResolveGroupMember test : {0}", memberSid);
                                    // Method for indeterminate group or user slower but work always.
                                    memberGroupSid = TestIfIsGroup(memberSid, rootLdapGroup.RecursiveGroupsOfDomains, subGroupPrincipal.domain, isForeignSecurity );
                                    if (memberGroupSid != null)
                                    {
                                        isGroup = true;
                                    }
                                    isUser = !isGroup;
                                }
                            }
                        }
                    }
                    else
                    {
                        DomainDirectoryEntry domainDirectoryEntry = GetDirectoryEntry(sDN);
                        if (domainDirectoryEntry.de != null)
                        {

                            memberSid = new SecurityIdentifier((byte[])domainDirectoryEntry.de.Properties["objectSid"].Value, 0).Value;

                            if (!ContainsOnlyUsers)
                            {
                                var objectClass = domainDirectoryEntry.de.Properties["objectClass"];

                                // For group check
                                isGroup = objectClass?.Contains("group") == true;
                                // For user check
                                isUser = objectClass?.Contains("user") == true;
                                if (isGroup)
                                {
                                    memberGroupSid = new GroupDomainSid() { Sid = memberSid, Domain = domainDirectoryEntry.domain.Name };
                                }
                                else if ((!isGroup && !isUser) || (isGroup && isUser))
                                {
                                    this.logger.LogDebug("ResolveGroupMember test : {0}", memberSid);
                                    // Method for indeterminate group or user slower but work always.
                                    memberGroupSid = TestIfIsGroup(memberSid, rootLdapGroup.RecursiveGroupsOfDomains, subGroupPrincipal.domain, isForeignSecurity);
                                    if (memberGroupSid != null)
                                    {
                                        isGroup = true;
                                    }
                                    isUser = !isGroup;
                                }
                            }
                        }
                    }

                    if (memberSid != null)
                    { 
                        if (isUser)
                        {
                            if (!MembersUserSid.Contains(memberSid))
                            {
                                MembersUserSid.Add(memberSid);
                            }
                        }
                        else if (isGroup)
                        {
                            if (!MembersGroupSid.Any(m => m.Sid == memberGroupSid.Sid))
                            {
                                MembersGroupSid.Add(memberGroupSid);
                            }
                        }
                    }
                }
                //);

                itemResolve = new SidResolvedGroup() { domainKey = subGroupPrincipal.domain, MembersGroupSid = MembersGroupSid.ToList(), MembersUserSid = MembersUserSid.ToList(), type = SidResolvedItemType.Group };
                await this.ldapRepositoryHelper.distributedCache.Add(KeyPrefixCacheGroup + groupDomainSid.Sid, itemResolve, this.LdapCacheGroupDuration);

                this.logger.LogDebug("ResolveGroupMember {0} => {1}\\{2} Decripted with DirectoryEntry ({3} groups + {4} users) : {5} ms", groupDomainSid.Sid, subGroupPrincipal.domain, groupName, MembersGroupSid.Count, MembersUserSid.Count, (DateTime.Now - start).TotalMilliseconds);

                return itemResolve;
            }
            else
            {
                DateTime end = DateTime.Now;

                TimeSpan ts = (end - start);
                this.logger.LogDebug("ResolveGroupMember {0} not found in {1} : {2} ms", groupDomainSid.Sid, subGroupPrincipal.domain, ts.TotalMilliseconds);
                start = DateTime.Now;
            }

            // TODO ad cache unresolve item to reduce try

            this.logger.LogDebug("ResolveGroupMember {0} : {1} ms", groupName, (DateTime.Now - start).TotalMilliseconds);

            return null;
        }

        private GroupDomainSid TestIfIsGroup(string memberSid, string[] recursiveGroupsOfDomains, string currentDomain, bool isForeignSecurity)
        {
            GroupDomainSid memberGroupSid = null;
            DomainGroupPrincipal testIsGroup = new DomainGroupPrincipal() { domain = null, groupPrincipal = null };
            if (isForeignSecurity)
            {
                testIsGroup = ResolveGroupPrincipal(recursiveGroupsOfDomains.Where((val, idx) => val != currentDomain).ToArray(), memberSid).Result;
            }
            else
            {
                testIsGroup = ResolveGroupPrincipal(new string[] { currentDomain }, memberSid).Result;
            }
            if (testIsGroup.groupPrincipal != null)
            {
                memberGroupSid = new GroupDomainSid() { Sid = memberSid, Domain = testIsGroup.domain };
            }

            return memberGroupSid;
        }

        struct DomainDirectoryEntry
        {
            public LdapDomain domain;
            public DirectoryEntry de;
        }

        private DomainDirectoryEntry GetDirectoryEntry(string sDN)
        {
            DomainDirectoryEntry domainDirectoryEntry = new DomainDirectoryEntry() { domain = null, de = null };
            LdapDomain adDomain = ldapDomains.Where(d => d.LdapName.Split('.').All( subD => sDN.Contains("DC="+subD))).FirstOrDefault();
            if (adDomain== null)
            {
                return domainDirectoryEntry;
            }

            domainDirectoryEntry.domain = adDomain;
            if (PrepareCredential(adDomain))
            {
                string ldapPath = $"LDAP://{adDomain.LdapName}/" + sDN;
                /*if (!string.IsNullOrEmpty(adDomain.Filter))
                {
                    ldapPath = $"LDAP://{adDomain.Filter}";
                }*/

                if (!string.IsNullOrEmpty(adDomain.LdapServiceAccount))
                {
                    domainDirectoryEntry.de = new DirectoryEntry(ldapPath, adDomain.LdapServiceAccount, adDomain.LdapServicePass);
                }
                else
                {
                    domainDirectoryEntry.de = new DirectoryEntry(ldapPath);
                }
            }

            return domainDirectoryEntry;
        }

        struct DomainGroupPrincipal
        {
            public string domain;
            public GroupPrincipal groupPrincipal;
        }


        Dictionary<string, DomainGroupPrincipal> cacheGroupPrincipal = new Dictionary<string, DomainGroupPrincipal>();
        object syncLocalGroupPrincipal = new Object();
        private async Task<DomainGroupPrincipal> ResolveGroupPrincipal(string[] groupDomains, string sid)
        {
            DomainGroupPrincipal domainGroupPrincipal = new DomainGroupPrincipal() { domain = null, groupPrincipal = null };
            lock (syncLocalGroupPrincipal)
            {
                if (cacheGroupPrincipal.TryGetValue(sid, out domainGroupPrincipal))
                {
                    this.logger.LogDebug("ResolveGroupPrincipal {0} => trouvé dans le cache", sid);
                }
                else
                {
                    foreach (var groupDomain in groupDomains)
                    {

                        DateTime start = DateTime.Now;
                        try
                        {
                            PrincipalContext searchTestContext = PrepareDomainContext(groupDomain).Result;
                            if (searchTestContext != null)
                            {
                                domainGroupPrincipal.groupPrincipal = GroupPrincipal.FindByIdentity(searchTestContext, IdentityType.Sid, sid);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogWarning(ex, "Could not join Domain :" + groupDomain);
                            throw ex;
                        }

                        if (domainGroupPrincipal.groupPrincipal != null)
                        {
                            domainGroupPrincipal.domain = groupDomain;
                            cacheGroupPrincipal.Add(sid, domainGroupPrincipal);
                            this.logger.LogDebug("ResolveGroupPrincipal {0}\\{1} => resolu : {2} ms", groupDomain, sid, (DateTime.Now - start).TotalMilliseconds);
                            return domainGroupPrincipal;

                        }
                        else
                        {
                            this.logger.LogDebug("ResolveGroupPrincipal {0}\\{1} => NON resolu : {2} ms", groupDomain, sid, (DateTime.Now - start).TotalMilliseconds);
                        }
                    }
                }
            }

            return domainGroupPrincipal;
        }

        public async Task<TUserFromDirectory> ResolveUserBySid(string sid, bool forceRefresh = false)
        {
            string KeyCache = KeyPrefixCacheUserSid + sid;
            return await ResolveUser(KeyCache, IdentityType.Sid, sid, forceRefresh);
        }

        public async Task<TUserFromDirectory> ResolveUserByLogin(string login, bool forceRefresh = false)
        {
            string KeyCache = KeyPrefixCacheUserLogin + login;
            return await ResolveUser(KeyCache, IdentityType.SamAccountName, login, forceRefresh);
        }

        private async Task<TUserFromDirectory> ResolveUser(string KeyCache, IdentityType identityType, string key, bool forceRefresh = false)
        {

            TUserFromDirectory itemResolve;
            itemResolve = (TUserFromDirectory)await this.ldapRepositoryHelper.distributedCache.Get(KeyCache);
            if (itemResolve != null)
            {
                if (forceRefresh)
                {
                    await this.ldapRepositoryHelper.distributedCache.Remove(KeyCache);
                    itemResolve = null;
                }
                else
                {
                    return itemResolve;
                }
            }
            foreach (var userDomain in ldapDomainsUsers)
            {
                PrincipalContext searchContext = await PrepareDomainContext(userDomain);
                if (searchContext != null)
                {
                    var userPrincipal = await Task.Run(() => UserPrincipal.FindByIdentity(searchContext, identityType, key));
                    if (userPrincipal != null)
                    {
                        itemResolve = GetUser(userPrincipal, userDomain.Name);
                        break;
                    }
                }
            }
            if (itemResolve == null)
            {
                itemResolve = new TUserFromDirectory();
            }
            await this.ldapRepositoryHelper.distributedCache.Add(KeyCache, itemResolve, this.LdapCacheUserDuration);
            return itemResolve;
        }
    }
}