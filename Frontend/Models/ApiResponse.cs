namespace _225DAPM32.Models
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T? Results { get; set; }
    }
}
