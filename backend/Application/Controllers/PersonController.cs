using Microsoft.AspNetCore.Mvc;
using Backend.Application.Services;
using Backend.Domain.Entities;
using System;

namespace Backend.Application.Controllers
{
    /// <summary>
    /// Controlador para gestionar personas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PersonService _personService;

        /// <summary>
        /// Constructor del controlador de personas.
        /// </summary>
        /// <param name="personService">Servicio de personas.</param>
        public PersonController(PersonService personService)
        {
            _personService = personService;
        }

        /// <summary>
        /// Crea una nueva persona.
        /// </summary>
        /// <param name="person">Datos de la persona a crear.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPost]
        public IActionResult CrearPersona([FromBody] Person person)
        {
            try
            {
                _personService.GuardarPersona(person);
                var respuesta = new { Mensaje = "Persona guardada correctamente" };
                return Ok(new { respuesta });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al guardar la persona: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene todas las personas.
        /// </summary>
        /// <returns>Lista de personas.</returns>
        [HttpGet]
        public IActionResult ObtenerPersonas()
        {
            try
            {
                var personas = _personService.ObtenerPersonas();
                return Ok(personas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener las personas: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene una persona por su ID.
        /// </summary>
        /// <param name="id">ID de la persona.</param>
        /// <returns>Devuelve la persona correspondiente al ID proporcionado.</returns>
        [HttpGet("id/{id}")]
        public IActionResult ObtenerPersonaPorId(string id)
        {
            try
            {
                var persona = _personService.ObtenerPersonaPorId(id);
                if (persona == null)
                {
                    return NotFound("Persona no encontrada");
                }
                return Ok(persona);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener la persona: {ex.Message}");
            }
        }

        /// <summary>
        /// Borra una persona a partir de su ID.
        /// </summary>
        /// <param name="id">ID de la persona a borrar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete("{id}")]
        public IActionResult EliminarPersona(string id)
        {
            try
            {
                _personService.EliminarPersona(id);
                var respuesta = new { Mensaje = $"Persona con ID {id} eliminada correctamente" };
                return Ok(new { respuesta });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar la persona: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza una persona a partir de su ID.
        /// </summary>
        /// <param name="id">ID de la persona a actualizar.</param>
        /// <param name="updatedPerson">Datos actualizados de la persona.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPut("{id}")]
        public IActionResult ActualizarPersona(string id, [FromBody] Person updatedPerson)
        {
            try
            {
                _personService.ActualizarPersona(id, updatedPerson);
                return Ok($"Persona con ID {id} actualizada correctamente.");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound($"Error al actualizar la persona: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar la persona: {ex.Message}");
            }
        }
    }
}
