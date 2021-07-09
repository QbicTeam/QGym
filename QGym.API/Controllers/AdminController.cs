using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
// using System.Text.Encoding.CodePages;
//using System.Text.Encoding;
using ExcelDataReader;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System.Collections.Generic;
using System.Linq;

using System.Net.Http;

using prometheus.data.securitas;
using prometheus.model.securitas;
using prometheus.dto.securitas;
using prometheus.data.gym;
using prometheus.model.gym;
using AutoMapper;
using prometheus.dto.gym.Capacity;
using Newtonsoft.Json;
using prometheus.dto.gym.Admin;
using prometheus.dto.gym.Members;
using System.Net.Http.Headers;
using System;
using QGym.API.Helpers;
using System.Reflection;
using Payment.DTOs;
using Microsoft.Extensions.Options;
using Payment.Utilities;
using IO = System.IO;
using System.Data;

//using Microsoft.AspNetCore.Authentication;

namespace QGym.API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class AdminController : ControllerBase
    {
        private readonly IGymRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<PaymentSettings> _paymentSettings;

        public AdminController(IGymRepository repo, IConfiguration config, IMapper mapper, IOptions<AppSettings> appSettings, IOptions<PaymentSettings> paymentSettings, IHttpClientHelper httpClientHelper)
        {
            this._repo = repo;
            this._config = config;
            this._mapper = mapper;
            this._appSettings = appSettings;
            this._httpClientHelper = httpClientHelper;
            this._paymentSettings = paymentSettings;
        }

        [HttpGet("settings/{field}")]
        public async Task<ActionResult> GetGeneralSettingField(string field)
        {
            try
            {

                //var result = new AuthorizedCapacityReportDTO();

                var gs = await _repo.GetGeneralSettings();

                if (gs == null) return BadRequest("Contacte al Administrador, sin configuración.");

                var result = gs.GetType().GetProperties().Where(a => a.Name == field).Select(p => p.GetValue(gs, null)).FirstOrDefault().ToString();

                if (result.Substring(0,1) == "{" || result.Substring(0, 1) == "[")
                {
                    if (field == "ScheduledWeek")
                        return Ok(JsonConvert.DeserializeObject<List<ScheduleDaySettings>>(result));
                    
                    if (field == "CovidMsg")
                    {
                        // var msgCovid = new CovidMsg() { PublishDate = "2 Octubre 2020", Title = "Nos vamos a Morir", Message = "Todo lo que se pueda " };
                        return Ok(JsonConvert.DeserializeObject<CovidMsg>(result));
                    }
                        

                    return Ok(JsonConvert.DeserializeObject<dynamic>(result));
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, field, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }

        }
        [HttpGet("settings/general")]
        public async Task<ActionResult> GetGeneralSetting()
        {
            try
            {

                //var result = new AuthorizedCapacityReportDTO();

                var gs = await _repo.GetGeneralSettings();

                if (gs == null) return BadRequest("Contacte al Administrador, sin configuración.");

                var result = this._mapper.Map<GeneralSettingsDTO>(gs);

                return Ok(result);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, "N/A", ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }

        }
        [HttpGet("schedule/weekdays")]
        public async Task<ActionResult> GetWeekdays()
        {
            try
            {
                var result = new List<DateTime>();
                var count = 0;
                var settings = await this._repo.GetGeneralSettings();
                var scheduleSettings = JsonConvert.DeserializeObject<List<ScheduleDaySettings>>(settings.ScheduledWeek);

               
                do
                {
                    var d = DateTime.Today.AddDays(count);
                    var ss = scheduleSettings.FirstOrDefault(stt => stt.Day.ToLower() == d.ToString("dddd").ToLower());

                    if (ss != null)
                        result.Add(d);

                    count++;
                } while (result.Count < 7);


                return Ok(result);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, "N/A", ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }

        }
        [HttpGet("schedule/{day}")]
        public async Task<ActionResult> ScheduleDay(string day)
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

                if (y < 2020 || m > 12 || d > 31)
                    return BadRequest("Fecha incorrecta.");

                var date = new DateTime(y, m, d);

                var settings = await this._repo.GetGeneralSettings();
                var scheduleSettings = JsonConvert.DeserializeObject<List<ScheduleDaySettings>>(settings.ScheduledWeek);
                var scheduledDay = this._repo.GetBookedDay(date);
                var currentAuthorizedCapacity = await this._repo.GetCurrentAuthorizedCapacity(date);

                resutl.Date = date;
                var ss = scheduleSettings.First(stt => stt.Day.ToLower() == date.ToString("dddd").ToLower());

                foreach (HoursRange hr in ss.RangeDates)
                {
                    var hd = new HoursData();

                    hd.Range = string.Format("{0} - {1}", hr.StarHour, hr.EndHour);

                    var time = hr.StarHour.Split(':');
                    var bookedTime = new DateTime(date.Year, date.Month, date.Day, Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0);

                    var sdh = scheduledDay.FirstOrDefault(d => d.Schedule == bookedTime);
                    var availability = currentAuthorizedCapacity - (sdh != null ? sdh.Count : 0);
                    var p = (availability*100) / currentAuthorizedCapacity;
                    //hd.Capacity = string.Format("{0}%/{1}", p, availability.ToString());
                    hd.Capacity = currentAuthorizedCapacity.ToString(); 
                    hd.capPercentaje = p;
                    hd.capPeople = availability;


                    resutl.SelectableHours.Add(hd);
                }

                return Ok(resutl);

            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, day, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }
        [HttpGet("schedule/weekly")]
        public async Task<ActionResult> ScheduleWeekly()
        {
            // var resutl = new ScheduleDayDTO() { SelectableHours = new List<HoursData>() };
            var result = new List<ScheduleDayDTO>();

            var sDay = new ScheduleDayDTO(); // { SelectableHours = new List<HoursData>() };

            try
            {
                /*
                // Day debe de estar enformato YYYYMMDD
                if (day.Length != 8)
                    return BadRequest("Formato de Fecha Incorrecto. (YYYYMMDD)");

                var y = Convert.ToInt32(day.Substring(0, 4));
                var m = Convert.ToInt32(day.Substring(4, 2));
                var d = Convert.ToInt32(day.Substring(6));

                if (y < 2020 || m > 12 || d > 31)
                    return BadRequest("Fecha incorrecta.");

                var date = new DateTime(y, m, d);
                */
                var count = 0;
                var settings = await this._repo.GetGeneralSettings();
                var scheduleSettings = JsonConvert.DeserializeObject<List<ScheduleDaySettings>>(settings.ScheduledWeek);

                /*
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
                    var availability = currentAuthorizedCapacity.Result - (sdh != null ? sdh.Count : 0);
                    var p = (availability * 100) / currentAuthorizedCapacity.Result;
                    hd.Capacity = string.Format("{0}%/{1}", p, availability.ToString());

                    resutl.SelectableHours.Add(hd);
                }
                */

                do
                {
                    sDay = new ScheduleDayDTO() { Date = DateTime.Today.AddDays(count), SelectableHours = new List<HoursData>() };
                    var ss = scheduleSettings.FirstOrDefault(stt => stt.Day.ToLower() == sDay.Date.ToString("dddd").ToLower());

                    if (ss != null)
                    {
                        // var userBookedDay = this._repo.GetUserBookedDay(memberDb.User.Id, sDay.Date);
                        var scheduledDay = this._repo.GetBookedDay(sDay.Date);
                        var currentAuthorizedCapacity = await this._repo.GetCurrentAuthorizedCapacity(sDay.Date);

                        foreach (HoursRange hr in ss.RangeDates)
                        {
                            var hd = new HoursData();

                            hd.Range = string.Format("{0} - {1}", hr.StarHour, hr.EndHour);

                            var time = hr.StarHour.Split(':');
                            var bookedTime = new DateTime(sDay.Date.Year, sDay.Date.Month, sDay.Date.Day, Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0);

                            var sdh = scheduledDay.FirstOrDefault(d => d.Schedule == bookedTime);
                            /*
                            if (sdh != null)
                            {
                                var availability = currentAuthorizedCapacity - sdh.Count;
                                if (availability < settings.NotificationCapacity)
                                    hd.Capacity = availability.ToString();

                                if (userBookedDay != null && hr.StarHour == userBookedDay.Schedule.ToString("HH:mm"))
                                    hd.Booked = true;
                            }

                            sDay.SelectableHours.Add(hd);
                            */
                            var availability = currentAuthorizedCapacity - (sdh != null ? sdh.Count : 0);
                            var p = (availability * 100) / currentAuthorizedCapacity;
                            hd.Capacity = currentAuthorizedCapacity.ToString(); // string.Format("{0}%/{1}", p, availability.ToString());
                            hd.capPercentaje = p;
                            hd.capPeople = availability;

                            sDay.SelectableHours.Add(hd);
                        }

                        result.Add(sDay);
                    }


                    count++;
                } while (result.Count < 7);


                return Ok(result);

            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, "", ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }
        [HttpGet("schedule/{datep}/{time}/capacity")]
        public async Task<ActionResult> CapacityInHour(string datep, string time)
        {
            try
            {
                var result = new CapacityInHourDTO();

                // datetime debe de estar enformato YYYYMMDD-HHmm
                if (datep.Length != 8)
                    return BadRequest("Formato de Fecha Incorrecto. (YYYYMMDD)");

                var y = Convert.ToInt32(datep.Substring(0, 4));
                var m = Convert.ToInt32(datep.Substring(4, 2));
                var d = Convert.ToInt32(datep.Substring(6, 2));

                if (y < 2020 || m > 12 || d > 31)
                    return BadRequest("Fecha incorrecta.");

                if (time.Length != 4)
                    return BadRequest("Formato de Fecha Incorrecto. (HHmm)");

                var HH = Convert.ToInt32(time.Substring(0, 2));
                var mm = Convert.ToInt32(time.Substring(2));

                if (HH > 24 || mm > 59)
                    return BadRequest("Hora incorrecta.");

                var date = new DateTime(y, m, d, HH, mm, 0);

                var ocupationHour = this._repo.GetCurrentOccupationHour(date);
                var currentAuthorizedCapacity = this._repo.GetCurrentAuthorizedCapacity(date);

                result.Capacity = currentAuthorizedCapacity.Result;
                result.Booked = ocupationHour;
                result.Availability = result.Capacity - result.Booked;
                result.BookedPercentage = (result.Booked * 100) / result.Capacity;

                return Ok(result);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, "Date: " + datep + ", Time: " + time, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }
        [HttpGet("schedule/{datep}/{time}/booked/members")]
        public async Task<ActionResult> MembersInHour(string datep, string time)
        {
            try
            {

                // datetime debe de estar enformato YYYYMMDD-HHmm
                if (datep.Length != 8)
                    return BadRequest("Formato de Fecha Incorrecto. (YYYYMMDD)");

                var y = Convert.ToInt32(datep.Substring(0, 4));
                var m = Convert.ToInt32(datep.Substring(4, 2));
                var d = Convert.ToInt32(datep.Substring(6, 2));

                if (y < 2020 || m > 12 || d > 31)
                    return BadRequest("Fecha incorrecta.");

                if (time.Length != 4)
                    return BadRequest("Formato de Fecha Incorrecto. (HHmm)");

                var HH = Convert.ToInt32(time.Substring(0, 2));
                var mm = Convert.ToInt32(time.Substring(2));

                if (HH > 24 || mm > 59)
                    return BadRequest("Hora incorrecta.");

                var date = new DateTime(y, m, d, HH, mm, 0);

                var dataDb = await this._repo.GetMembersBookedInHour(date);

                var result = _mapper.Map<List<Member>, List<MembersDetailsDTO>>(dataDb.ToList());


                return Ok(result);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, "Date: " + datep + ", Time: " + time, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }
        [HttpGet("members/{day}/elegibles")]
        public async Task<ActionResult> MembersElegibles(string day)
        {
            try
            {

                // datetime debe de estar enformato YYYYMMDD
                if (day.Length != 8)
                    return BadRequest("Formato de Fecha Incorrecto. (YYYYMMDD)");

                var y = Convert.ToInt32(day.Substring(0, 4));
                var m = Convert.ToInt32(day.Substring(4, 2));
                var d = Convert.ToInt32(day.Substring(6, 2));

                if (y < 2020 || m > 12 || d > 31)
                    return BadRequest("Fecha incorrecta.");

                var date = new DateTime(y, m, d);

                var dataDb = await this._repo.GetMembersElegibles(date);

                var result = _mapper.Map<List<Member>, List<MembersDetailsDTO>>(dataDb.ToList());


                return Ok(result);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, day, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }

        [HttpPost("payment/{userId}")]
        public async Task<IActionResult> Payment([FromBody] ReserveTimeDTO reserveData, int userId)
        {
            try
            {
                //var token = _httpClientHelper.GetAsync<string>()

                var si = new SignInDto() { Email = this._paymentSettings.Value.UserPayment, Password = this._paymentSettings.Value.PasswordPayment };
                var response = await _httpClientHelper.PostAsync<Result<AuthenticationResult>, SignInDto>(si, this._paymentSettings.Value.AutenticateUri);
                if (!response.IsSuccess)
                {
                    return BadRequest("El Pago no se pudo realizar, contacte al Administrador"); // Error por login con Payment Adalid
                }

                var token = response.Value.Token;



                //response
                return Ok();


            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, reserveData, ex);
                return BadRequest(this._appSettings.Value.ServerError); //this._config.GetSection("AppSettings:ServerError").Value);
            }

        }
        [HttpPut("settings/general")]
        public async Task<ActionResult> UpdateGeneralSetting([FromBody] GeneralSettingsDTO generalSettings)
        {
            try
            {
                if (generalSettings.ScheduleChangeHours < 0)
                    return BadRequest("Valor Invalido, las horas no deben ser negativas");

                var gsDb = await _repo.GetGeneralSettings();

                gsDb.RegistrationCost = generalSettings.RegistrationCost;
                gsDb.ReregistrationCost = generalSettings.ReregistrationCost;
                gsDb.ScheduleChangeHours = generalSettings.ScheduleChangeHours;

                await this._repo.SaveAll();

                return Ok();
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, "N/A", ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }

        }

        [HttpPost("importByFiles")]
        public async Task<IActionResult> ImportFiles(PathFiles paths)
        {
            var restult = new Imported() 
            { 
                MembersImported = new List<MemberImported>(), 
                MembersUpdateVilidity = new List<MemberImported>() 
            };
            try
            {
                string memberFile = string.Empty;
                string validityFile = string.Empty;
                // string filePath = @"C:\Proyectos\QGym_BK\ExcelFiles\Miembros_210301.xlsx";

                // Validate file of Members. (Finding type file member)
                using (var stream = IO.File.Open(paths.Path1, IO.FileMode.Open, IO.FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // 2. Use the AsDataSet extension method
                        var result = reader.AsDataSet();
                        var hoja = result.Tables[0];

                        // var x = hoja.Rows[0].ItemArray[0].ToString().ToUpper();
                        // var x2 = hoja.Rows[0][0].ToString().ToUpper();

                        if (hoja.Rows[0][0].ToString().ToUpper() == "EMPRESA")
                        {
                            memberFile = paths.Path1;
                            validityFile = paths.Path2;
                        }
                        else
                        {
                            memberFile = paths.Path2;
                            validityFile = paths.Path1;
                        }
                    }
                }

                // Get all members
                var membersDb = await this._repo.GetMembersToList();
                var membersListDb = _mapper.Map<List<Member>, List<MemberToListDTO>>(membersDb.ToList());

                // Working with customers
                using (var stream = IO.File.Open(memberFile, IO.FileMode.Open, IO.FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet();
                        var hoja = result.Tables[0];
                        var memberRole = await this._repo.GetRoleByName("Miembro");

                        foreach (DataRow row in hoja.Rows)
                        {
                            if (row[4].ToString().ToUpper() == "CELULAR") // Omitir el primer renglon.
                                continue;

                            var memberId = row[1].ToString(); // Clave Unica (MemberId)
                            var memberFinded  = membersListDb.FirstOrDefault(m => m.MemberId == memberId);

                            if (memberFinded != null)
                                continue;

                            // Add new Member.
                            var userToCreate = new User
                            {
                                UserName = row[6].ToString(), // Email
                                DisplayName = row[3].ToString(), // Nombre
                                Role = memberRole, // Miembro
                                IsActive = true,
                                CreationDate = DateTime.Now,
                                LastModificationDate = DateTime.Now
                            };

                            // Get BirthDate in datetime.
                            var oldStr = row[10].ToString(); // Edad
                            var dayMonStr = row[11].ToString(); // Cumpleaños
                            var day = Convert.ToInt32(dayMonStr.Substring(0,2));
                            var monStr = dayMonStr.Substring(3, 3);
                            int mon = 0;
                            switch (monStr)
                            {
                                case "ene":
                                    mon = 1;
                                    break;
                                case "feb":
                                    mon = 2;
                                    break;
                                case "mar":
                                    mon = 3;
                                    break;
                                case "abr":
                                    mon = 4;
                                    break;
                                case "may":
                                    mon = 5;
                                    break;
                                case "jun":
                                    mon = 6;
                                    break;
                                case "jul":
                                    mon = 7;
                                    break;
                                case "ago":
                                    mon = 8;
                                    break;
                                case "sep":
                                    mon = 9;
                                    break;
                                case "oct":
                                    mon = 10;
                                    break;
                                case "nov":
                                    mon = 11;
                                    break;
                                case "dic":
                                    mon = 12;
                                    break;
                            }
                            var year = DateTime.Now.AddYears(Convert.ToInt32(oldStr) * -1).Year;
                            var birthdate = new DateTime(year, mon, day, 0, 0, 0);

                            var memberToCreate = new Member() { 
                                User = userToCreate,
                                CellPhone = row[4].ToString(),
                                Phone = row[5].ToString(),
                                Birthdate = birthdate
                            };

                            if (string.IsNullOrEmpty(userToCreate.UserName))
                                userToCreate.UserName = row[1].ToString();


                            // Create a Member
                            var memberCreated = await this._repo.Register(memberToCreate, memberId);

                            var memImp = new MemberImported() { MemberId = memberId, Name = memberCreated.User.DisplayName };
                            restult.MembersImported.Add(memImp);
                        }
                    }
                }

                // Working with validity.
                using (var stream = IO.File.Open(validityFile, IO.FileMode.Open, IO.FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet();
                        var hoja = result.Tables[0];

                        foreach (DataRow row in hoja.Rows)
                        {
                            var encabezado = row[0].ToString().ToUpper().Substring(0, 3);
                            if (encabezado == "MEM" || encabezado == "CLA") // Omitir el primer renglon.
                                continue;

                            var memberId = row[0].ToString(); // Clave Unica (MemberId)
                            var memDb = await this._repo.GetMember(null, memberId, 0);

                            if (memDb == null)
                                continue;

                            var venc = DateTime.Parse(row[4].ToString());
                            if (memDb.MembershipExpiration < venc)
                            {
                                memDb.MembershipExpiration = venc;
                                await this._repo.SaveAll();
                                var memUp = new MemberImported() { MemberId = memDb.MemberId, Name = memDb.User.DisplayName, MembershipExpiration = memDb.MembershipExpiration };
                                restult.MembersUpdateVilidity.Add(memUp);
                            }
                        }
                    }
                }
                
                return Ok(restult);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, paths, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }
    }
}
