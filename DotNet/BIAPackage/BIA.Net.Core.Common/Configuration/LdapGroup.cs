using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Common.Configuration
{
    public class LdapGroup
    {
        /// <summary>
        /// List of domain of the user to find in this Ldap Group. (use to now where to add the user)
        /// </summary>
        public string[] AddUsersOfDomains { get; set; }

         /// <summary>
        /// List of domain of the user to find in this Ldap Group.
        /// </summary>
        public string[] RecursiveGroupsOfDomains { get; set; }

       /// <summary>
        /// Name of the group as find in the Ldap.
        /// </summary>
        public string LdapName { get; set; }

        /// <summary>
        /// Name of the Domain (reference to Authentication > LdapDomains > Name).
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Name of the Domain (reference to Authentication > LdapDomains > Name).
        /// </summary>
        public bool ContainsOnlyUsers { get; set; }
    }
}
