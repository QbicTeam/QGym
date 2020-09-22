using System;
using System.Collections.Generic;

namespace QGym.API.Model.Security
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }
                
        public string DisplayName { get; set; }
        
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public bool IsActive { get; set; }

        // public string PhoneNumber { get; set; }

        // public string PhotoSourceCode { get; set; }

        // public string PhotoUrl { get; set; }

        public DateTime CreationDate { get; set; }
        
        public DateTime? LastModificationDate { get; set; }  

        // public List<RegistrationCode> RegistrationCodes { get; set; }          

        // public List<QuestionResponse> QuestionResponses { get; set; }

        public Role Role { get; set; }
    }
}