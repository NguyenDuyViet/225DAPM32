namespace Backend.DTOs.Request
{
    public class RestRequestDTO
    {
        public string NameRestaurant { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
    }
}
