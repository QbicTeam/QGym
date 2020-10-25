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
using Microsoft.Extensions.Options;
using prometheus.dto.gym.Sales;

using System.IO;

namespace QGym.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SalesController : Controller
    {
        private readonly IGymRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _appSettings;

        public SalesController(IGymRepository repo, IConfiguration config, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            this._repo = repo;
            this._config = config;
            this._mapper = mapper;
            this._appSettings = appSettings;
        }

        [HttpGet("report/{startDay}/{endDay}")] // report/{fechaI-FechaF}
        public async Task<ActionResult> GetPayments(string startDay, string endDay)  
        {
            try
            {
                var memberDb = await this._repo.GetMember(null, null, 4);
                var x = 0;
                if (string.IsNullOrEmpty(startDay) && string.IsNullOrEmpty(endDay))
                    x = 1;
                
                /*
                var result = new List<PayReportDto>();
                //var packages = await _repo.GetPackages();
                //var result = _mapper.Map<List<MembershipType>, List<MembershipTypeDTO>>(packages.ToList());

                var payFake = new PayReportDto() {
                    SaleDate = DateTime.Today.AddDays(-1),
                    FullName = "Nombre del Socio 1", 
                    MembershipType = "Familiar", 
                    MemberId = "12345",
                    Period = "7 días",
                    Vigency = DateTime.Today.AddDays(6), 
                    Price = 520, 
                    ReferntPayment = "TqD453", 
                    Birthdate = DateTime.Today.AddYears(-24), 
                    Gender = "Hombre", 
                    Phone = "664 854 8965"
                };
                result.Add(payFake);

                payFake = new PayReportDto()
                {
                    SaleDate = DateTime.Today.AddDays(-2),
                    FullName = "Nombre del Socio 3",
                    MembershipType = "Acceso Total",
                    MemberId = "987654",
                    Period = "60 días",
                    Vigency = DateTime.Today.AddDays(58),
                    Price = 1250,
                    ReferntPayment = "XWQ846",
                    Birthdate = DateTime.Today.AddYears(-34),
                    Gender = "Mujer",
                    Phone = "664 854 1256"
                };
                result.Add(payFake);

                payFake = new PayReportDto()
                {
                    SaleDate = DateTime.Today,
                    FullName = "Nombre del Socio 10",
                    MembershipType = "Lady Fitnets",
                    MemberId = "759682",
                    Period = "30 días",
                    Vigency = DateTime.Today.AddDays(35),
                    Price = 650,
                    ReferntPayment = "POX861",
                    Birthdate = DateTime.Today.AddYears(-18),
                    Gender = "Mujer",
                    Phone = "664 956 3514"
                };
                result.Add(payFake);

                payFake = new PayReportDto()
                {
                    SaleDate = DateTime.Today.AddDays(-1),
                    FullName = "Nombre del Socio 5",
                    MembershipType = "Estudiante",
                    MemberId = "627453",
                    Period = "30 días",
                    Vigency = DateTime.Today.AddDays(32),
                    Price = 310,
                    ReferntPayment = "JDP682",
                    Birthdate = DateTime.Today.AddYears(-19),
                    Gender = "Hombre",
                    Phone = "663 761 3392"
                };
                result.Add(payFake);

                payFake = new PayReportDto()
                {
                    SaleDate = DateTime.Today.AddDays(-1),
                    FullName = "Nombre del Socio 14",
                    MembershipType = "Ejecutivo",
                    MemberId = "495127",
                    Period = "60 días",
                    Vigency = DateTime.Today.AddDays(67),
                    Price = 1500,
                    ReferntPayment = "KLT927",
                    Birthdate = DateTime.Today.AddYears(-39),
                    Gender = "Hombre",
                    Phone = "663 926 4627"
                };
                result.Add(payFake);
                */

                return Ok(FakeReportePayment());

            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, "N/A", ex);
                return BadRequest(this._appSettings.Value.ServerError);
            }

        }


        [HttpGet("report/{startDay}/{endDay}/download")]
        public async Task<FileResult> GetPaymentsReportDonwload(string startDay, string endDay) 
        {
            try
            {
                var memberDb = await this._repo.GetMember(null, null, 4);

                var x = 0;
                if (string.IsNullOrEmpty(startDay) && string.IsNullOrEmpty(endDay))
                    x = 1;

                // TODO: Generar el archivo CSV


                string fileName = "RepoteTest"+ DateTime.Now.ToString("yyyyMMdd-HHmmss") +".csv";
                
                var data = FakeReportePayment();

                var builder = new StringBuilder();
                builder.AppendLine("Fecha Venta, Cliente, Paquete, Membresia, Periodo, Fin Vigencia, Precio, Referencia Pago, Cumpleaños, Sexo, Telefono");
                foreach (var rep in data)
                {
                    builder.AppendLine($"{rep.SaleDate},{rep.FullName},{rep.MembershipType},{rep.MemberId},{rep.Period},{rep.Vigency},{rep.Price},{rep.ReferntPayment},{rep.Birthdate},{rep.Gender},{rep.Phone}");
                }

                return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", fileName);
                



            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, "Startday: " + startDay + ", Enddate: " + endDay , ex);
                //return BadRequest(this._appSettings.Value.ServerError);
                return null;
            }

        }

        private static List<PayReportDto> FakeReportePayment()
        {
            var result = new List<PayReportDto>();
            var payFake = new PayReportDto()
            {
                SaleDate = DateTime.Today.AddDays(-1),
                FullName = "Nombre del Socio 1",
                MembershipType = "Familiar",
                MemberId = "12345",
                Period = "7 días",
                Vigency = DateTime.Today.AddDays(6),
                Price = 520,
                ReferntPayment = "TqD453",
                Birthdate = DateTime.Today.AddYears(-24),
                Gender = "Hombre",
                Phone = "664 854 8965"
            };
            result.Add(payFake);

            payFake = new PayReportDto()
            {
                SaleDate = DateTime.Today.AddDays(-2),
                FullName = "Nombre del Socio 3",
                MembershipType = "Acceso Total",
                MemberId = "987654",
                Period = "60 días",
                Vigency = DateTime.Today.AddDays(58),
                Price = 1250,
                ReferntPayment = "XWQ846",
                Birthdate = DateTime.Today.AddYears(-34),
                Gender = "Mujer",
                Phone = "664 854 1256"
            };
            result.Add(payFake);

            payFake = new PayReportDto()
            {
                SaleDate = DateTime.Today,
                FullName = "Nombre del Socio 10",
                MembershipType = "Lady Fitnets",
                MemberId = "759682",
                Period = "30 días",
                Vigency = DateTime.Today.AddDays(35),
                Price = 650,
                ReferntPayment = "POX861",
                Birthdate = DateTime.Today.AddYears(-18),
                Gender = "Mujer",
                Phone = "664 956 3514"
            };
            result.Add(payFake);

            payFake = new PayReportDto()
            {
                SaleDate = DateTime.Today.AddDays(-1),
                FullName = "Nombre del Socio 5",
                MembershipType = "Estudiante",
                MemberId = "627453",
                Period = "30 días",
                Vigency = DateTime.Today.AddDays(32),
                Price = 310,
                ReferntPayment = "JDP682",
                Birthdate = DateTime.Today.AddYears(-19),
                Gender = "Hombre",
                Phone = "663 761 3392"
            };
            result.Add(payFake);

            payFake = new PayReportDto()
            {
                SaleDate = DateTime.Today.AddDays(-1),
                FullName = "Nombre del Socio 14",
                MembershipType = "Ejecutivo",
                MemberId = "495127",
                Period = "60 días",
                Vigency = DateTime.Today.AddDays(67),
                Price = 1500,
                ReferntPayment = "KLT927",
                Birthdate = DateTime.Today.AddYears(-39),
                Gender = "Hombre",
                Phone = "663 926 4627"
            };
            result.Add(payFake);


            return result;
        }
    }
}
