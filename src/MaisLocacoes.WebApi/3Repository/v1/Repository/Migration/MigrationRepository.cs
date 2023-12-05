using MaisLocacoes.WebApi._3Repository.v1.IRepository.Migration;
using MaisLocacoes.WebApi.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MaisLocacoes.WebApi._3Repository.v1.Repository.Migration
{
    public class MigrationRepository : IMigrationRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory;
        private readonly string _connectionString;

        public MigrationRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddMigrationAllDataBases(List<string> databaseNames)
        {
            foreach (var database in databaseNames)
            {
                using (var context = _contextFactory.CreateContext(database))
                {
                    await context.Database.MigrateAsync();
                }
            }
        }

        public async Task<List<string>> GetDatabaseNamesAsync()
        {
            var databaseNames = new List<string>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT datname FROM pg_database WHERE datname NOT IN ('template0', 'template1')", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        databaseNames.Add(reader.GetString(0));
                    }
                }
            }

            return databaseNames;
        }
    }
}
