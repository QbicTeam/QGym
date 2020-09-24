using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;
// using QGym.API.DTO;
using prometheus.dto.securitas;
// using QGym.API.Data;
using prometheus.dto.gym;
using prometheus.model.gym;

//using QGym.API.Model;
/*
using Framework.DataTypes.Model.Base;
using Framework.DataTypes.Model.Licenciamiento;
using Framework.DataTypes.Model.Infraestructura;
*/

// using Framework.Helpers.Infrastructure;
// using Framework.Helpers;

//  prometheus.model.securitas
//  prometheus.model.gym

//  prometheus.dto.securitas
//  prometheus.dto.gym

//  prometheus.data.securitas
//  prometheus.data.gym

using prometheus.data.gym;
using prometheus.model.gym;
using prometheus.dto.gym;

namespace QGym.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class QGymController: ControllerBase
    {
        
        private readonly IGymRepository _repo;
        private readonly IMapper _mapper;

        public QGymController(IGymRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
/*
        [HttpPost("clientes")]
        public async Task<IActionResult> SaveCliente([FromBody]ClienteParaRegistroDTO cliente)
        {

            var cli = _mapper.Map<Cliente>(cliente);
            //rvw.Created = DateTime.Now;
            var paq = await _repo.GetPaquete(cliente.PaqueteId);
            cli.Licencia = new Licencia() {
                PaqueteInicial = paq,
                CostoInicial = paq.Costo,
                NumUsuariosTotal = paq.NumUsuarios,
                NumNegociosTotal = paq.NumNegocios,
                // Apps = _mapper.Map<ICollection<LicenciaApp>>(paq.Apps), //paq.Apps,
                Apps = _mapper.Map<ICollection<PaqueteApp>, ICollection<LicenciaApp>>(paq.Apps), 
                CostoTotalActual = paq.Costo,
                FechaAlta = DateTime.Now
            };
            //cli.Licencia.Apps = _mapper.Map<ICollection<LicenciaApp>>(paq.Apps);

            var cliAct = new ClienteActualizacion() {
                Tipo = 1,
                Cliente = cli,
                // Apps = _mapper.Map<ICollection<ClienteActualizacionApp>>(paq.Apps), //paq.Apps,
                Apps = _mapper.Map<ICollection<PaqueteApp>, ICollection<ClienteActualizacionApp>>(paq.Apps), 
                Fecha = DateTime.Now,
                Status = 1

            };


            _repo.Add<Cliente>(cli);
            _repo.Add<ClienteActualizacion>(cliAct);

            if (await _repo.SaveAll())
            {
                return  Ok();

            }
                
            return BadRequest("Not Saved");
        }

        [HttpGet("paquetes")]
        public async Task<IActionResult> GetPaquetes()
        {
            var result = await _repo.GetPaquetes();
            var resultDTO = _mapper.Map<IEnumerable<PaqueteParaLista>>(result);

            return Ok(resultDTO);
        }
*/
        
    }
}