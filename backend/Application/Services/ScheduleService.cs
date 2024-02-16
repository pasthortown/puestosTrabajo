using MongoDB.Driver;
using Backend.Domain.Entities;
using Backend.Infrastructure;
using System;
using System.Collections.Generic;

namespace Backend.Application.Services
{
    public class ScheduleService
    {
        private readonly IMongoCollection<Schedule> _schedules;

        public ScheduleService(MongoDBContext context)
        {
            _schedules = context.Schedules;
        }

        public void GuardarAgenda(Schedule agenda)
        {
            ValidarAgenda(agenda);
            _schedules.InsertOne(agenda);
        }

        public List<Schedule> ObtenerAgendas()
        {
            return _schedules.Find(a => true).ToList();
        }

        public List<Schedule> ObtenerAgendasPorFecha(string fecha)
        {
            DateTime fechaConsulta = DateTime.Parse(fecha).Date;
            DateTime fechaSiguiente = fechaConsulta.AddDays(1); // Para incluir hasta el final del día.

            var filter = Builders<Schedule>.Filter.Gte("start_date", fechaConsulta) &
                        Builders<Schedule>.Filter.Lt("start_date", fechaSiguiente);
            
            return _schedules.Find(filter).ToList();
        }

        public void EliminarAgenda(string agendaId)
        {
            var filter = Builders<Schedule>.Filter.Eq("id", agendaId);
            var agendaExistente = _schedules.Find(filter).FirstOrDefault();
            if (agendaExistente == null)
            {
                throw new InvalidOperationException($"No se encontró una agenda con ID {agendaId}.");
            }
            _schedules.DeleteOne(filter);
        }

        public void ActualizarAgenda(string agendaId, Schedule updatedSchedule)
        {
            var filter = Builders<Schedule>.Filter.Eq("id", agendaId);
            var agendaExistente = _schedules.Find(filter).FirstOrDefault();

            if (agendaExistente == null)
            {
                throw new InvalidOperationException($"No se encontró una agenda con ID {agendaId}.");
            }

            agendaExistente.workspace = updatedSchedule.workspace;
            agendaExistente.start_date = updatedSchedule.start_date;
            agendaExistente.end_date = updatedSchedule.end_date;
            agendaExistente.person_id = updatedSchedule.person_id;

            _schedules.ReplaceOne(filter, agendaExistente);
        }

        public Schedule ObtenerAgendaPorId(string agendaId)
        {
            var filter = Builders<Schedule>.Filter.Eq("id", agendaId);
            return _schedules.Find(filter).FirstOrDefault();
        }

        private void ValidarAgenda(Schedule agenda)
        {
            if (agenda == null)
            {
                throw new ArgumentNullException(nameof(agenda), "La agenda no puede ser nula.");
            }

            if (string.IsNullOrWhiteSpace(agenda.workspace))
            {
                throw new ArgumentException("El campo workspace de la agenda es obligatorio.", nameof(agenda.workspace));
            }
        }
    }
}
