using ServiceClient.Domain.Entities;
using ServiceClient.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceClient.Application.Interfaces
{
    public interface IClientService
    {
        Task<Result<Client>> CreateAsync(Client client);
        Task<Result<Client>> UpdateAsync(Client client);
        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
