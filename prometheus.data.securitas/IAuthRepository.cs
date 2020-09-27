using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// using QGym.API.Model.Security;
//using Model.Securitas;
using prometheus.model.securitas;

namespace prometheus.data.securitas
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<bool> UpdatingImported(string memberId, string email, string password);

        Task<User> Login(string username, string password);

        Task<bool> UserExists(string username);

        Task<bool> ChangePassword(string username, string password, string newPassword);        

        Task<User> GetUserById(int userId);
        
        Task<Role> GetRoleByName(string name);
        Task<Role> GetRoleById(int id);

        string GenerateConfirmationCode();

        string ObfuscateEmail(string email);

        Task<bool> SaveAll();
    }
}