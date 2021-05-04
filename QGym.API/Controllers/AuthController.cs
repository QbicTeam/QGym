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
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

using QGym.API.Helpers;
using System.Reflection;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

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
        private readonly IOptions<AppSettings> _appSettings;

        public AuthController(IAuthRepository repo, IGymRepository repoGym, IConfiguration config, IOptions<AppSettings> appSettings)
        {
            this._repo = repo;
            this._repoGym = repoGym;
            this._config = config;
            this._appSettings = appSettings;
        }
        [HttpPost("register/confirmationCode")]
        public async Task<ActionResult> RegisterConfirmationCode(UserForConfirmationCodeDTO confirmation)
        {
            try
            {

                var memberDb = await this._repoGym.GetMember(confirmation.Email, null, 0);
                if (memberDb != null)
                    return BadRequest("Usuario ya registrado, intente hacer login");


                var codeConfirm = this._repo.GenerateConfirmationCode();

                // Envio de Correo.
                 new PrometheusServicesHelper(this._appSettings).SendEmail(memberDb.User.UserName, memberDb.User.DisplayName, codeConfirm);

                return Ok(codeConfirm);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, confirmation, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }
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
                var memberToCreate = new Member() { User = userDb};
                var memberCreated = await this._repoGym.Register(memberToCreate, "");


                return StatusCode(201);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, userForRegisterDto, ex);
                return BadRequest(this._appSettings.Value.ServerError);
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
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, userForUpdateRegisterDto, ex);
                return BadRequest(this._appSettings.Value.ServerError);
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

                var memberDb = await _repoGym.GetMember(userFromRepo.UserName, null, 0);
                // if (string.IsNullOrEmpty(userFromRepo.PhotoUrl))
                // {
                //     //TODO: This must be removed...
                //     userFromRepo.PhotoUrl = "http://majahide-001-site1.itempurl.com/releasecandidates/PhotosManagerAPI/prometheusmedia/SIQBICPROFILES/UserProfiles/nopic.jpg";
                // }

                /*
                var userData = new {
                    role = userFromRepo.Role.DisplayName,
                    membershipExpiration = memberDb.MembershipExpiration,
                    photoUrl = memberDb.PhotoUrl,
                    packageId = memberDb.MembershipTypeActiveId
                };
                */
                // uri foto
                // role role
                // version paquete
                // expiration

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, memberDb.User.Id.ToString()),
                    new Claim(ClaimTypes.Email, userFromRepo.UserName),
                    new Claim(ClaimTypes.Name, userFromRepo.DisplayName) //,
                    //new Claim(ClaimTypes.Uri, memberDb.PhotoUrl == null ? "" : memberDb.PhotoUrl),
                    //new Claim(ClaimTypes.Role, userFromRepo.Role.DisplayName),
                    //new Claim(ClaimTypes.Version, memberDb.MembershipTypeActiveId == null ? "0" : memberDb.MembershipTypeActiveId.ToString()),
                    //new Claim(ClaimTypes.Expiration, memberDb.MembershipExpiration.ToString("yyyy-MM-dd"))

                    //new Claim(ClaimTypes.UserData,  JsonConvert.SerializeObject(userData))
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
                    photoUrl = memberDb.PhotoUrl,
                    packageId = memberDb.MembershipTypeActiveId == null ? 0 : memberDb.MembershipTypeActiveId,
                    memberId = memberDb.MemberId
                });
                // return Ok(token);
            } catch(Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, userForLoginDTO, ex);
                return BadRequest(this._appSettings.Value.ServerError);
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
            try
            {


                var user = await this._repo.GetUserById(id);

                user.DisplayName = userForUpdate.DisplayName;
                // if (!string.IsNullOrEmpty(userForUpdate.PhotoUrl ))
                //     user.PhotoUrl = userForUpdate.PhotoUrl;

                //TODO: Missing Update responses.

                //TODO: Missing change password

                await this._repo.SaveAll();

                return Ok(new { msg = "" });
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, userForUpdate, ex, "id: " + id.ToString());
                return BadRequest(this._appSettings.Value.ServerError);
            }

        }

        [HttpPost("resetpassword/validation")]
        public async Task<ActionResult> ResetPasswordValidation(UserForConfirmationCodeDTO toValidation)
        {
            try
            {

                var memberDb = await this._repoGym.GetMember(toValidation.MemberId, toValidation.MemberId, 0);
                if (memberDb == null)
                    return BadRequest("Usuario no encontrado");

                var lst = await this._repoGym.GetValidationTypes();

                return Ok(new { email = memberDb.User.UserName, keys = lst.Select(v => v.Name).ToList() });
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, toValidation, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }
        [HttpPost("resetpassword/confirmation")]
        public async Task<ActionResult> ResetPasswordConfirmation(UserForConfirmationCodeDTO confirmation)
        {
            try
            {
                var valid = false;
                var memberDb = await this._repoGym.GetMember(confirmation.Email, confirmation.MemberId, 0);
                if (memberDb == null)
                    return BadRequest("Usuario no encontrado");

                // Validation Confirmation Data
                valid = ValidateConfirmationData(confirmation, memberDb, valid);

                if (!valid)
                    return BadRequest("Verificacion Invalida");
                

                var codeConfirm = this._repo.GenerateConfirmationCode();

                // Envio de Correo.
                new PrometheusServicesHelper(this._appSettings).SendEmail(memberDb.User.UserName, memberDb.User.DisplayName, codeConfirm);

                return Ok(codeConfirm);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, confirmation, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }
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
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, userForResetPwd, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }


        [HttpPost("confirmationCode")]
        public async Task<IActionResult> ConfirmationCode(UserForConfirmationCodeDTO confirmationDto)
        {
            // Este metodo valida el usuario por el MemberId, y verifica que el Correo Enviado, no este registrado previamente.
            try
            {
                var memberDb = await this._repoGym.GetMember(null, confirmationDto.MemberId, 0);

                if (memberDb == null)
                    return BadRequest("Usuario no encontrado");

                var memberEmailDb = await this._repoGym.GetMember(confirmationDto.Email, null, 0);
                if (memberEmailDb != null)
                    return BadRequest("El correo ya se encuentra Registrado");


                var valid = false;
                // Validation Confirmation Data
                valid = ValidateConfirmationData(confirmationDto, memberDb, valid);

                if (!valid)
                    return BadRequest("Verificacion Invalida");

                var codeConfirm = this._repo.GenerateConfirmationCode();

                // Envio de Correo.
                new PrometheusServicesHelper(this._appSettings).SendEmail(memberDb.User.UserName, memberDb.User.DisplayName, codeConfirm);

                return Ok(codeConfirm);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, confirmationDto, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }

        private static bool ValidateConfirmationData(UserForConfirmationCodeDTO confirmationDto, Member memberDb, bool valid)
        {
            switch (confirmationDto.Key)
            {
                case "Birthdate":
                    if (memberDb.Birthdate.ToString("dd/MM/yyyy") == confirmationDto.Value) valid = true;
                    break;
                case "Phone":
                    if (memberDb.Phone == confirmationDto.Value) valid = true;
                    break;
                case "CellPhone":
                    if (memberDb.CellPhone == confirmationDto.Value) valid = true;
                    break;
            }

            return valid;
        }

        public string CurrentPwd { get; set; }

        public string NewPwd { get; set; }

        // public List<QuestionResponseDTO> Responses { get; set; }  

        [HttpGet("users/{id}/settings")]
        public async Task<ActionResult> GetUserSettings(int id) 
        {
            try
            {
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
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, id, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }

        [HttpGet("member/{memberId}")]
        public async Task<ActionResult> GetMembar(string memberId)
        {
            try
            {
                var memberDb = await this._repoGym.GetMember(null, memberId, 0);

                if (memberDb == null)
                    return NotFound("Socio no entrado."); // BadRequest("Usuario no encontrado");

                if (memberDb.User.PasswordHash != null)
                    return BadRequest("Socio ya registrado, intente hacer Login.");

                var result = new MemberToValidateDTO();

                result.DisplayName = memberDb.User.DisplayName;

                result.EmailObfuscated = this._repo.ObfuscateEmail(memberDb.User.UserName);
                if (string.IsNullOrEmpty(result.EmailObfuscated))
                {
                    var lst = await this._repoGym.GetValidationTypes();
                    result.ValidationTypes = lst.Select(v => v.Name).ToList();
                }
                else
                {
                    result.Email = memberDb.User.UserName;
                    result.ConfirmationCode = this._repo.GenerateConfirmationCode(); // Convert.ToInt32(this._repo.GenerateConfirmationCode());
                    // Mandar correo de confirmacion
                    new PrometheusServicesHelper(this._appSettings).SendEmail(memberDb.User.UserName, memberDb.User.DisplayName, result.ConfirmationCode);
                }
                

                return Ok(result);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, memberId, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }

        }

        
        /*
        static void SendEmail()
        {
            var emailUrl = "http://prometheusapis.net/emailapi/emails";

            WebRequest oRequest = WebRequest.Create(emailUrl);
            oRequest.Method = "post";
            oRequest.ContentType = "application/json;charset-UTF-8";
            // oRequest.Headers["x-api-key"] = "03ffbf2f-f820-4655-90f2-ea7dc1689fba";
            oRequest.Headers.Add("x-api-key","03ffbf2f-f820-4655-90f2-ea7dc1689fba");
            using (var oSW = new StreamWriter(oRequest.GetRequestStream()))
            {
                var values = new List<Variables>();
                values.Add(new Variables() { Variable = "Socio", Value = "Carlos F." });
                values.Add(new Variables() { Variable = "Codigo", Value = "12yTres4" });
                var emailCode = new SendEmailDto()
                {
                    To = "carlossotoocio@gmail.com",
                    Body = "",
                    Subject = "Codigo de Confirmacion",
                    IsHtml = true,
                    TemplateId = 4,
                    Values = values
                };
                string json = JsonConvert.SerializeObject(emailCode);
                oSW.Write(json);
                oSW.Flush();
                oSW.Close();
            }
            WebResponse oResponse = oRequest.GetResponse();
            using (var oSR = new StreamReader(oResponse.GetResponseStream()))
            {
                oSR.ReadToEnd();
            }
        }
        */
    }
}