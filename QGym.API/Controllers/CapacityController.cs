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
using System.Reflection;
using QGym.API.Helpers;
using Microsoft.Extensions.Options;

namespace QGym.API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class CapacityController : ControllerBase
    {
        private readonly IGymRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _appSettings;

        public CapacityController(IGymRepository repo, IConfiguration config, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            this._repo = repo;
            this._config = config;
            this._mapper = mapper;
            this._appSettings = appSettings;
        }

        [HttpGet()]
        public async Task<ActionResult> GetCapacity()
        {
            try
            {

                var result = new AuthorizedCapacityReportDTO();

                var tc = await _repo.GetGeneralSettings();
                result.TotalCapacity = tc.TotalCapacity;

                var autorizedCapasityList = await _repo.GetAuthorizedCapacityToList();
                result.AuthorizedCapacities = _mapper.Map<List<AuthorizedCapacity>, List<AuthorizedCapacityDTO>>(autorizedCapasityList.ToList());  

                return Ok(result);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, "N/A", ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }

        }

        
        [HttpGet("autorizedCapacities")]
        public async Task<ActionResult> GetCapacities()
        {
            try
            {

                var autorizedCapasityList = await _repo.GetAuthorizedCapacityToList();
                var result = _mapper.Map<List<AuthorizedCapacity>, List<AuthorizedCapacityDTO>>(autorizedCapasityList.ToList());

                return Ok(result);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, "N/A", ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }

        }

        [HttpPut("{totalCapacity}")]
        public async Task<IActionResult> UpdatingCapacity(int totalCapacity)
        {
            try
            {
                if (totalCapacity <= 0)
                    return BadRequest("Valor Invalido, la Capacidad debe ser Mayor a Cero");

                var tc = await _repo.GetGeneralSettings();
                tc.TotalCapacity = totalCapacity;

                await this._repo.SaveAll();

                return Ok();
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, totalCapacity, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }

        [HttpPost("autorizedCapacities")]
        public async Task<IActionResult> SaveCapacities(AuthorizedCapacityForCreateDTO autCapacity)
        {
            try
            {
                
                if (autCapacity.Capacity <= 0)
                    return BadRequest("Valor Invalido, la Capacidad debe ser mayor a Cero");
                
                if (autCapacity.PercentageCapacity <= 0 || autCapacity.PercentageCapacity > 100)
                    return BadRequest("Valor Invalido, el Porcentaje debe ser entre 1 y 100");
                
                if (autCapacity.StartDate <= DateTime.Today)
                    return BadRequest("Valor Invalido, Fecha de Inicio es menor a Hoy");
                
                if (autCapacity.EndDate <= autCapacity.StartDate)
                    return BadRequest("Valor Invalido, Fecha Final debe ser Mayor a la Fecha Incial");

                var newAutCapacity = _mapper.Map<AuthorizedCapacity>(autCapacity);

                this._repo.Add(newAutCapacity);
                await this._repo.SaveAll();

                return Ok();
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, autCapacity, ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }
        }



    }
}
