using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.securitas
{
    public class UserForConfirmationCodeDTO
    {
        public string MemberId { get; set; }
        public string Email { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

    }
}
