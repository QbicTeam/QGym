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
//using Payment.Utilities;
using prometheus.dto.gym.Payment;

namespace QGym.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IGymRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<PaymentSettings> _paymentSettings;

        public PaymentController(IGymRepository repo, IConfiguration config, IMapper mapper, IOptions<AppSettings> appSettings, IOptions<PaymentSettings> paymentSettings, IHttpClientHelper httpClientHelper)
        {
            this._repo = repo;
            this._config = config;
            this._mapper = mapper;
            this._appSettings = appSettings;
            this._httpClientHelper = httpClientHelper;
            this._paymentSettings = paymentSettings;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> Payment([FromBody] PaymentRequestDto paymentRq, int userId)
        {
            try
            {

                var memberDb = await this._repo.GetMember(null, null, userId);
                if (memberDb == null)
                    return NotFound("El Usuario no esta registrado");

                var packageDb = await this._repo.GetMembershipById(paymentRq.MemberschipTypeId);
                if (packageDb == null)
                    return NotFound("El Paquete no esta registrado");

                if (!packageDb.IsActive)
                    return NotFound("El Paquete no esta Activo");

                if (packageDb.Price > paymentRq.Amount)
                    return NotFound("El monto pagado es menor al del paquete.");


                /*
                //var token = _httpClientHelper.GetAsync<string>()

                var si = new SignInDto() { Email = this._paymentSettings.Value.UserPayment, Password = this._paymentSettings.Value.PasswordPayment };
                var response = await _httpClientHelper.PostAsync<Result<AuthenticationResult>, SignInDto>(si, this._paymentSettings.Value.AutenticateUri);
                if (!response.IsSuccess)
                {
                    return BadRequest("El Pago no se pudo realizar, contacte al Administrador"); // Error por login con Payment Adalid
                }

                var token = response.Value.Token;
                */

                if (memberDb.MembershipExpiration > DateTime.Today)
                    memberDb.MembershipExpiration = memberDb.MembershipExpiration.AddDays(packageDb.PeriodicityDays);
                else
                    memberDb.MembershipExpiration = DateTime.Today.AddDays(packageDb.PeriodicityDays);

                memberDb.MembershipTypeActive = packageDb;

                await this._repo.SaveAll();

                //response
                return Ok();


            }
            catch (Exception ex)
            {
                new FileManagerHelper().RecordLogFile(MethodBase.GetCurrentMethod().ReflectedType.FullName, paymentRq, ex, "UserId: " + userId.ToString());
                return BadRequest(this._appSettings.Value.ServerError); 
            }

        }

    }
}
