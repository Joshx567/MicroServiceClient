using Microsoft.AspNetCore.Mvc;
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
using ServiceClient.Domain.Common;
using ServiceClient.Domain.Rules;

namespace MicroServiceClients.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        // GET: api/clients
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientService.GetAllAsync();
            return Ok(clients);
        }

        // GET: api/clients/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null)
                return NotFound();
            return Ok(client);
        }

        // POST: api/clients
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Client client)
        {
            Console.WriteLine("Cliente recibido en el microservicio:");
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(client));

            // Validación
            var validation = ClientValidationRules.Validate(client);
            if (validation.IsFailure)
            {
                Console.WriteLine("Error de validación: " + validation.Error);
                return BadRequest(new { Error = validation.Error });
            }

            try
            {
                var createdClient = await _clientService.CreateAsync(client);
                Console.WriteLine("Cliente validado y creado: " + System.Text.Json.JsonSerializer.Serialize(createdClient));
                return CreatedAtAction(nameof(GetById), new { id = createdClient.Value.Id }, createdClient.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Excepción al crear cliente: " + ex.Message);
                return BadRequest(new { Error = ex.Message });
            }
        }


        // PUT: api/clients/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Client client)
        {
            if (id != client.Id)
                return BadRequest(new { Error = "ID de URL no coincide con ID del cliente." });

            var validation = ClientValidationRules.Validate(client);
            if (validation.IsFailure)
            {
                return BadRequest(new { Error = validation.Error });
            }

            try
            {
                var result = await _clientService.UpdateAsync(client);

                if (result.IsFailure)
                    return BadRequest(new { Error = result.Error });

                var updatedClient = result.Value;
                return Ok(updatedClient);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // DELETE: api/clients/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _clientService.DeleteAsync(id);
            if (!success)
                return NotFound();
            return NoContent();
        }
    }
}
