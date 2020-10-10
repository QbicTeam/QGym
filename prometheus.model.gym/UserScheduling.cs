using prometheus.model.securitas;
using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.model.gym
{
    public class UserScheduling
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public DateTime Schedule { get; set; }
        public bool IsAttended { get; set; }
    }
}
