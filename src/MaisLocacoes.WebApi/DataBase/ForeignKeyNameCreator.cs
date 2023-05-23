namespace MaisLocacoes.WebApi.DataBase
{
    public class ForeignKeyNameCreator
    {
        public static string CreateForeignKeyName(string entityManyName, string entityOneName)
        {
            return $"FK_{entityManyName}_{entityOneName}";
        }
    }
}