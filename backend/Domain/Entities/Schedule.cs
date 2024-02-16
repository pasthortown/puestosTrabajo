namespace Backend.Domain.Entities
{
    public class Schedule
    {
        public int id { get; set; }
        public string workspace { get; set; }
        public DateTime date { get; set; }
        public string email { get; set; }
        
        public Schedule(int id, string workspace, DateTime date, string email)
        {
            this.id = id;
            this.workspace = workspace;
            this.date = date;
            this.email = email;
        }
    }
}