using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Admin
{
    public class Imported
    {
        public List<MemberImported> MembersImported { get; set; }
        public List<MemberImported> MembersUpdateVilidity  { get; set; }
    }
}
