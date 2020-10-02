using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.EntityFrameworkCore;
/*
using Framework.DataTypes.Model.Base;
using Framework.DataTypes.Model.Licenciamiento;
using Framework.DataTypes.Model.Infraestructura;
*/
using prometheus.model.gym;
using prometheus.model.securitas;
using System;

namespace prometheus.data.gym
{
    public class GymRepository: IGymRepository
    {
        private readonly DataContext _context;

        public GymRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T: class
        {
            _context.Add(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Member> Register(Member member, string memberId)
        {

            member.MemberId = memberId;
            member.IsVerified = string.IsNullOrEmpty(memberId) ? false : true;
            member.MembershipExpiration = DateTime.Now.AddDays(-1);

            await this._context.Members.AddAsync(member);
            await this._context.SaveChangesAsync();

            return member;
        }
        public async Task<Member> GetMember(string email, string memberId)
        {
           var cliente = await _context.Members
            .Include(u => u.User).ThenInclude(r => r.Role)
            .FirstOrDefaultAsync(m => m.User.UserName == email || m.MemberId == memberId);

            return cliente;
        }
        public async Task<User> GetUserById(int id)
        {
            var userDb = await _context.Users
             .FirstOrDefaultAsync(u => u.Id == id);

            return userDb;
        }
        public async Task<IEnumerable<ValidationType>> GetValidationTypes()
        {
            return await _context.ValidationTypes.ToListAsync();
        }

        public async Task<IEnumerable<Member>> GetMembersToList()
        {
            return await _context.Members
                .Include(u => u.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Member>> GetMembersType(string type)
        {
            if (type == "valid")
            {
                DateTime mtoday = DateTime.Today;

                return await _context.Members
                    .Include(u => u.User)
                    .Where(m => m.MembershipExpiration >= DateTime.Today)
                    .ToListAsync();

            }

            return await _context.Members
                .Include(u => u.User)
                .Where(u => u.User.IsActive == true)
                .ToListAsync();

        }
        public async Task<IEnumerable<Member>> GetMembersForBlock()
        {
            return await _context.Members
                .Include(u => u.User)
                .ToListAsync();
        }
        
        public async Task<Role> GetRoleByName(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.DisplayName == roleName);
        }

        public async Task<GeneralSettings> GetGeneralSettings()
        {
            return await _context.GeneralSettings.FirstOrDefaultAsync(g => g.Id == 1);
        }

        public async Task<IEnumerable<AuthorizedCapacity>> GetAuthorizedCapacityToList()
        {
            return await _context.AuthorizedCapacities.OrderByDescending(a => a.EndDate).ToListAsync();
        }


        public async Task<MembershipType> GetMembershipById(int id)
        {
            return await _context.MembershipTypes.FirstOrDefaultAsync(ms => ms.Id == id);
        }

        public async Task<IEnumerable<MembershipType>> GetPackages()
        {
            return await _context.MembershipTypes.OrderByDescending(m => m.PeriodicityDays).ThenBy(ms => ms.IsActive).ToListAsync();
        }

        /*
        public async Task<IEnumerable<Cliente>> GetClientes()
        {
            var clientes = await _context.Clientes
            .Include(n => n.Negocios)
            //.Where(c => c.Id == id)
            .OrderBy(c => c.NomCorto)
            .ToListAsync();

            return clientes;
        }
        */

    }
}