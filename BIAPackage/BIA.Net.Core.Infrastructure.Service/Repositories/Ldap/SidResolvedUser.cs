using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Ldap
{
    [Serializable]
    public class SidResolvedUser<TUserFromDirectory> : SidResolvedItem
    {
        public TUserFromDirectory user;
    }
}
