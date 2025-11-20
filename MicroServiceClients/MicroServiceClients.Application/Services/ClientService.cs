// Ruta: ServiceClient/Application/Services/ClientService.cs

using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Common;
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

    public async Task<Result<Client>> CreateAsync(Client client)
    {
        var validationResult = ClientValidationRules.Validate(client);
        if (validationResult.IsFailure)
        {
            return Result<Client>.Failure(validationResult.Error); // Devuelve el error
        }

        var newId = await _clientRepository.AddAsync(client);
        var createdClient = await _clientRepository.GetByIdAsync(newId);

        if (createdClient == null)
            return Result<Client>.Failure("Error: no se pudo recuperar el cliente después de la creación.");

        return Result<Client>.Success(createdClient);
    }

    public async Task<Result<Client>> UpdateAsync(Client client)
    {
        var validationResult = ClientValidationRules.Validate(client);
        if (validationResult.IsFailure)
            return Result<Client>.Failure(validationResult.Error);

        var success = await _clientRepository.UpdateAsync(client);
        if (!success) return Result<Client>.Failure("No se pudo actualizar el cliente.");

        return Result<Client>.Success(client);
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