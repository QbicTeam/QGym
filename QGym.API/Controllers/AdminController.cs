using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
    }
}
