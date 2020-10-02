using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Capacity
{
    public class AuthorizedCapacityReportDTO
    {
        public int TotalCapacity { get; set; }
        public List<AuthorizedCapacityDTO> AuthorizedCapacities { get; set; }
    }
}
