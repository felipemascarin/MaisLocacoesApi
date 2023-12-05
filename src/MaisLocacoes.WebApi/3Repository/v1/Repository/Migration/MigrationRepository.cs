using MaisLocacoes.WebApi._3Repository.v1.IRepository.Migration;
using MaisLocacoes.WebApi.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MaisLocacoes.WebApi._3Repository.v1.Repository.Migration
{
    public class MigrationRepository : IMigrationRepository
    {
        private readonly IConfiguration _configuration;
        private readonly PostgreSqlContextFactory _contextFactory;
        private readonly string _connectionString;
        private readonly string _projectDirectory;

        public MigrationRepository(PostgreSqlContextFactory contextFactory, IConfiguration configuration)
        {
            _contextFactory = contextFactory;
            _configuration = configuration;
            _projectDirectory = _configuration["ProjectPatchForMigration:ProjectPatch"];
            _connectionString = _configuration["MyPostgreSqlConnection:MyPostgreSqlConnectionString"];
        }

        public async Task AddMigrationAllDataBases(List<string> databaseNames)
        {
            foreach (var dbName in databaseNames)
            {
                using (var context = _contextFactory.CreateContext(dbName))
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

                using (var command = new NpgsqlCommand("SELECT datname FROM pg_database WHERE datname NOT ILIKE '%template%'", connection))
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