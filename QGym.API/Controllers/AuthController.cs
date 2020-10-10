using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System.Collections.Generic;
using System.Linq;

using prometheus.data.securitas;
using prometheus.model.securitas;
using prometheus.dto.securitas;
using prometheus.data.gym;
using prometheus.model.gym;
// using SIQbic.API.Model.Enums;

namespace QGym.API.Controllers 
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IGymRepository _repoGym;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IGymRepository repoGym, IConfiguration config)
        {
            this._repo = repo;
            this._repoGym = repoGym;
            this._config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDto)
        {
            try
            {
                // Create as User
                userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower();

                if (await this._repo.UserExists(userForRegisterDto.UserName))
                {
                    return BadRequest("Usuario previamente registrado."); // This username already exists
                }

                var memberRole = await this._repo.GetRoleByName("Miembro");
                var userToCreate = new User
                {
                    UserName = userForRegisterDto.UserName,
                    DisplayName = userForRegisterDto.DisplayName,
                    Role = memberRole, // Miembro
                    IsActive = true,
                    CreationDate = DateTime.Now,
                    LastModificationDate = DateTime.Now

                };

                var userCreated = await this._repo.Register(userToCreate, userForRegisterDto.Password);

                // Create a Member
                var userDb = await this._repoGym.GetUserById(userCreated.Id);
                var memberToCreate = new Member() { User = userDb };
                var memberCreated = await this._repoGym.Register(memberToCreate, "");

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }
        }

        [HttpPut("register")]
        public async Task<IActionResult> UpdatingImported(UserForUpdateRegisterDTO userForUpdateRegisterDto)
        {
            try
            {

                if (await this._repo.UserExists(userForUpdateRegisterDto.Email))
                {
                    return BadRequest("Usuario previamente registrado."); // This username already exists
                }

                await this._repo.UpdatingImported(userForUpdateRegisterDto.MemberId, userForUpdateRegisterDto.Email, userForUpdateRegisterDto.Password);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {

            try
            {
                var userFromRepo = await _repo.Login(userForLoginDTO.UserName.ToLower(), userForLoginDTO.Password);

                if (userFromRepo == null)
                {
                    return BadRequest("Credenciales Invalidas"); // Unauthorized();
                }

                if (!userFromRepo.IsActive)
                    return BadRequest("Usuario Bloqueado, contacte al Administrador.");

                var memberDb = await _repoGym.GetMember(userFromRepo.UserName, "", 0);
                // if (string.IsNullOrEmpty(userFromRepo.PhotoUrl))
                // {
                //     //TODO: This must be removed...
                //     userFromRepo.PhotoUrl = "http://majahide-001-site1.itempurl.com/releasecandidates/PhotosManagerAPI/prometheusmedia/SIQBICPROFILES/UserProfiles/nopic.jpg";
                // }
                
                
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, memberDb.MemberId),
                    new Claim(ClaimTypes.Email, userFromRepo.UserName),
                    new Claim(ClaimTypes.Name, userFromRepo.DisplayName)
                    // , new Claim(ClaimTypes.Webpage, userFromRepo.PhotoUrl)
                };


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._config.GetSection("AppSettings:Token").Value));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    //Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new
                {
                    token = tokenHandler.WriteToken(token),
                    role = userFromRepo.Role.DisplayName,
                    membershipExpiration = memberDb.MembershipExpiration,
                    photoUrl = memberDb.PhotoUrl
                });
            } catch(Exception ex)
            {
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }
        }

        // [HttpGet("questions")]
        // public async Task<IActionResult> GetQuestions()
        // {
        //     List<QuestionForList> result = new List<QuestionForList>();
            
        //     var data = await this._repo.GetQuestions();

        //     foreach(var itm in data)
        //     {
        //         result.Add(new QuestionForList{
        //             Id = itm.Id,
        //             DisplayValue = itm.DisplayText
        //         });
        //     }

        //     return Ok(result);
        // }

        [HttpPut("users/{id}")]
        public async Task<ActionResult> UpdateUser(int id, UserForUpdateDTO userForUpdate)
        {
            var user = await this._repo.GetUserById(id);

            user.DisplayName = userForUpdate.DisplayName;
            // if (!string.IsNullOrEmpty(userForUpdate.PhotoUrl ))
            //     user.PhotoUrl = userForUpdate.PhotoUrl;

            //TODO: Missing Update responses.

            //TODO: Missing change password

            await this._repo.SaveAll();

            return Ok(new { msg = ""});
        }

        [HttpPut("resetpassword")]
        public async Task<ActionResult> ResetPassword(UserForResertPwdDTO userForResetPwd)
        {
            try
            {
                var result = await this._repo.ChangePassword(userForResetPwd.Email, userForResetPwd.OldPassword, userForResetPwd.NewPassword);

                // await this._repo.SaveAll();
                if (result)
                    return Ok();
                else
                    return BadRequest("Credenciales Invalidas");
            }
            catch (Exception ex)
            {
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }
        }

        [HttpPost("confirmationCode")]
        public async Task<IActionResult> ConfirmationCode(UserForConfirmationCodeDTO confirmationDto)
        {
            try
            {
                var memberDb = await this._repoGym.GetMember(confirmationDto.Email, confirmationDto.MemberId, 0);

                if (memberDb == null)
                    return BadRequest("Usuario no encontrado");

                // TODO: Esto esta provicional, Falta determinar los campos de validacion.
                if (confirmationDto.Key == "Edad")
                {
                    if (confirmationDto.MemberId != confirmationDto.Value)
                    {
                        return BadRequest("Verificacion Invalida");
                    }
                }

                var codeConfirm = this._repo.GenerateConfirmationCode();

                // TODO: Envio de Correo.


                return Ok(codeConfirm);
            }
            catch (Exception ex)
            {
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }
        }


        public string CurrentPwd { get; set; }

        public string NewPwd { get; set; }

        // public List<QuestionResponseDTO> Responses { get; set; }  

        [HttpGet("users/{id}/settings")]
        public async Task<ActionResult> GetUserSettings(int id) {

            var user = await this._repo.GetUserById(id);

            UserForSettingsDTO result = new UserForSettingsDTO();

            result.Id = user.Id;
            result.DisplayName = user.DisplayName;
            result.UserName = user.UserName;
            // if (!string.IsNullOrEmpty(user.PhotoUrl))
            // {
            //     result.PhotoUrl = user.PhotoUrl;
            // }
            // else
            // {
            //     result.PhotoUrl = "http://majahide-001-site1.itempurl.com/releasecandidates/PhotosManagerAPI/prometheusmedia/SIQBICPROFILES/UserProfiles/nopic.jpg";
            // }
            // result.Questions = new List<QuestionForList>();

            // var questions = await this._repo.GetQuestions();

            // foreach(var q in questions) {
            //     result.Questions.Add(new QuestionForList{
            //         Id = q.Id,
            //         DisplayValue = q.DisplayText
            //     });
            // }

            // result.Responses = new List<QuestionResponseDTO>();

            // foreach(var qr in user.QuestionResponses) {
            //     result.Responses.Add(new QuestionResponseDTO {
            //         QuestionId = qr.Id,
            //         Response = qr.Response
            //     });
            // }

            return Ok(result);
        }

        [HttpGet("member/{memberId}")]
        public async Task<ActionResult> GetMembar(string memberId)
        {
            try
            {
                var memberDb = await this._repoGym.GetMember("", memberId, 0);

                var result = new MemberToValidateDTO();

                result.DisplayName = memberDb.User.DisplayName;

                result.Email = this._repo.ObfuscateEmail(memberDb.User.UserName);
                if (string.IsNullOrEmpty(result.Email))
                {
                    var lst = await this._repoGym.GetValidationTypes();
                    result.ValidationTypes = lst.Select(v => v.Name).ToList();
                }
                else
                {
                    result.ConfirmationCode = this._repo.GenerateConfirmationCode();
                    // TODO: Mandar correo de confirmacion
                }
                

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }

        }

        // [HttpGet("onboard/{rcode}")]
        // public async Task<ActionResult> StartRegistrationProcess(string rcode)
        // {
        //     string CODE_NOT_VALID_MESSAGE = "502 - Codigo de registro no valido.";
        //     string CODE_CANCELLED_MESSAGE = "503 - El codigo fue cancelado.";
        //     string CODE_DUE_DATE_MESSAGE ="504 - El codigo ya expiro.";
        //     string CODE_ALREADY_ACTIVADED_MESSAGE= "505 - Este codigo ya fue utilizado.";
        //     string CODE_IN_WAITING_APPROVAL = "506 - Invitacion en proceso de aprobacion";

        //     UserToStartRegister result = new UserToStartRegister();

        //     var data = await this._repo.GetInvitations();
        //     var regCode = data.ToList().FirstOrDefault(rc => rc.Code == rcode);

        //     if (regCode == null)
        //     {
        //         return BadRequest(CODE_NOT_VALID_MESSAGE);
        //     }
        //     else
        //     {
        //         RegistrationCodeStatusType userRegCodeStatus = (RegistrationCodeStatusType) Enum.Parse(typeof(RegistrationCodeStatusType), regCode.Status);

        //         if (userRegCodeStatus ==  RegistrationCodeStatusType.Cancelled)
        //         {
        //             return StatusCode(500, CODE_CANCELLED_MESSAGE);
        //         }
        //         else if (userRegCodeStatus == RegistrationCodeStatusType.Activated)
        //         {
        //             return StatusCode(500, CODE_ALREADY_ACTIVADED_MESSAGE);
        //         }
        //         else if (userRegCodeStatus == RegistrationCodeStatusType.Requested)
        //         {
        //             return StatusCode(500, CODE_IN_WAITING_APPROVAL);
        //         }
        //         else if (regCode.DueDate < DateTime.Now)
        //         {
        //             return StatusCode(500, CODE_DUE_DATE_MESSAGE);
        //         }
        //         else
        //         {
        //             return Ok (new UserToStartRegister{
        //                 Email = regCode.InvitedEmail
        //              });
        //         }
        //     }

        // }

        // [HttpPost("invites/{rcode}")]
        // public async Task<ActionResult> RequestInvite(string rcode)
        // {
        //     string CODE_NOT_VALID_MESSAGE = "502 - Codigo de registro no valido.";

        //     UserToStartRegister result = new UserToStartRegister();

        //     var data = await this._repo.GetInvitations();
        //     var regCode = data.FirstOrDefault(rc => rc.Code == rcode);

        //     if (regCode == null)
        //     {
        //         return BadRequest(CODE_NOT_VALID_MESSAGE);
        //     }
        //     else
        //     {

        //         RegistrationCodeStatusType userRegCodeStatus = (RegistrationCodeStatusType) Enum.Parse(typeof(RegistrationCodeStatusType), regCode.Status);

        //         await this._repo.RequestInvitation(regCode.InvitedEmail, regCode.SponsorEmail, regCode.RoleId, regCode.InvitedName);

        //         return Ok();
        //     }

        // }

        // [HttpPut("invites/{id}")]
        // public async Task<ActionResult> UpdateInvite(int id, InviteForUpdateDTO inviteForUpdate)
        // {
        //     var reg = await this._repo.GetInvitationById(id);

        //     reg.RoleId = inviteForUpdate.RoleId;
        //     reg.InvitedName = inviteForUpdate.InvitedName;
        //     await this._repo.SaveAll();

        //     return Ok();
        // }

        // [HttpDelete("invites/{id}")]
        // public async Task<ActionResult> CancelInvite(int id)
        // {
        //     var reg = await this._repo.GetInvitationById(id);

        //     reg.Status = RegistrationCodeStatusType.Cancelled.ToString();

        //     await this._repo.SaveAll();

        //     return Ok();
        // }

        // [HttpPut("invites/{id}/approve")]
        // public async Task<ActionResult> ApproveInvite(int id)
        // {
        //     var reg = await this._repo.GetInvitationById(id);

        //     reg.Status = RegistrationCodeStatusType.Accepted.ToString();

        //     await this._repo.SaveAll();

        //     return Ok();
        // }

        // [HttpPost("invites")]
        // public async Task<ActionResult> CreateInvite(InviteForCreationDTO inviteForCreation)
        // {

        //     RegistrationCode regCode = new RegistrationCode {
        //         DateCreated = DateTime.Now,
        //         InvitedEmail = inviteForCreation.InvitedEmail,
        //         InvitedName = inviteForCreation.InvitedName,
        //         SponsorEmail = inviteForCreation.SponsorEmail,
        //         RoleId = inviteForCreation.RoleId,
        //         Status = RegistrationCodeStatusType.Created.ToString()
        //     };

        //     await this._repo.CreateInvitation(regCode);

        //     var result = new RegistrationCodeForReport{
        //             Id = regCode.Id,
        //             DateCreated = regCode.DateCreated,
        //             DueDate = regCode.DueDate,
        //             InvitedEmail = regCode.InvitedEmail,
        //             InvitedName = regCode.InvitedName,
        //             SponsorEmail = regCode.SponsorEmail,
        //             Status = regCode.Status,
        //             Code = regCode.Code
        //         };

        //     if (inviteForCreation.RoleId == 0) result.Role = new Role { Id = inviteForCreation.RoleId, DisplayName="Usuario" };
        //     if (inviteForCreation.RoleId == 1) result.Role = new Role { Id = inviteForCreation.RoleId, DisplayName="Operador" };
        //     if (inviteForCreation.RoleId == 2) result.Role = new Role { Id = inviteForCreation.RoleId, DisplayName="Administrador" };

        //     return Ok(result);

        // }

        // [HttpGet("invites")]
        // public async Task<ActionResult> GetInvites() 
        // {
        //     var invitations = await this._repo.GetInvitations();

        //     var result = new List<RegistrationCodeForReport>();
        //     foreach(var i in invitations)
        //     {
        //         result.Add(new RegistrationCodeForReport{
        //             Id = i.Id,
        //             DateCreated = i.DateCreated,
        //             DueDate = i.DueDate,
        //             InvitedEmail = i.InvitedEmail,
        //             InvitedName = i.InvitedName,
        //             SponsorEmail = i.SponsorEmail,
        //             Status = i.Status,
        //             Code = i.Code
        //         });

        //         if (i.RoleId == 0) result[result.Count-1].Role = new Role { Id = 0, DisplayName = "Usuario"};
        //         if (i.RoleId == 1) result[result.Count-1].Role = new Role { Id = 1, DisplayName = "Operador"};
        //         if (i.RoleId == 2) result[result.Count-1].Role = new Role { Id = 2, DisplayName = "Administrador"};

        //     }

        //     return Ok(result);
        // }


    }
}