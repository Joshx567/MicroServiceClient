using Microsoft.AspNetCore.Mvc;
using ServiceClient.Domain.Entities;
using ServiceClient.Domain.Ports;

namespace MicroServiceClients.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;

        // Inyectamos la dependencia del repositorio
        public ClientsController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        // GET: api/clients
        // Obtiene todos los clientes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientRepository.GetAllAsync();
            return Ok(clients);
        }

        // GET: api/clients/5
        // Obtiene un cliente por su ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
            {
                return NotFound(); // Devuelve 404 si no se encuentra
            }
            return Ok(client);
        }

        // POST: api/clients
        // Crea un nuevo cliente (persona + cliente)
        // En el método Create de tu ClientsController

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Client client)
        {
            if (client == null)
            {
                return BadRequest();
            }

            // Asigna aquí los valores de auditoría antes de crear
            client.CreatedAt = DateTime.UtcNow;
            client.CreatedBy = "API"; // O el nombre del usuario autenticado

            var newId = await _clientRepository.AddAsync(client);

            // Usamos el nuevo ID para la respuesta CreatedAtAction
            return CreatedAtAction(nameof(GetById), new { id = newId }, client);
        }

        // PUT: api/clients/5
        // Actualiza un cliente existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Client client)
        {
            if (id != client.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");
            }

            var updatedClient = await _clientRepository.UpdateAsync(client);
            if (updatedClient == null)
            {
                return NotFound(); // No se encontró el registro para actualizar
            }

            return NoContent(); // Estándar para una actualización exitosa
        }

        // DELETE: api/clients/5
        // Elimina un cliente por su ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _clientRepository.DeleteAsync(id);
            if (!success)
            {
                return NotFound(); // No se encontró el registro para eliminar
            }

            return NoContent(); // Estándar para una eliminación exitosa
        }
    }
}