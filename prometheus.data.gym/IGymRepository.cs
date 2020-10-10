using System.Collections.Generic;
using System.Threading.Tasks;

/*
using Framework.DataTypes.Model.Base;
using Framework.DataTypes.Model.Licenciamiento;
using Framework.DataTypes.Model.Infraestructura;
*/

using prometheus.model.gym;
using prometheus.model.securitas;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace prometheus.data.gym
{
    public interface IGymRepository
    {
         void Add<T>(T entity) where T: class;
         void Remove<T>(T entity) where T : class;
         Task<bool> SaveAll();
        /// <summary>
        /// Alta del Socio, se registra con fecha de Vencimiento del dia de ayer.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        Task<Member> Register(Member member, string memberId);
        /// <summary>
        /// Regresa toda la informacio del Miembro con todo y usuario, de acuerdo al Correo, Numero de Membrecia o UsuarioId
        /// </summary>
        /// <param name="email"></param>
        /// <param name="memberId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Member> GetMember(string email, string memberId, int userId);
        /// <summary>
        /// Regresa la informacion del Usuario (Solo del usuario no el Miembre) de acuerdo al Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> GetUserById(int id);
        /// <summary>
        /// Regresa la lista de los tipos de datos con los que se pueden validar.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ValidationType>> GetValidationTypes();
        /// <summary>
        /// Regresa todos los Usuarios sin restriccion.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Member>> GetMembersToList();
        /// <summary>
        /// Regresa los Usuarios, Valid para los que tienen Vigencias Activas, de lo contrario a regresa los Activos
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<IEnumerable<Member>> GetMembersType(string type);
        /// <summary>
        /// Regresa los Usuarios para Bloquear (Todos)
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Member>> GetMembersForBlock();
        /// <summary>
        /// Regresa los Socios que tienen reservacion para la Fecha/Hora solicitada.
        /// </summary>
        /// <param name="bookedTime"></param>
        /// <returns></returns>
        Task<IEnumerable<Member>> GetMembersBookedInHour(DateTime bookedTime);
        /// <summary>
        /// Regresa los Socios que esten Activos y tengan una membrecia Vigente a la fecha solicitada
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        Task<IEnumerable<Member>> GetMembersElegibles(DateTime day);
        /// <summary>
        /// Regresa el Role de acuerdo al Nombre
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<Role> GetRoleByName(string roleName);
        /// <summary>
        /// Regresa las configuraciones Generales del sistema
        /// </summary>
        /// <returns></returns>
        Task<GeneralSettings> GetGeneralSettings();
        /// <summary>
        /// Regresa el listado de las Capacidad Autorizada Historicas, Ordenade de forma descendente
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AuthorizedCapacity>> GetAuthorizedCapacityToList();
        /// <summary>
        /// Regresa la Capacidad Autorizada de acuerdo a la fecha
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<int> GetCurrentAuthorizedCapacity(DateTime date);
        /// <summary>
        /// Regresa la Ocupacion actual por la Fecha Hora espesificada.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        int GetCurrentOccupationHour(DateTime date);
        /// <summary>
        /// Regresa la reservacion de Usuario en el dia (indica hora)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        UserScheduling GetUserBookedDay(int userId, DateTime date);
        /// <summary>
        /// Regresa las reservaciones del Usuario de hoy en adelante
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<UserScheduling>> GetUserBookedSummary(int userId);
        /// <summary>
        /// Regresa la cantidad de reservaciones por rango de horas en el dia 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        IEnumerable<Scheduled> GetBookedDay(DateTime date);
        /// <summary>
        /// Regresa un listado de los Paquetes/Membresias
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<MembershipType>> GetPackages();
        /// <summary>
        /// Regresa la lista de los  Paquetes/Membresias activos
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<MembershipType>> GetActivePackages();
        /// <summary>
        /// Regresa el Paquete/Membresia espesifico de acuerdo al Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MembershipType> GetMembershipById(int id);

        // Task<IEnumerable<User>> GetUsers();
        //Task<PagedList<User>> GetUsers(UserParams userParams);

        /*
        Task<Cliente> GetCliente(int id);
        Task<IEnumerable<Cliente>> GetClientes();
        */
    }
}