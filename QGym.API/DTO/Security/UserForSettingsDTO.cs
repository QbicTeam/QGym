using System.Collections.Generic;

namespace QGym.API.DTO.Security
{
    public class UserForSettingsDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        
        public string DisplayName { get; set; }

        public string PhotoUrl { get; set; }

        // public List<QuestionForList> Questions { get; set; }

        // public List<QuestionResponseDTO> Responses { get; set; }  
    }
}