using MongoDB.Driver;
using Backend.Domain.Entities;
using Backend.Infrastructure;
using System;
using System.Collections.Generic;

namespace Backend.Application.Services
{
    public class PersonService
    {
        private readonly IMongoCollection<Person> _persons;

        public PersonService(MongoDBContext context)
        {
            _persons = context.Persons;
        }

        public void GuardarPersona(Person persona)
        {
            ValidarPersona(persona);
            _persons.InsertOne(persona);
        }

        public List<Person> ObtenerPersonas()
        {
            return _persons.Find(p => true).ToList();
        }

        public void EliminarPersona(string personaId)
        {
            var filter = Builders<Person>.Filter.Eq("Id", personaId);
            var personaExistente = _persons.Find(filter).FirstOrDefault();
            if (personaExistente == null)
            {
                throw new InvalidOperationException($"No se encontró una persona con ID {personaId}.");
            }
            _persons.DeleteOne(filter);
        }

        public void ActualizarPersona(string personaId, Person updatedPerson)
        {
            var filter = Builders<Person>.Filter.Eq("Id", personaId);
            var personaExistente = _persons.Find(filter).FirstOrDefault();

            if (personaExistente == null)
            {
                throw new InvalidOperationException($"No se encontró una persona con ID {personaId}.");
            }

            personaExistente.Nombre = updatedPerson.Nombre;
            personaExistente.Email = updatedPerson.Email;

            _persons.ReplaceOne(filter, personaExistente);
        }

        public Person ObtenerPersonaPorId(string personaId)
        {
            var filter = Builders<Person>.Filter.Eq("Id", personaId);
            return _persons.Find(filter).FirstOrDefault();
        }

        private void ValidarPersona(Person persona)
        {
            if (persona == null)
            {
                throw new ArgumentNullException(nameof(persona), "La persona no puede ser nula.");
            }

            if (string.IsNullOrWhiteSpace(persona.Nombre))
            {
                throw new ArgumentException("El campo Nombre de la persona es obligatorio.", nameof(persona.Nombre));
            }

            if (string.IsNullOrWhiteSpace(persona.Email))
            {
                throw new ArgumentException("El campo Email de la persona es obligatorio.", nameof(persona.Email));
            }
        }
    }
}
