using prometheus.dto.gym.Membership;
using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Members
{
    public class MemberDTO
    {
        public int Id { get; set; }
        //-- User
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; } // fullName  - Anterior: DisplayName
        public bool IsBlock { get; set; } // IsActive
        public DateTime CreationDate { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public string RoleName { get; set; } //
        //--
        public string MemberId { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime MembershipExpiration { get; set; }
        public int? MembershipTypeActiveId { get; set; }
        
        public MembershipTypeDTO MembershipTypeActive { get; set; }
        
        public string BlockingReason { get; set; }
        public bool IsVerified { get; set; }
        public DateTime Birthdate { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
    }
}
