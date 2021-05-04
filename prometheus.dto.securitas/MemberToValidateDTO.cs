using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.securitas
{
    public class MemberToValidateDTO
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string EmailObfuscated { get; set; }
        public string ConfirmationCode { get; set; }

        public List<string> ValidationTypes { get; set; }

    }
}
