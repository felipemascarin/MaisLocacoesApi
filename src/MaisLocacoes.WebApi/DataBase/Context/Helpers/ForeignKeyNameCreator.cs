namespace MaisLocacoes.WebApi.DataBase.Context.Helpers
{
    public class ForeignKeyNameCreator
    {
        public static string CreateForeignKeyName(string entityManyName, string entityOneName)
        {
            return $"FK_{entityManyName}_{entityOneName}";
        }
    }
}