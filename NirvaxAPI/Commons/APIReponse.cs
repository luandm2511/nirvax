namespace WebAPI.Commons
{
    public class APIReponse<T>
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public bool? Result { get; set; }
        public T Data { get; set; }
    }
}
