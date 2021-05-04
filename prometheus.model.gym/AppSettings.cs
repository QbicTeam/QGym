using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.model.gym
{
    public class AppSettings
    {
        public string Token { get; set; }
        public string ServerError { get; set; }
        public string PhotoDefault { get; set; }

        public string EmailUrl { get; set; }
        public string EmailApiKey { get; set; }
        public string SubjectConfirmationEmail { get; set; }
        public int TemplateIdConfirmationEmail { get; set; }

    }
}
