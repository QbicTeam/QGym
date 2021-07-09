using System;
using System.Collections.Generic;
using System.Text;

namespace prometheus.dto.gym.PrometheusMgr
{
    public class SendEmailDto
    {
        public string To { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public bool IsHtml { get; set; }
        public int TemplateId { get; set; }
        public List<Variables> Values { get; set; }
    }

    public class Variables
    {
        public string Variable { get; set; }
        public string Value { get; set; }
    }
}
