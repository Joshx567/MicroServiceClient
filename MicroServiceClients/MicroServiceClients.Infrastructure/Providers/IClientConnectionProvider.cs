using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace ServiceClient.Infrastructure.Providers
{
    // Esta es la interfaz que ya tienes definida
    public interface IClientConnectionProvider
    {
        IDbConnection CreateConnection();
    }

    // Esta es la implementación concreta que necesitas crear
    public class NpgsqlClientConnectionProvider : IClientConnectionProvider
    {
        private readonly string _connectionString;

        public NpgsqlClientConnectionProvider(IConfiguration configuration)
        {
            // Lee la cadena de conexión desde appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no fue encontrada.");
        }

        public IDbConnection CreateConnection()
        {
            // Crea y devuelve una nueva conexión a PostgreSQL en cada llamada
            return new NpgsqlConnection(_connectionString);
        }
    }
}