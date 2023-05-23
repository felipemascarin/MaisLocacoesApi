namespace MaisLocacoes.WebApi.Exceptions
{
    public class GenericException
    {
        public string Message { get; set; }

        public GenericException(string message)
        {
            Message = message;
        }
    }
}