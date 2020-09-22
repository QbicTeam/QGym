using System.Collections.Generic;
using System.Threading.Tasks;

/*
using Framework.DataTypes.Model.Base;
using Framework.DataTypes.Model.Licenciamiento;
using Framework.DataTypes.Model.Infraestructura;
*/

namespace QGym.API.Data
{
    public interface IGymRepository
    {
         void Add<T>(T entity) where T: class;
         Task<bool> SaveAll();
         // Task<IEnumerable<User>> GetUsers();
         //Task<PagedList<User>> GetUsers(UserParams userParams);
         
         /*
         Task<Cliente> GetCliente(int id);
         Task<IEnumerable<Cliente>> GetClientes();
         */
    }
}