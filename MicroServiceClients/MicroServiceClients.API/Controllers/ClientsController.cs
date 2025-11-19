using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceClients.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _service;

        public ClientsController(IClientService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene un cliente por ID.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var client = await _service.GetByIdAsync(id);
            if (client == null) return NotFound(new { error = "Cliente no encontrado" });
            return Ok(client);
        }

        /// <summary>
        /// Obtiene todos los clientes.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _service.GetAllAsync();
            return Ok(clients);
        }

        /// <summary>
        /// Crea un nuevo cliente.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] Client newClient)
        {
            var created = await _service.CreateAsync(newClient);
            return CreatedAtAction(nameof(GetClientById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Actualiza un cliente existente.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] Client updatedClient)
        {
            if (id != updatedClient.Id)
                return BadRequest(new { error = "El ID del cliente no coincide." });

            var updated = await _service.UpdateAsync(updatedClient);
            if (updated == null) return NotFound(new { error = "Cliente no encontrado" });

            return Ok(updated);
        }

        /// <summary>
        /// Elimina un cliente.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var deleted = await _service.DeleteByIdAsync(id);
            if (!deleted) return NotFound(new { error = "Cliente no encontrado" });

            return Ok(new { message = "Cliente eliminado correctamente" });
        }
    }
}
