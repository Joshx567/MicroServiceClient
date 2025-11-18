using ServiceClient.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceClient.Domain.Ports
{
    /// <summary>
    /// Define el contrato para el repositorio de clientes.
    /// Esta interfaz es el "puerto" que la capa de Aplicación usa para hablar con la Infraestructura.
    /// </summary>
    public interface IClientRepository
    {
        Task<int> AddAsync(Client client); // Cambiado
        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Client client); // Cambiado
        Task<bool> DeleteAsync(int id);     
    }
}