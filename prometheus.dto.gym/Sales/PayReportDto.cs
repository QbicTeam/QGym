using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.Sales
{
    public class PayReportDto
    {
        public DateTime SaleDate { get; set; }
        public string FullName { get; set; }
        public string MembershipType { get; set; }
        public string Period { get; set; }
        public Double Price { get; set; }
        public DateTime Vigency { get; set; }
        public string MemberId { get; set; }
        public int UserId { get; set; }
        public string ReferntPayment { get; set; }

        // Fakes
        public string Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string Phone { get; set; }






    }
}
