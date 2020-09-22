using System.Collections.Generic;

namespace QGym.API.DTO.Security
{
    public class UserForUpdateDTO
    {
        public string DisplayName { get; set; }

        public string PhotoUrl { get; set; }

        public string CurrentPwd { get; set; }

        public string NewPwd { get; set; }

        // public List<QuestionResponseDTO> Responses { get; set; }  
    }
}

