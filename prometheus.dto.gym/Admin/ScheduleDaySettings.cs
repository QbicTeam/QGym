using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Admin
{
    public class ScheduleDaySettings
    {
        public string Day { get; set; }
        public List<HoursRange> RangeDates { get; set; }

    }
}
