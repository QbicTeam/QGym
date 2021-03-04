using System;
using System.ComponentModel.DataAnnotations.Schema;
using prometheus.model.securitas;

namespace prometheus.model.gym
{
    public class Member
    {
        public int Id { get; set; }
        
        public User User { get; set; }

        public string MemberId { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime MembershipExpiration { get; set; }
        public int? MembershipTypeActiveId { get; set; }
        public MembershipType MembershipTypeActive { get; set; }
        public string BlockingReason { get; set; }
        public bool IsVerified { get; set; }
        public DateTime Birthdate { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
    }
}