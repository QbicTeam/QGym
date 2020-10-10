using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Members
{
    public class ScheduleDayDTO
    {
        public DateTime Date { get; set; }
        public List<HoursData> SelectableHours { get; set; }
    }
}
