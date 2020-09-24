using Microsoft.EntityFrameworkCore;
/*
using Framework.DataTypes.Model.Base;
using Framework.DataTypes.Model.Licenciamiento;
using Framework.DataTypes.Model.Infraestructura;
*/

// using QGym.API.Model;

// using QGym.API.Model.Security;
using prometheus.model.gym;
using prometheus.dto.gym;

using prometheus.model.securitas;

namespace prometheus.data.gym
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options) {}

        public DbSet<User> Users { get; set; }
        // public DbSet<Cliente> Clientes { get; set; }
        // public DbSet<ClienteNegocio> ClienteNegocios { get; set; }
        
        
    }
}