using System;
// using QGym.API.Model.Security;

using prometheus.model.securitas;

namespace prometheus.dto.securitas
{
    public class RegistrationCodeForReport
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DueDate { get; set; }

        public string InvitedEmail { get; set; }

        public string InvitedName { get; set; }

        public string SponsorEmail { get; set; }

        public Role Role { get; set; }       
    }
}