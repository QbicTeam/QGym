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
using System.Reflection;
using QGym.API.Helpers;
using Microsoft.Extensions.Options;

namespace QGym.API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class ScheduleController : Controller
    {
        private readonly IGymRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _appSettings;

        public ScheduleController(IGymRepository repo, IConfiguration config, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            this._repo = repo;
            this._config = config;
            this._mapper = mapper;
            this._appSettings = appSettings;
        }

        [HttpGet()]
        public async Task<ActionResult> GetSchedule()
        {
            try
            {

                var tc = await _repo.GetGeneralSettings();

                return Ok(tc.ScheduleChangeHours);
                
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, "N/A", ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }

        }

        [HttpPut("{hours}")]
        public async Task<IActionResult> UpdatingSchedule(int hours)
        {
            try
            {

                if (hours < 0)
                    return BadRequest("Valor Invalido, las horas no deben ser negativas");

                var tc = await _repo.GetGeneralSettings();
                tc.ScheduleChangeHours = hours;

                await this._repo.SaveAll();

                return Ok();
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, hours, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }

    }
}
