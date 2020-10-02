using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Capacity
{
    public class AuthorizedCapacityDTO
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Capacity { get; set; }
        public decimal PercentageCapacity { get; set; }
    }
}
