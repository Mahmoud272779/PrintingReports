namespace App.Domain.Entities.Process.AttendLeaving
{
    public class Holidays
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public ICollection<HolidaysEmployees> EmployeesHolidays { get; set; }

    }
}
