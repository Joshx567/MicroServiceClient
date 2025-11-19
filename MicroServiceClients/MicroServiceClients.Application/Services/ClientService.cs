// Ruta: ServiceClient/Application/Services/ClientService.cs

using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
using ServiceClient.Domain.Ports;
using ServiceClient.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Client> CreateAsync(Client client)
    {
        var validationResult = ClientValidationRules.Validate(client);
        if (validationResult.IsFailure)
        {
            throw new ArgumentException(validationResult.Error);
        }

        // Asigna valores de negocio por defecto.
        client.CreatedAt = DateTime.UtcNow;
        client.IsActive = true;
        // Podrías asignar 'CreatedBy' aquí si tuvieras info del usuario.
        // client.CreatedBy = "CurrentUser"; 

        // --- LÓGICA CORREGIDA ---
        // 1. Llama al repositorio, que devuelve el ID del nuevo cliente.
        var newId = await _clientRepository.AddAsync(client);

        // 2. Busca al cliente recién creado usando su nuevo ID para devolver el objeto completo.
        //    Esto garantiza que obtienes los datos tal como quedaron en la BD (con defaults, etc.).
        var createdClient = await _clientRepository.GetByIdAsync(newId);
        if (createdClient == null)
        {
            // Esto sería muy raro, pero es una buena práctica manejarlo.
            throw new InvalidOperationException("Error: No se pudo recuperar el cliente después de la creación.");
        }

        return createdClient;
    }

    public async Task<Client?> UpdateAsync(Client client)
    {
        var validationResult = ClientValidationRules.Validate(client);
        if (validationResult.IsFailure)
        {
            throw new ArgumentException(validationResult.Error);
        }

        // --- LÓGICA CORREGIDA ---
        // 1. Llama al repositorio, que devuelve 'true' si la actualización fue exitosa.
        var success = await _clientRepository.UpdateAsync(client);

        // 2. Si fue exitoso, devuelve el objeto cliente actualizado. Si no, devuelve null.
        return success ? client : null;
    }

    // --- CORREGIDO: Renombrado el método y la llamada ---
    public async Task<bool> DeleteAsync(int id)
    {
        // La llamada ahora coincide con el nombre del método en IClientRepository.
        return await _clientRepository.DeleteAsync(id);
    }

    // --- Estos métodos ya estaban correctos ---
    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _clientRepository.GetAllAsync();
    }

    public async Task<Client?> GetByIdAsync(int id)
    {
        return await _clientRepository.GetByIdAsync(id);
    }
}