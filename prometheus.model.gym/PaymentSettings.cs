using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.model.gym
{
    public class PaymentSettings
    {
        public string UserPayment { get; set; }
        public string PasswordPayment { get; set; }
        public string BaseUriPayment { get; set; }
        public string AutenticateUri { get; set; }
        public string CreateAccountUri { get; set; }
        public string CreateRoleUri { get; set; }
        public string ProcessPaymentUri { get; set; }

        

    }
}
