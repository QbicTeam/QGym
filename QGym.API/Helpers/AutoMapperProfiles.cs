using System;
using AutoMapper;
using System.Collections.Generic;

using QGym.API.DTO;
/*using Framework.DataTypes.Model.Base;
using Framework.DataTypes.Model.Licenciamiento;
using Framework.DataTypes.Model.Infraestructura;
*/
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
        }
        
    }
}