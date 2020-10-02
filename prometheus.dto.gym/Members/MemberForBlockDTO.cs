﻿using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Members
{
    public class MemberForBlockDTO
    {
        public string MemberId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public bool IsBlock { get; set; }
        public string BlockingReason { get; set; }

    }
}