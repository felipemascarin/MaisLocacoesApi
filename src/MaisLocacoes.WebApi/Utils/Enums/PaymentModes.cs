namespace MaisLocacoes.WebApi.Utils.Enums
{
    public class PaymentModes
    {
        public static IEnumerable<string> PaymentModesEnum = new List<string>
        {
            "pix",
            "credito",
            "debito",
            "dinheiro",
            "transferencia"
        };
    }
}