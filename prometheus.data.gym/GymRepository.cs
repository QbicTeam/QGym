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
using Microsoft.Extensions.Options;

namespace prometheus.data.gym
{
    public class GymRepository: IGymRepository
    {
        private readonly DataContext _context;
        private readonly IOptions<AppSettings> _appSettings;

        public GymRepository(DataContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }

        public void Add<T>(T entity) where T: class
        {
            _context.Add(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            _context.Remove(entity);
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
            member.PhotoUrl = _appSettings.Value.PhotoDefault;

            await this._context.Members.AddAsync(member);
            await this._context.SaveChangesAsync();

            return member;
        }
        public async Task<Member> GetMember(string email, string memberId, int userId)
        {
           var cliente = await _context.Members
            .Include(u => u.User).ThenInclude(r => r.Role)
            .FirstOrDefaultAsync(m => m.User.UserName == email || m.MemberId == memberId || m.User.Id == userId);

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
        public async Task<IEnumerable<Member>> GetMembersBookedInHour(DateTime bookedTime)
        {
            return await _context.Members
                .Include(u => u.User)
                .Include(p => p.MembershipTypeActive)
                .Where(s => _context.UserSchedulings.Where(us => us.Schedule == bookedTime).Select(us => us.UserId).Contains(s.User.Id))
                .ToListAsync();
        }
        public async Task<IEnumerable<Member>> GetMembersElegibles(DateTime day)
        {
            return await _context.Members
                .Include(u => u.User)
                .Include(p => p.MembershipTypeActive)
                .Where(m => m.MembershipExpiration > day && m.User.IsActive)
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

        public async Task<int> GetCurrentAuthorizedCapacity(DateTime date)
        {
            var capacity = await _context.AuthorizedCapacities
                .Where(ac => ac.StartDate <= date && ac.EndDate >= date )
                .Select(f => f.Capacity)
                .FirstOrDefaultAsync();

            if (capacity != null && capacity > 0)
                return capacity;

            var dateEndMax = _context.AuthorizedCapacities
                .Where(ac => ac.EndDate <= date)
                .Select(ed => ed.EndDate)
                .Max();

            capacity = await _context.AuthorizedCapacities
                .Where(ac => ac.EndDate == dateEndMax)
                .Select(f => f.Capacity)
                .FirstOrDefaultAsync();

            return capacity;
        }

        public int GetCurrentOccupationHour(DateTime date)
        {
            var occupation = _context.UserSchedulings
                .Where(us => us.Schedule == date)                
                .Count();

            return occupation;
        }

        public UserScheduling GetUserBookedDay(int userId, DateTime date)
        {
            var ds = new DateTime(date.Year, date.Month, date.Day);
            var de = ds.AddDays(1);
            var scheduled = _context.UserSchedulings 
                .Where(s => s.User.Id == userId && s.Schedule >= ds && s.Schedule < de)
                .FirstOrDefault();
            return scheduled;
        }
        public async Task<IEnumerable<UserScheduling>> GetUserBookedSummary(int userId)
        {
            return await _context.UserSchedulings
                .Where(s => s.User.Id == userId && s.Schedule >= DateTime.Today)
                .OrderBy(s => s.Schedule)
                .ToListAsync();
        }
        /// <summary>
        /// Regresa la cantidad de reservaciones por rango de horas en el dia 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public IEnumerable<Scheduled> GetBookedDay(DateTime date)
        {
            var ds = new DateTime(date.Year, date.Month, date.Day);
            var de = ds.AddDays(1);
            var result = _context.UserSchedulings
                    .Where(us => us.Schedule >= ds && us.Schedule < de)
                    .GroupBy(s => s.Schedule)                    
                    .Select(g => new Scheduled { Schedule = g.Key, Count = g.Count() })
                    .OrderBy(u => u.Schedule);
            return result;
        }

        public async Task<MembershipType> GetMembershipById(int id)
        {
            return await _context.MembershipTypes.FirstOrDefaultAsync(ms => ms.Id == id);
        }

        public async Task<IEnumerable<MembershipType>> GetPackages()
        {
            return await _context.MembershipTypes.OrderByDescending(m => m.PeriodicityDays).ThenBy(ms => ms.IsActive).ToListAsync();
        }
        public async Task<IEnumerable<MembershipType>> GetActivePackages()
        {
            return await _context.MembershipTypes.Where(p => p.IsActive).OrderByDescending(m => m.PeriodicityDays).ToListAsync();
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