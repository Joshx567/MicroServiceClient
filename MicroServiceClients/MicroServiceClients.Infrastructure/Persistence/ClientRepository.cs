using Dapper;
using ServiceClient.Domain.Entities;
using ServiceClient.Domain.Ports;
using ServiceClient.Infrastructure.Providers;
using System.Collections.Generic;
using System.Threading.Tasks;

// El nombre de la clase no cambia
public class ClientRepository : IClientRepository
{
    private readonly IClientConnectionProvider _connectionProvider;

    public ClientRepository(IClientConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    // Cambiado para reflejar una inserción simple y devolver el nuevo ID.
    // Lo llamamos 'AddAsync' para mantener consistencia con tus otros repositorios.
    public async Task<int> AddAsync(Client client)
    {
        //comprobar si llegan datos...
        Console.WriteLine("Insertando cliente en la base de datos:");
        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(client));

        // La entidad 'Client' ya debe venir con los valores necesarios (name, ci, etc.)
        const string sql = @"
            INSERT INTO public.clients (
                name, first_lastname, second_lastname, date_birth, ci, is_active,
                fitness_level, initial_weight_kg, current_weight_kg, emergency_contact_phone,
                created_at, created_by
            )
            VALUES (
                @Name, @FirstLastname, @SecondLastname, @DateBirth, @Ci, @IsActive,
                @FitnessLevel, @InitialWeightKg, @CurrentWeightKg, @EmergencyContactPhone,
                @CreatedAt, @CreatedBy
            ) RETURNING id;";

        using var conn = _connectionProvider.CreateConnection();
        // ExecuteScalarAsync es perfecto para devolver un solo valor, como el ID.
        var newId = await conn.ExecuteScalarAsync<int>(sql, client);
        return newId;
    }

    // Simplificado para leer desde una sola tabla.
    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        const string sql = "SELECT * FROM public.clients ORDER BY name, first_lastname;";
        using var conn = _connectionProvider.CreateConnection();
        return await conn.QueryAsync<Client>(sql);
    }

    // Simplificado para leer desde una sola tabla.
    public async Task<Client?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM public.clients WHERE id = @Id;";
        using var conn = _connectionProvider.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<Client>(sql, new { Id = id });
    }

    // Simplificado a un solo UPDATE y devuelve bool para indicar éxito.
    public async Task<bool> UpdateAsync(Client client)
    {
        const string sql = @"
            UPDATE public.clients SET
                name = @Name,
                first_lastname = @FirstLastname,
                second_lastname = @SecondLastname,
                date_birth = @DateBirth,
                ci = @Ci,
                is_active = @IsActive,
                fitness_level = @FitnessLevel,
                current_weight_kg = @CurrentWeightKg,
                emergency_contact_phone = @EmergencyContactPhone,
                last_modification = @LastModification,
                last_modified_by = @LastModifiedBy
            WHERE id = @Id;";

        using var conn = _connectionProvider.CreateConnection();
        var affectedRows = await conn.ExecuteAsync(sql, client);
        return affectedRows > 0;
    }

    // Simplificado, pero la lógica es casi idéntica.
    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM public.clients WHERE id = @Id;";
        using var conn = _connectionProvider.CreateConnection();
        var affectedRows = await conn.ExecuteAsync(sql, new { Id = id });
        return affectedRows > 0;
    }
}