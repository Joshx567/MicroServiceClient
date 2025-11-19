using ServiceClient.Domain.Entities;

namespace ServiceClient.Application.Interfaces
{
    public interface IClientService
    {
        Task<Client> CreateAsync(Client client);
        Task<Client?> UpdateAsync(Client client);
        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}