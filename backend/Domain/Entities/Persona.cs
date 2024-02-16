namespace Backend.Domain.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        
        public Person(int id, string nombre, string email)
        {
            Id = id;
            Nombre = nombre;
            Email = email;
        }
    }
}
