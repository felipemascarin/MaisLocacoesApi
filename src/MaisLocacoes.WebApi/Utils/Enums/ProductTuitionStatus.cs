namespace MaisLocacoes.WebApi.Utils.Enums
{
    public class ProductTuitionStatus
    {
        public static IEnumerable<string> ProductTuitionStatusEnum = new List<string>
        {
            "activated",
            "scheduled",
            "delivered",
            "deliver",
            "withdraw",
            "returned"
        };
    }
}