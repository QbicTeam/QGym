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
using AutoMapper;
using prometheus.dto.gym.Members;
using prometheus.dto.gym.Admin;
using Newtonsoft.Json;
using System.Reflection;
using QGym.API.Helpers;
using Microsoft.Extensions.Options;
using prometheus.dto.gym.Membership;

namespace QGym.API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class MembersController : ControllerBase
    {
        private readonly IGymRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _appSettings;

        public MembersController(IGymRepository repo, IConfiguration config, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            this._repo = repo;
            this._config = config;
            this._mapper = mapper;
            this._appSettings = appSettings;
        }

        [HttpGet()]
        public async Task<ActionResult> GetMembars()
        {
            try
            {

                var dataDb = await this._repo.GetMembersToList();  
                //USO: List<Destino> personViews = Mapper.Map<List<Origen>, List<Destino>>(people);
      
                var result = _mapper.Map<List<Member>, List<MemberToListDTO>>(dataDb.ToList());


                return Ok(result);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, "N/A", ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }

        }

        [HttpGet("{type}")]
        public async Task<ActionResult> GetMembarsType(string type)
        {
            try
            {
                if (type != "valid" && type != "active")
                    return BadRequest("Petición invalida");

                var dataDb = await this._repo.GetMembersType(type);

                var result = _mapper.Map<List<Member>, List<MemberToListDTO>>(dataDb.ToList());


                return Ok(result);

            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, type, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }

        }

        [HttpGet("{memberId}/details/forBlock")]
        public async Task<ActionResult> GetMembarsForBlock(string memberId)
        {
            try
            {
                var dataDb = await this._repo.GetMember(null, memberId, 0);

                var result = _mapper.Map<MemberForBlockDTO>(dataDb);

                return Ok(result);

            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, memberId, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }

        }

        [HttpGet("{userId}/details/complete")]
        public async Task<ActionResult> GetMembarsFull(int userId)
        {
            try
            {
                var dataDb = await this._repo.GetMember(null, null, userId); // memberId
                var package = new MembershipType();

                if (dataDb.MembershipTypeActiveId != null && dataDb.MembershipTypeActiveId > 0)
                    package = await this._repo.GetMembershipById(dataDb.MembershipTypeActiveId.Value);

                var result = _mapper.Map<MemberDTO>(dataDb);
                if (package.Id > 0)
                    result.MembershipTypeActive = _mapper.Map<MembershipTypeDTO>(package);

                return Ok(result);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, userId, ex); // memberId, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }

        }
        
        [HttpPut("{memberId}/status")]
        public async Task<IActionResult> UpdatingStatus(string memberId, [FromBody] MemberForBlockDTO data)
        {
            try
            {

                var user = await this._repo.GetMember(null, memberId, 0);

                user.User.IsActive = !data.IsBlock;   //isBlock == 1? false : true;
                user.BlockingReason = data.BlockingReason;

                await this._repo.SaveAll();
                
                return Ok();
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, data, ex,"MemberId: " + memberId);
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> Updating([FromBody] MemberDTO member, int userId) // string memberId)
        {
            try
            {
                var memberDb = await this._repo.GetMember(null, null, userId); // memberId, 0); 

                if (memberDb == null)
                    return NotFound("El Usuario no esta registrado");

                if (!string.IsNullOrEmpty(member.MemberId) && string.IsNullOrEmpty(memberDb.MemberId))
                {
                    var validateMemberId = await this._repo.GetMember(null, member.MemberId, 0);
                    if (validateMemberId == null)
                        memberDb.MemberId = member.MemberId;
                    else
                        return BadRequest("El Numero de Membresia ya esta registrado.");
                }
                    

                if (!string.IsNullOrEmpty(member.PhotoUrl) ) // && string.IsNullOrEmpty(memberDb.PhotoUrl))
                    memberDb.PhotoUrl = member.PhotoUrl;

                //if (!string.IsNullOrEmpty(member.PhotoUrl) && memberDb.PhotoUrl != member.PhotoUrl)
                //    memberDb.PhotoUrl = member.PhotoUrl;

                if (member.MembershipExpiration > memberDb.MembershipExpiration)
                    memberDb.MembershipExpiration = member.MembershipExpiration;

                if (memberDb.MembershipTypeActiveId == null && memberDb.MembershipTypeActiveId > 0)
                {
                    var membership = await this._repo.GetMembershipById(member.MembershipTypeActiveId.Value);
                    memberDb.MembershipTypeActive = membership;
                }

                if (member.IsVerified)
                    memberDb.IsVerified = true;

                if (!string.IsNullOrEmpty(member.Email))
                    memberDb.User.UserName = member.Email;

                if (!string.IsNullOrEmpty(member.FullName))
                    memberDb.User.DisplayName = member.FullName;

                if (!string.IsNullOrEmpty(member.RoleName) && member.RoleName != memberDb.User.Role.DisplayName)
                {
                    var role = await this._repo.GetRoleByName(member.RoleName);
                    memberDb.User.Role = role;
                }

                memberDb.User.IsActive = !member.IsBlock;

                if (!string.IsNullOrEmpty(member.BlockingReason))
                    memberDb.BlockingReason = member.BlockingReason;


                memberDb.User.LastModificationDate = DateTime.Today;




                    await this._repo.SaveAll();

                return Ok();

            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, member, ex, "userId: " + userId.ToString());
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }
        [HttpGet("{userId}/schedule/summary")]
        public async Task<ActionResult> ScheduleSummary(int userId)
        {
            var resutl = new List<ScheduleSumaryDTO>();

            try
            {
                var memberDb = await this._repo.GetMember(null, null, userId);

                if (memberDb == null)
                    return BadRequest("Socio no encontrado.");

                if (!memberDb.User.IsActive)
                    return BadRequest("Socio Bloqueado.");

                
                var settings = await this._repo.GetGeneralSettings();
                var scheduleSettings = JsonConvert.DeserializeObject<List<ScheduleDaySettings>>(settings.ScheduledWeek);
                var scheduleUserDb = await this._repo.GetUserBookedSummary(userId);

                foreach(UserScheduling s in scheduleUserDb)
                {
                    var sday = new ScheduleSumaryDTO() { Date = s.Schedule };
                    var ss = scheduleSettings.First(stt => stt.Day.ToLower() == s.Schedule.ToString("dddd").ToLower());
                    var sh = ss.RangeDates.First(h => h.StarHour == s.Schedule.ToString("HH:mm"));
                    sday.BookedHour = string.Format("{0} - {1}", sh.StarHour, sh.EndHour);
                    resutl.Add(sday);
                }

                return Ok(resutl);

            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, userId, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }

        [HttpGet("{userId}/schedule/{day}")]
        public async Task<ActionResult> ScheduleDay(int userId, string day)
        {
            var resutl = new ScheduleDayDTO() { SelectableHours = new List<HoursData>() };
            try
            {
                // Day debe de estar enformato YYYYMMDD
                if (day.Length != 8)
                    return BadRequest("Formato de Fecha Incorrecto. (YYYYMMDD)");

                var y = Convert.ToInt32(day.Substring(0, 4));
                var m = Convert.ToInt32(day.Substring(4, 2));
                var d = Convert.ToInt32(day.Substring(6));

                if ( y < 2020 || m > 12 || d > 31)
                    return BadRequest("Fecha incorrecta.");

                var date = new DateTime(y, m, d);

                var memberDb = await this._repo.GetMember(null, null, userId);

                if (memberDb == null)
                    return BadRequest("Socio no encontrado.");

                if (!memberDb.User.IsActive)
                    return BadRequest("Socio Bloqueado.");


                var settings = await this._repo.GetGeneralSettings();
                var scheduleSettings = JsonConvert.DeserializeObject<List<ScheduleDaySettings>>(settings.ScheduledWeek);
                var userBookedDay = this._repo.GetUserBookedDay(memberDb.User.Id, date);
                var scheduledDay = this._repo.GetBookedDay(date);
                var currentAuthorizedCapacity = this._repo.GetCurrentAuthorizedCapacity(date);

                resutl.Date = date;
                var ss = scheduleSettings.First(stt => stt.Day.ToLower() == date.ToString("dddd").ToLower());
                
                foreach (HoursRange hr in ss.RangeDates)
                {
                    var hd = new HoursData();

                    hd.Range = string.Format("{0} - {1}", hr.StarHour, hr.EndHour);

                    var time = hr.StarHour.Split(':');
                    var bookedTime = new DateTime(date.Year, date.Month, date.Day, Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0);

                    var sdh = scheduledDay.FirstOrDefault(d => d.Schedule == bookedTime);
                    if (sdh != null)
                    {
                        var availability = currentAuthorizedCapacity.Result - sdh.Count;
                        if (availability < settings.NotificationCapacity)
                            hd.Capacity = availability.ToString();
                        
                        if (userBookedDay != null && hr.StarHour == userBookedDay.Schedule.ToString("HH:mm"))
                            hd.Booked = true;
                    }
                    
                    resutl.SelectableHours.Add(hd);
                }

                return Ok(resutl);

            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, "UserId: " + userId.ToString() + ", day: " + day , ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }
        [HttpPost("{userId}/schedule")]
        public async Task<IActionResult> Reserving([FromBody] ReserveTimeDTO reserveData, int userId)
        {
            try
            {
                var memberDb = await this._repo.GetMember(null, null, userId);
                if (memberDb == null)
                    return BadRequest("Socio no encontrado.");

                if (!memberDb.User.IsActive)
                    return BadRequest("Socio Bloqueado.");

                var settings = await this._repo.GetGeneralSettings();
                var time = reserveData.Hour.Split(':');
                var reserveTime = new DateTime(reserveData.Date.Year, reserveData.Date.Month, reserveData.Date.Day, Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0);

                if (reserveTime > memberDb.MembershipExpiration)
                    return BadRequest("Membrecia Vencida.");

                if (reserveTime < DateTime.Now)
                    return BadRequest("La reservación no puede ser en el pasado");

                var minTimeToReserve = DateTime.Now.AddHours(settings.ScheduleChangeHours);

                if (reserveTime < minTimeToReserve)
                    return BadRequest("Se debe reservar con " + settings.ScheduleChangeHours.ToString() + " horas de anticipación");

                var scheduleSettings = JsonConvert.DeserializeObject<List<ScheduleDaySettings>>(settings.ScheduledWeek);

                var foundHour = false;
                for (int s = 0; s < scheduleSettings.Count; s++)
                {
                    if (scheduleSettings[s].Day.ToLower() == reserveTime.ToString("dddd").ToLower())
                    {
                        for (int h = 0; h < scheduleSettings[s].RangeDates.Count; h++)
                        {
                            if (scheduleSettings[s].RangeDates[h].StarHour == reserveData.Hour)
                            {
                                foundHour = true;
                                break;
                            }
                        }
                    }
                }

                if (!foundHour)
                    return BadRequest("Horario de reservación invalido");


                var authorizedCapacity = await this._repo.GetCurrentAuthorizedCapacity(reserveTime);
                var currentOccupation = this._repo.GetCurrentOccupationHour(reserveTime);

                if (authorizedCapacity <= currentOccupation)
                    return BadRequest("Horario saturado, Favor de seleccionar otra hora.");

                var userBookedDay = this._repo.GetUserBookedDay(memberDb.User.Id, reserveTime);

                var newBooked = new UserScheduling() { Schedule = reserveTime, User = memberDb.User, IsAttended = false };

                this._repo.Add(newBooked);
                if (userBookedDay != null) // (userBookedDay.Result != null)
                    this._repo.Remove(userBookedDay);

                // Validar vigencia?
                // Validar Bloqueo?
                
                // Y Validar el Usuario
                // Y Validar el tiempo permitido para hacer cambios
                // Y Validar que la fecha no sea menor a hoy
                // Y Validar que la Hora de reserva se encuentre en la configuracion.
                // Y Validar la disponibilidad
                // hacer la reservacion
                // Si tiene reservacion para ese mismo dia, eliminarla.
                // Regresar la disponibilidad del dia?



                await this._repo.SaveAll();

                return Ok();

            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, reserveData, ex, "UserId : " + userId.ToString());
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }

        [HttpDelete("{userId}/schedule")]
        public async Task<IActionResult> Removing([FromBody] ReserveTimeDTO reserveData, int userId)
        {
            try
            {
                var memberDb = await this._repo.GetMember(null, null, userId);
                if (memberDb == null)
                    return BadRequest("Socio no encontrado.");


                var time = reserveData.Hour.Split(':');
                var reserveTime = new DateTime(reserveData.Date.Year, reserveData.Date.Month, reserveData.Date.Day, Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0);

                var userBookedDay = this._repo.GetUserBookedDay(memberDb.User.Id, reserveTime);

                if (userBookedDay != null)
                    this._repo.Remove(userBookedDay);

                await this._repo.SaveAll();

                return Ok();

            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, reserveData, ex, "UserId : " + userId.ToString());
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }



    }
}
