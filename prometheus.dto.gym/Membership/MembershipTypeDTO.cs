using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Membership
{
    public class MembershipTypeDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int PeriodicityDays { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
