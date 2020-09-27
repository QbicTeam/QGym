using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.securitas
{
    public class UserForUpdateRegisterDTO
    {
        public string MemberId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
