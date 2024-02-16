namespace Backend.Domain.Entities
{
    public class Schedule
    {
        public int id { get; set; }
        public string workspace { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public int person_id { get; set; }
        
        public Schedule(int id, string workspace, DateTime start_date, DateTime end_date, int person_id)
        {
            this.id = id;
            this.workspace = workspace;
            this.start_date = start_date;
            this.end_date = end_date;
            this.person_id = person_id;
        }
    }
}