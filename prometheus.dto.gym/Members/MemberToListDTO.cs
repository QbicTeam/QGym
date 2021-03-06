﻿using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Members
{
    public class MemberToListDTO
    {
        public string MemberId { get; set; }
        public string FullName { get; set; } // fullName  - Anterior: DisplayName
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public string SearchText { get; set; }
        public int UserId { get; set; }

    }
}
