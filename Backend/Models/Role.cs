namespace Backend.Models
{
    public class Role
    {
        public int IdRole { get; set; }
        public string RoleName { get; set; }

        public ICollection<User> Users { get; set; }
    }
}