using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.securitas
{
    public class UserForResertPwdDTO
    {
        public string Email { get; set; }
        /// <summary>
        /// Only if necesary validate oldPwd.
        /// </summary>
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
