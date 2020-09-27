using System;

namespace prometheus.model.gym
{
    public class AuthorizedCapacity
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Capacity { get; set; }
        public decimal PercentageCapacity { get; set; }

    }
}