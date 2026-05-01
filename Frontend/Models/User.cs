using System.ComponentModel.DataAnnotations;

namespace _225DAPM32.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Status { get; set; } // 'active','inactive'
        public DateTime CreatedAt { get; set; }
        public DateTime? LastOnline { get; set; }
        public string UpdateBio { get; set; }
        public string UpdateAvatar { get; set; }
        public string UpdateBg { get; set; }
        public decimal? CurrentLat { get; set; }
        public decimal? CurrentLng { get; set; }
        public float? CancelRate { get; set; }
        public int IdRole { get; set; }

        // Navigation properties
        public Role Role { get; set; }
    }
}