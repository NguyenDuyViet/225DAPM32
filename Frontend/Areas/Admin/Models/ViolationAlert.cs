namespace _225DAPM32.Areas.Admin.Models
{
    public class ViolationAlert
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}