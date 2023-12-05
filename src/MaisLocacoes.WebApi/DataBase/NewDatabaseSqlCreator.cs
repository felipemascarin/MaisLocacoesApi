using System.Text;

namespace MaisLocacoes.WebApi.Context
{
    public class NewDatabaseSqlCreator
    {
        public static string SqlQueryForNewDatabase(string newDatabaseName)
        {
            var stringBuilder = new StringBuilder();
            //Cria um banco de dados para uma nova company cadastrada
            stringBuilder.Append(@$"CREATE DATABASE {newDatabaseName}
                                    WITH 
                                    ENCODING 'UTF8'
                                    LC_COLLATE = 'Portuguese_Brazil.1252'
                                    LC_CTYPE = 'Portuguese_Brazil.1252'
                                    TEMPLATE template0;");

            return stringBuilder.ToString();
        }
    }
}