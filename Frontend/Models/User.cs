using System.ComponentModel.DataAnnotations;

namespace _225DAPM32.Models
{
    public class User
    {
        [Key]
        public int Id_User { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Status { get; set; } // active, inactive
        public DateTime Created_At { get; set; }
        public DateTime LastOnline { get; set; }
        public string Update_Bio { get; set; }
        public string Update_Avatar { get; set; }
        public string Update_Bg { get; set; }
        public decimal Current_Lat { get; set; }
        public decimal Current_Lng { get; set; }
        public float Cancel_Rate { get; set; }
    }
}