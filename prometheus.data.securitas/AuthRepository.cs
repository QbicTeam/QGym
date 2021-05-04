using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using prometheus.model.securitas;



// using Model.Securitas;
// using QGym.API.Model.Security.Enums;

namespace prometheus.data.securitas
{
    public class AuthRepository : IAuthRepository
    {
        // int EXPIRATION_DATE_VIGENCY = 5;

        private readonly SecuritasDbContext _context;

        public AuthRepository(SecuritasDbContext context)
        {
            this._context = context;
        }

        public async Task<bool> ChangePassword(string username, string password, string newPassword)
        {
            var userDb = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (!string.IsNullOrEmpty(password))
            {
                if (!VerifyPasswordHash(password, userDb.PasswordHash, userDb.PasswordSalt))
                {
                    return false;
                }
            }

            byte[] passwordHash;
            byte[] passwordSalt;

            CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);

            userDb.PasswordHash = passwordHash;
            userDb.PasswordSalt = passwordSalt;

            
            await this._context.SaveChangesAsync();

            return true;

        }

        public async Task<User> Login(string username, string password)
        {
            var user = await this._context.Users
                .Include(r => r.Role)
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash;
            byte[] passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await this._context.Users.AddAsync(user);
            await this._context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UpdatingImported(string memberId, string email, string password)
        {
            // En la Importacion se debe de poner el Numero de Membrecia en el User.UserName.
            // En la Importacion se debe alimentar la info correspondiente en Memers
            var user = await this._context.Users
                .FirstOrDefaultAsync(u => u.UserName == memberId);

            byte[] passwordHash;
            byte[] passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            user.UserName = email;
            await this._context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await this._context.Users.AnyAsync(u => u.UserName == username))
            {
                return true;
            }

            return false;
        }

        // public async Task<List<Question>> GetQuestions()
        // {
        //     return await this._context.Questions.ToListAsync();
        // }

        public async Task<User> GetUserById(int userId)
        {
            // var user = await this._context.Users.Include(rc => rc.RegistrationCodes)
            //     .Include(qr => qr.QuestionResponses).FirstOrDefaultAsync(u => u.Id == userId);
            var user = await this._context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        // public async Task<string> RequestInvitation(string invitedEmail, string sponsorEmail, int roleId, string invitedName) 
        // {
        //     string rcode = GenerateInvitationCode();

        //     this._context.Invitations.Add(new RegistrationCode{
        //         Code = rcode,
        //         DateCreated = DateTime.Now,
        //         RoleId = roleId,
        //         DueDate = DateTime.Now.AddDays(EXPIRATION_DATE_VIGENCY),
        //         InvitedEmail = invitedEmail,
        //         InvitedName = invitedName,
        //         SponsorEmail = sponsorEmail,
        //         Status = RegistrationCodeStatusType.Requested.ToString()
        //     });

        //     await this._context.SaveChangesAsync();

        //     return rcode;
        // }

        // public async Task<List<RegistrationCode>> GetInvitations()
        // {
        //     return await this._context.Invitations.ToListAsync();
        // }

        // public async Task<RegistrationCode> GetInvitationById(int id)
        // {
        //     var reg = await this._context.Invitations.FirstOrDefaultAsync(r => r.Id == id);

        //     return reg;
        // }

        // public async Task<bool> UpdateRegisterCodeRecord(string regCode, string status, int userId) 
        // {
        //     var reg = await this._context.Invitations.FirstOrDefaultAsync(i => i.Code == regCode);
        //     var user = await this._context.Users.FirstOrDefaultAsync();


        //     if (reg != null)
        //     {
        //         reg.Status = status;
        //         if (user.RegistrationCodes == null) user.RegistrationCodes = new List<RegistrationCode>();
        //         user.RegistrationCodes.Add(reg);
        //         await this._context.SaveChangesAsync();

        //         return true;
        //     }
        //     else
        //     {
        //         return false;
        //     }

        // }

        // public async Task<string> CreateInvitation(RegistrationCode regCode)
        // {
        //     string code = GenerateInvitationCode();
        //     regCode.Code = code;
        //     regCode.DueDate = regCode.DateCreated.AddDays(EXPIRATION_DATE_VIGENCY);

        //     this._context.Invitations.Add(regCode);
        //     await this._context.SaveChangesAsync();

        //     return code;
        // }

        // private string GenerateInvitationCode() 
        // {
        //     string result = string.Empty;
        //     DateTime rDate =  DateTime.Now;

        //     Random rnd = new Random(DateTime.Now.Second);
        //     int fr1 = rnd.Next(10, 99);
        //     int fr2 = rnd.Next(10, 99);
        //     int fr3 = rnd.Next(10, 99);
        //     int fr4 = rnd.Next(10, 99);
        //     int fr5 = rnd.Next(10, 99);
        //     int fr6 = rnd.Next(10, 99);
        //     int fr7 = rnd.Next(10, 99);

        //     result = "r" + fr1.ToString() + rDate.Year.ToString()
        //         + fr2.ToString() + rDate.Month.ToString("00")
        //         + fr3.ToString() + rDate.Day.ToString("00")
        //         + fr4.ToString() + rDate.Hour.ToString("00")
        //         + fr5.ToString() + rDate.Minute.ToString("00")
        //         + fr6.ToString() + rDate.Second.ToString("00")
        //         + fr7.ToString();

        //     return result;
        // }

        public string GenerateConfirmationCode()
        {
            string result = string.Empty;
            DateTime rDate = DateTime.Now;

            Random rnd = new Random(DateTime.Now.Second);
            int fr1 = rnd.Next(0, 9);
            int fr2 = rnd.Next(0, 9);
            int fr3 = rnd.Next(0, 9);
            int fr4 = rnd.Next(0, 9);

            result = fr1.ToString()
                + fr2.ToString()
                + fr3.ToString()
                + fr4.ToString();

            // Todo: Remover cuando se pueda mandar el correo.
            // result = DateTime.Today.ToString("MMdd");

            return result;
        }

        public string ObfuscateEmail(string email)
        {            
            var result = string.Empty;

            var parts = email.Split('@');

            if(parts.Length == 1)
                return result;

            if(parts[0].Length >= 3)
                result = parts[0].Substring(0, 3) + "*****@" + parts[1]; 
            else
                result = parts[0].Substring(0, 1) + "*****@" + parts[1];


            return result;
        }
        public async Task<Role> GetRoleById(int id)
        {
            var role = await this._context.Roles.FirstOrDefaultAsync(r => r.Id == id);

            return role;
        }

        public async Task<Role> GetRoleByName(string name)
        {
            var role = await this._context.Roles.FirstOrDefaultAsync(r => r.DisplayName == name);

            return role;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }

        private bool VerifyPasswordHash(string password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int idx = 0; idx < computedHash.Length; idx++)
                {
                    if (computedHash[idx] != PasswordHash[idx]) return false;
                }
            }

            return true;
        }

        public async Task<bool> SaveAll()
        {
            return await this._context.SaveChangesAsync() > 0;
        }



    }
}