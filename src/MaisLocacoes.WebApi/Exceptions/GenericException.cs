namespace MaisLocacoes.WebApi.Exceptions
{
    public class GenericException
    {
        public GenericException(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}