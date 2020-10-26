using prometheus.model.securitas;
using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.model.gym
{
    public class PaymentTransLog
    {
        public int Id { get; set; }
        public Member Member { get; set; }
        public DateTime Date { get; set; }
        public string Card { get; set; }
        public MembershipType Package { get; set; }
        public int PackageId { get; set; }
        public string PackageJson { get; set; }
        public int DaysValidity { get; set; }
        public double Amount { get; set; }
        public string ResultTrans { get; set; }
        public bool IsSuccess { get; set; }

    }
}
