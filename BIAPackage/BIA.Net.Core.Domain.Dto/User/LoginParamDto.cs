using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIA.Net.Core.Domain.Dto.User
{
    public class LoginParamDto
    {
        /// <summary>
        /// Gets or sets is the current teams logged.
        /// </summary>
        public CurrentTeamDto[] CurrentTeamLogins { get; set; }

        /// <summary>
        /// Gets or sets is the teams config.
        /// </summary>
        public TeamConfigDto[] TeamsConfig { get; set; }

        /// <summary>
        /// Gets or sets if it requiered a complet token.
        /// </summary>
        public bool LightToken { get; set; }
    }
}
