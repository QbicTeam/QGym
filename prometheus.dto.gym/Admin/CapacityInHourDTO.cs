using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Admin
{
    public class CapacityInHourDTO
    {
        public int Capacity { get; set; }
        public int BookedPercentage { get; set; }
        public int Booked { get; set; }
        public int Availability { get; set; }
    }
}
