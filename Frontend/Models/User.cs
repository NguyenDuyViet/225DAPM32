using System.ComponentModel.DataAnnotations;

namespace _225DAPM32.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }
        public int IdRole { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public string Status { get; set; } // 'active','inactive','locked'
        public DateTime CreatedAt { get; set; }
        public DateTime? LastOnline { get; set; }
        public decimal? CurrentLat { get; set; }
        public decimal? CurrentLng { get; set; }
        public float? CancelRate { get; set; }

        // Legacy frontend profile fields kept so existing Profile views do not break.
        public string UpdateBio { get; set; }
        public string UpdateAvatar { get; set; }
        public string UpdateBg { get; set; }

        public Role Role { get; set; }
    }
}
