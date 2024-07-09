namespace WebAPI.Commons
{
    public class ApiException
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string? Detail { get; set; }
       
    }
}
