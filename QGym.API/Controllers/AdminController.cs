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
using prometheus.dto.gym.Capacity;
using Newtonsoft.Json;
using prometheus.dto.gym.Admin;
using prometheus.dto.gym.Members;

namespace QGym.API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class AdminController : ControllerBase
    {
        private readonly IGymRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AdminController(IGymRepository repo, IConfiguration config, IMapper mapper)
        {
            this._repo = repo;
            this._config = config;
            this._mapper = mapper;
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

                if (result.Substring(1,1) == "{" || result.Substring(1, 1) == "[")
                {
                    if (field == "ScheduledWeek")
                        return Ok(JsonConvert.DeserializeObject<List<ScheduleDaySettings>>(result));

                    return Ok(JsonConvert.DeserializeObject<dynamic>(result));
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
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
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
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
                    var p = (availability*100) / currentAuthorizedCapacity.Result;
                    hd.Capacity = string.Format("{0}%/{1}", p, availability.ToString());

                    resutl.SelectableHours.Add(hd);
                }

                return Ok(resutl);

            }
            catch (Exception ex)
            {
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }
        }
        [HttpGet("schedule/{datetime}/capacity")]
        public async Task<ActionResult> CapacityInHour(string datetime)
        {
            var result = new CapacityInHourDTO();

            // datetime debe de estar enformato YYYYMMDD-HHmm
            if (datetime.Length != 13)
                return BadRequest("Formato de Fecha Incorrecto. (YYYYMMDD-HHmm)");

            var y = Convert.ToInt32(datetime.Substring(0, 4));
            var m = Convert.ToInt32(datetime.Substring(4, 2));
            var d = Convert.ToInt32(datetime.Substring(6,2));

            if (y < 2020 || m > 12 || d > 31)
                return BadRequest("Fecha incorrecta.");

            var HH = Convert.ToInt32(datetime.Substring(9, 2));
            var mm = Convert.ToInt32(datetime.Substring(11));

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
        [HttpGet("schedule/{datetime}/booked/members")]
        public async Task<ActionResult> MembersInHour(string datetime)
        {
            try
            {

                // datetime debe de estar enformato YYYYMMDD-HHmm
                if (datetime.Length != 13)
                    return BadRequest("Formato de Fecha Incorrecto. (YYYYMMDD-HHmm)");

                var y = Convert.ToInt32(datetime.Substring(0, 4));
                var m = Convert.ToInt32(datetime.Substring(4, 2));
                var d = Convert.ToInt32(datetime.Substring(6, 2));

                if (y < 2020 || m > 12 || d > 31)
                    return BadRequest("Fecha incorrecta.");

                var HH = Convert.ToInt32(datetime.Substring(9, 2));
                var mm = Convert.ToInt32(datetime.Substring(11));

                if (HH > 24 || mm > 59)
                    return BadRequest("Hora incorrecta.");

                var date = new DateTime(y, m, d, HH, mm, 0);

                var dataDb = await this._repo.GetMembersBookedInHour(date);

                var result = _mapper.Map<List<Member>, List<MembersDetailsDTO>>(dataDb.ToList());


                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
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
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }
        }
    }
}
