using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.EntityFrameworkCore;
/*
using Framework.DataTypes.Model.Base;
using Framework.DataTypes.Model.Licenciamiento;
using Framework.DataTypes.Model.Infraestructura;
*/
using prometheus.model.gym;
using prometheus.model.securitas;

namespace prometheus.data.gym
{
    public class GymRepository: IGymRepository
    {
        private readonly DataContext _context;

        public GymRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T: class
        {
            _context.Add(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        /*
        public async Task<Cliente> GetCliente(int id)
        {
           var cliente = await _context.Clientes
            .Include(n => n.Negocios)
            //.Where(c => c.Id == id)
            .FirstOrDefaultAsync(c => c.Id == id);

            return cliente;
        }

        public async Task<IEnumerable<Cliente>> GetClientes()
        {
            var clientes = await _context.Clientes
            .Include(n => n.Negocios)
            //.Where(c => c.Id == id)
            .OrderBy(c => c.NomCorto)
            .ToListAsync();

            return clientes;
        }
        */
    
    }
}