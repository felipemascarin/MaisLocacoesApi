namespace MaisLocacoes.WebApi.Utils.Enums
{
    public class UserRole
    {
        public static IEnumerable<string> PersonRolesEnum = new List<string>
        {
            "owner",
            "employee",
            "delivery"
        };
    }
}