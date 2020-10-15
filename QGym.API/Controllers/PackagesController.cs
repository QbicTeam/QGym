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
using prometheus.dto.gym.Membership;
using prometheus.dto.gym.Packages;
using System.Reflection;
using QGym.API.Helpers;

namespace QGym.API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class PackagesController : Controller
    {
        private readonly IGymRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public PackagesController(IGymRepository repo, IConfiguration config, IMapper mapper)
        {
            this._repo = repo;
            this._config = config;
            this._mapper = mapper;
        }

        [HttpGet()]
        public async Task<ActionResult> GetPackages()
        {
            try
            {
                
                var packages = await _repo.GetPackages();
                var result = _mapper.Map<List<MembershipType>, List<MembershipTypeDTO>>(packages.ToList());


                return Ok(result);
                
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, "N/A", ex);
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }

        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetPackages(int id)
        {
            try
            {

                var package = await _repo.GetMembershipById(id);
                var result = _mapper.Map<MembershipTypeFullDTO>(package);

                return Ok(result);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, id, ex);
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }

        }
        [HttpGet("actives")]
        public async Task<ActionResult> GetActivesPackages()
        {
            try
            {

                var packages = await _repo.GetActivePackages();
                var result = _mapper.Map<List<MembershipType>, List<ActivePackageDTO>>(packages.ToList());
                

                return Ok(result);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, "N/A", ex);
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatingPackages([FromBody] MembershipTypeForCreateDTO packageRq, int id)
        {
            try
            {

                var packageDb = await _repo.GetMembershipById(id);

                if (!string.IsNullOrEmpty(packageRq.Code))
                    packageDb.Code = packageRq.Code;

                if (!string.IsNullOrEmpty(packageRq.Name))
                    packageDb.Name = packageRq.Name;

                if (packageRq.PeriodicityDays > 0)
                    packageDb.PeriodicityDays = packageRq.PeriodicityDays;

                if (packageRq.Price > 0)
                    packageDb.Price = packageRq.Price;

                if (!string.IsNullOrEmpty(packageRq.ShortDescription))
                    packageDb.ShortDescription = packageRq.ShortDescription;

                if (!string.IsNullOrEmpty(packageRq.LongDescription))
                    packageDb.LongDescription = packageRq.LongDescription;

                packageDb.IsActive = packageRq.IsActive;

                await this._repo.SaveAll();

                return Ok();
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, packageRq, ex);
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> SavePackages([FromBody] MembershipTypeForCreateDTO packageRq)
        {
            try
            {

                if (string.IsNullOrEmpty(packageRq.Code))
                    return BadRequest("Valor Invalido, Codigo no enviado");

                if (string.IsNullOrEmpty(packageRq.Name))
                    return BadRequest("Valor Invalido, Nombre no enviado");

                if (packageRq.IsActive && packageRq.PeriodicityDays <= 0)
                    return BadRequest("Valor Invalido, Tiempo de vigencia");

                if (packageRq.IsActive && packageRq.Price <= 0)
                    return BadRequest("Valor Invalido, Precio");

                if (packageRq.IsActive && string.IsNullOrEmpty(packageRq.ShortDescription))
                    return BadRequest("Valor Invalido, Descripcion corta HTML");

                if (packageRq.IsActive && string.IsNullOrEmpty(packageRq.LongDescription))
                    return BadRequest("Valor Invalido, Descripcion larga HTML");

                var newPackage = this._mapper.Map<MembershipType>(packageRq);

                this._repo.Add(newPackage);

                await this._repo.SaveAll();


                return Ok(newPackage.Id);
            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, packageRq, ex);
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }
        }


    }
}
