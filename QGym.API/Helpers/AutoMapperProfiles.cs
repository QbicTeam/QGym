using System;
using AutoMapper;
using System.Collections.Generic;

using System.Linq;

using prometheus.model.securitas;
using prometheus.dto.securitas;
using prometheus.model.gym;
using prometheus.dto.gym;
using prometheus.dto.gym.Members;
using prometheus.dto.gym.Capacity;
using prometheus.dto.gym.Membership;
using prometheus.dto.gym.Admin;
using prometheus.dto.gym.Packages;

namespace QGym.API.Helpers
{
    public class AutoMapperProfiles: Profile 
    {
        public AutoMapperProfiles()
        {
            /*
            CreateMap<Origen, Destino>()
                .ForMember(dest => dest.PropDest, opt => {
                    opt.MapFrom(src => src.[PropOrg.FirstOrDefault(p => p.Condicion)].PropOrgFinal);
                })
                 .ForMember(la => la.Id, opt => opt.Ignore()) // Para ignorar una propiedad.
            .BeforeMap( (s, d) => d.Propiedad = "valor directo"); // Para Inicializar un valor

                  USO: var myVar = _mapper.Map<destino>(Origen);

            Ejemplo Listas e Ignore
            Mapper.CreateMap<Person, PersonView>();
            Mapper.CreateMap<PersonView, Person>()
                .ForMember(person => person.Id, opt => opt.Ignore());
                 USO:    List<PersonView> personViews =  Mapper.Map<List<Person>, List<PersonView>>(people);
            */
            /*
            CreateMap<ClienteParaRegistroDTO, Cliente>();
            CreateMap<ClienteActualizacion, ActualizacionClienteParaListaDTO>()
                .ForMember(dest => dest.NombreCliente, opt => {
                     opt.MapFrom(src => src.Cliente.NomEmpresa);
                 })
                 .ForMember(dest => dest.ActualizacionId, opt => {
                     opt.MapFrom(src => src.Id);
                 });
            */
            // MemberToListDTO
            CreateMap<Member, MemberToListDTO>()
                .ForMember(dest => dest.Email, opt => {
                    opt.MapFrom(src => src.User.UserName);
                })
                .ForMember(dest => dest.FullName, opt => {
                    opt.MapFrom(src => src.User.DisplayName);
                })
                .ForMember(dest => dest.SearchText, opt => {
                    opt.MapFrom(src => src.MemberId + " " + src.User.DisplayName + " " + src.User.UserName);
                })
                .ForMember(dest => dest.UserId, opt => {
                    opt.MapFrom(src => src.User.Id);
                });

            CreateMap<Member, MemberForBlockDTO>()
                .ForMember(dest => dest.Email, opt => {
                    opt.MapFrom(src => src.User.UserName);
                })
                .ForMember(dest => dest.IsBlock, opt => {
                    opt.MapFrom(src => !src.User.IsActive);
                })
                .ForMember(dest => dest.FullName, opt => {
                    opt.MapFrom(src => src.User.DisplayName);
                })
                .ForMember(dest => dest.UserId, opt => {
                    opt.MapFrom(src => src.User.Id);
                });

            CreateMap<Member, MemberDTO>()
                .ForMember(dest => dest.Email, opt => {
                    opt.MapFrom(src => src.User.UserName);
                })
                .ForMember(dest => dest.IsBlock, opt => {
                    opt.MapFrom(src => !src.User.IsActive);
                })
                .ForMember(dest => dest.FullName, opt => {
                    opt.MapFrom(src => src.User.DisplayName);
                })
                .ForMember(dest => dest.CreationDate, opt => {
                    opt.MapFrom(src => src.User.CreationDate);
                })
                .ForMember(dest => dest.LastModificationDate, opt => {
                    opt.MapFrom(src => src.User.LastModificationDate);
                })
                .ForMember(dest => dest.RoleName, opt => {
                    opt.MapFrom(src => src.User.Role.DisplayName);
                })
                .ForMember(dest => dest.UserId, opt => {
                    opt.MapFrom(src => src.User.Id);
                });
            
            CreateMap<Member, MembersDetailsDTO>()
                .ForMember(dest => dest.Email, opt => {
                    opt.MapFrom(src => src.User.UserName);
                })
                .ForMember(dest => dest.FullName, opt => {
                    opt.MapFrom(src => src.User.DisplayName);
                })
                .ForMember(dest => dest.Package, opt => {
                    opt.MapFrom(src => src.MembershipTypeActive.Name);
                })
                .ForMember(dest => dest.Period, opt => {
                    opt.MapFrom(src => (src.MembershipExpiration - DateTime.Today).TotalDays.ToString("N0") + " Días");
                })
                .ForMember(dest => dest.DueDate, opt => {
                    opt.MapFrom(src => src.MembershipExpiration);
                })
                .ForMember(dest => dest.UserId, opt => {
                    opt.MapFrom(src => src.User.Id);
                });

            CreateMap<AuthorizedCapacity, AuthorizedCapacityDTO>();
            CreateMap<AuthorizedCapacityForCreateDTO, AuthorizedCapacity>();

            CreateMap<MembershipType, MembershipTypeDTO>();
            CreateMap<MembershipType, MembershipTypeFullDTO>();
            CreateMap<MembershipTypeForCreateDTO, MembershipType>();

            CreateMap<MembershipType, ActivePackageDTO>()
                .ForMember(dest => dest.Period, opt => {
                    opt.MapFrom(src => src.PeriodicityDays.ToString() + " Días");
                });

            CreateMap<GeneralSettings, GeneralSettingsDTO>();
            CreateMap<GeneralSettingsDTO, GeneralSettings>();
        }

    }
}