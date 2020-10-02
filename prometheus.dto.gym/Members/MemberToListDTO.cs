using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Members
{
    public class MemberToListDTO
    {
        public string MemberId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }

    }
}
