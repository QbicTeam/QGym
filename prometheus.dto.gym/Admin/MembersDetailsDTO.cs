using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Admin
{
    public class MembersDetailsDTO
    {
        public string MemberId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Package { get; set; }
        public string Period { get; set; }
        public DateTime DueDate { get; set; }
        public string PhotoUrl { get; set; }
        public int UserId { get; set; }

    }
}
