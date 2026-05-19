namespace Backend.DTOs.Response
{
    public class ApiResponse<T>
    {
        public int Code { get; set; } = 1000;
        public string Message { get; set; }
        public T Results { get; set; }
    }
}
