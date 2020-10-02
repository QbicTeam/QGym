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

namespace QGym.API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class ScheduleController : Controller
    {
        private readonly IGymRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public ScheduleController(IGymRepository repo, IConfiguration config, IMapper mapper)
        {
            this._repo = repo;
            this._config = config;
            this._mapper = mapper;
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
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
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
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }
        }

    }
}
