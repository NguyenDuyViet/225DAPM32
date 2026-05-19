namespace Backend.DTOs.Response
{
    public class UserResponse
    {
        public int IdUser { get; set; }
        public int IdRole { get; set; }
        public string? RoleName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastOnline { get; set; }
        public decimal? CurrentLat { get; set; }
        public decimal? CurrentLng { get; set; }
        public float? CancelRate { get; set; }
    }
}
