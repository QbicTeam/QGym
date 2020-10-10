using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Members
{
    public class HoursData
    {
        public string Range { get; set; }
        // Disponibilidad Restante.
        public string Capacity { get; set; }
        public bool Booked { get; set; }
    }
}
