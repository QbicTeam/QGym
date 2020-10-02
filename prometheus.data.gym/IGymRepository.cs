using System.Collections.Generic;
using System.Threading.Tasks;

/*
using Framework.DataTypes.Model.Base;
using Framework.DataTypes.Model.Licenciamiento;
using Framework.DataTypes.Model.Infraestructura;
*/

using prometheus.model.gym;
using prometheus.model.securitas;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace prometheus.data.gym
{
    public interface IGymRepository
    {
         void Add<T>(T entity) where T: class;
         Task<bool> SaveAll();

        Task<Member> Register(Member member, string memberId);
        Task<Member> GetMember(string email, string memberId);
        Task<User> GetUserById(int id);
        Task<IEnumerable<ValidationType>> GetValidationTypes();

        Task<IEnumerable<Member>> GetMembersToList();

        Task<IEnumerable<Member>> GetMembersType(string type);

        Task<IEnumerable<Member>> GetMembersForBlock();

        

        Task<Role> GetRoleByName(string roleName);

        Task<GeneralSettings> GetGeneralSettings();

        Task<IEnumerable<AuthorizedCapacity>> GetAuthorizedCapacityToList();

        Task<IEnumerable<MembershipType>> GetPackages();
        Task<MembershipType> GetMembershipById(int id);

        // Task<IEnumerable<User>> GetUsers();
        //Task<PagedList<User>> GetUsers(UserParams userParams);

        /*
        Task<Cliente> GetCliente(int id);
        Task<IEnumerable<Cliente>> GetClientes();
        */
    }
}