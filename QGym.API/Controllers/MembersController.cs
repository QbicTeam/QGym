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
using prometheus.dto.gym.Members;

namespace QGym.API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class MembersController : ControllerBase
    {
        private readonly IGymRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public MembersController(IGymRepository repo, IConfiguration config, IMapper mapper)
        {
            this._repo = repo;
            this._config = config;
            this._mapper = mapper;
        }

        [HttpGet()]
        public async Task<ActionResult> GetMembars()
        {
            try
            {

                var dataDb = await this._repo.GetMembersToList();  
                //USO: List<Destino> personViews = Mapper.Map<List<Origen>, List<Destino>>(people);
      
                var result = _mapper.Map<List<Member>, List<MemberToListDTO>>(dataDb.ToList());


                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }

        }

        [HttpGet("{type}")]
        public async Task<ActionResult> GetMembarsType(string type)
        {
            try
            {
                if (type != "valid" && type != "active")
                    return BadRequest("Petición invalida");

                var dataDb = await this._repo.GetMembersType(type);

                var result = _mapper.Map<List<Member>, List<MemberToListDTO>>(dataDb.ToList());


                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }

        }

        [HttpGet("{memberId}/details/forBlock")]
        public async Task<ActionResult> GetMembarsForBlock(string memberId)
        {
            try
            {
                var dataDb = await this._repo.GetMember("", memberId);

                var result = _mapper.Map<MemberForBlockDTO>(dataDb);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }

        }

        [HttpGet("{memberId}/details/complete")]
        public async Task<ActionResult> GetMembarsFull(string memberId)
        {
            try
            {
                var dataDb = await this._repo.GetMember("", memberId);

                var result = _mapper.Map<MemberDTO>(dataDb);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }

        }
        
        [HttpPut("{memberId}/status/{isBlock}")]
        public async Task<IActionResult> UpdatingStatus(string memberId, int isBlock)
        {
            try
            {

                var user = await this._repo.GetMember("", memberId);

                user.User.IsActive = isBlock == 1? false : true;

                await this._repo.SaveAll();
                
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(this._config.GetSection("AppSettings:ServerError").Value);
            }
        }

        [HttpPut("{memberId}")]
        public async Task<IActionResult> Updating([FromBody] MemberDTO member, string memberId)
        {
            try
            {
                var memberDb = await this._repo.GetMember("", memberId);


                if(!string.IsNullOrEmpty(member.MemberId) && string.IsNullOrEmpty(memberDb.MemberId))
                    memberDb.MemberId = member.MemberId;

                if (!string.IsNullOrEmpty(member.PhotoUrl) && string.IsNullOrEmpty(memberDb.PhotoUrl))
                    memberDb.PhotoUrl = member.PhotoUrl;

                if (!string.IsNullOrEmpty(member.PhotoUrl) && memberDb.PhotoUrl != member.PhotoUrl)
                    memberDb.PhotoUrl = member.PhotoUrl;

                if (member.MembershipExpiration > memberDb.MembershipExpiration)
                    memberDb.MembershipExpiration = member.MembershipExpiration;

                if (memberDb.MembershipTypeActiveId == null && memberDb.MembershipTypeActiveId > 0)
                {
                    var membership = await this._repo.GetMembershipById(member.MembershipTypeActiveId.Value);
                    memberDb.MembershipTypeActive = membership;
                }

                if (member.IsVerified)
                    memberDb.IsVerified = true;

                if (!string.IsNullOrEmpty(member.Email))
                    memberDb.User.UserName = member.Email;

                if (!string.IsNullOrEmpty(member.DisplayName))
                    memberDb.User.DisplayName = member.DisplayName;

                if (!string.IsNullOrEmpty(member.RoleName) && member.RoleName != memberDb.User.Role.DisplayName)
                {
                    var role = await this._repo.GetRoleByName(member.RoleName);
                    memberDb.User.Role = role;
                }

                memberDb.User.IsActive = !member.IsBlock;

                if (!string.IsNullOrEmpty(member.BlockingReason))
                    memberDb.BlockingReason = member.BlockingReason;


                memberDb.User.LastModificationDate = DateTime.Today;




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
