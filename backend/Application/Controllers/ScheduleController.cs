using Microsoft.AspNetCore.Mvc;
using Backend.Application.Services;
using Backend.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Backend.Application.Controllers
{
    /// <summary>
    /// Controlador para gestionar agendas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleService _scheduleAppService;

        /// <summary>
        /// Constructor del controlador de agendas.
        /// </summary>
        /// <param name="scheduleService">Servicio de agendas.</param>
        public ScheduleController(ScheduleService scheduleService)
        {
            _scheduleAppService = scheduleService;
        }

        /// <summary>
        /// Crea una agenda nueva.
        /// </summary>
        /// <param name="schedule">Datos de la agenda a crear.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPost]
        public IActionResult CrearAgenda([FromBody] Schedule schedule)
        {
            try
            {
                _scheduleAppService.GuardarAgenda(schedule);
                return Ok("Agenda guardada correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al guardar la agenda: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene las agendas de una fecha específica.
        /// </summary>
        /// <param name="fecha">Fecha para obtener las agendas.</param>
        /// <returns>Devuelve las agendas de esa fecha.</returns>
        [HttpGet("fecha/{fecha}")]
        public IActionResult ObtenerAgendasPorFecha(string fecha)
        {
            try
            {
                var agendas = _scheduleAppService.ObtenerAgendasPorFecha(fecha);
                return Ok(agendas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener las agendas por fecha: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene todas las agendas.
        /// </summary>
        /// <returns>Lista de agendas.</returns>
        [HttpGet]
        public IActionResult ObtenerAgendas()
        {
            try
            {
                var agendas = _scheduleAppService.ObtenerAgendas();
                return Ok(agendas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener las agendas: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene una agenda por su ID.
        /// </summary>
        /// <param name="id">ID de la agenda.</param>
        /// <returns>Devuelve la agenda correspondiente al ID proporcionado.</returns>
        [HttpGet("id/{id}")]
        public IActionResult ObtenerAgendaPorId(string id)
        {
            try
            {
                var agenda = _scheduleAppService.ObtenerAgendaPorId(id);
                if (agenda == null)
                {
                    return NotFound("Agenda no encontrada");
                }
                return Ok(agenda);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener la agenda: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene las agendas asociadas a un correo electrónico.
        /// </summary>
        /// <param name="email">Correo electrónico para buscar las agendas.</param>
        /// <returns>Devuelve las agendas asociadas al correo electrónico proporcionado.</returns>
        [HttpGet("email/{email}")]
        public IActionResult ObtenerAgendasPorEmail(string email)
        {
            try
            {
                var agendas = _scheduleAppService.ObtenerAgendasPorEmail(email);
                if (agendas.Count == 0)
                {
                    return NotFound("No se encontraron agendas para este correo electrónico");
                }
                return Ok(agendas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener las agendas por correo electrónico: {ex.Message}");
            }
        }

        /// <summary>
        /// Borra una agenda a partir de su ID.
        /// </summary>
        /// <param name="id">ID de la agenda a borrar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete("{id}")]
        public IActionResult EliminarAgenda(string id)
        {
            try
            {
                _scheduleAppService.EliminarAgenda(id);
                return Ok($"Agenda con ID {id} eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar la agenda: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza una agenda a partir de su ID.
        /// </summary>
        /// <param name="id">ID de la agenda a actualizar.</param>
        /// <param name="updatedSchedule">Datos actualizados de la agenda.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPut("{id}")]
        public IActionResult ActualizarAgenda(string id, [FromBody] Schedule updatedSchedule)
        {
            try
            {
                _scheduleAppService.ActualizarAgenda(id, updatedSchedule);
                return Ok($"Agenda con ID {id} actualizada correctamente.");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound($"Error al actualizar la agenda: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar la agenda: {ex.Message}");
            }
        }
    }
}
