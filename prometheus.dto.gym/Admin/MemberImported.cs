using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Admin
{
    public class MemberImported
    {
        public string MemberId { get; set; }
        public string Name { get; set; }
        public DateTime MembershipExpiration { get; set; }
    }
}
