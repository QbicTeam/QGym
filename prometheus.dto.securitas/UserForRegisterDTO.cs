using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prometheus.dto.securitas
{
    public class UserForRegisterDTO
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(8, MinimumLength= 4, ErrorMessage="Invalid length, password must between 4 and 8")]
        public string Password { get; set; }
        
        public string DisplayName { get; set; }        
        
        [Required]
        public string Phone { get; set; }

        [Required]
        public string Question1 { get; set; }

        [Required]
        public string Question2 { get; set; }

        [Required]
        public string Question3 { get; set; }

        [Required]
        public string Response1 { get; set; }

        [Required]
        public string Response2 { get; set; }

        [Required]
        public string Response3 { get; set; }

        [Required]
        public string RegistrationCode { get; set; }

        public string PhotoUrl { get; set; }

    }
}