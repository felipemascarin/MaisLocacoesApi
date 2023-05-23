namespace MaisLocacoes.WebApi.Utils.Enums
{
    public class OsStatus
    {
        public static IEnumerable<string> OsStatusEnum = new List<string>
        {
            "waiting",
            "started",
            "completed",
            "returned",
            "canceled",
        };
    }
}