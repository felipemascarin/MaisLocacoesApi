using MaisLocacoes.WebApi._3Repository.v1.IRepository.Migration;
using MaisLocacoes.WebApi.DataBase.Context;
using Npgsql;
using System.Diagnostics;

namespace MaisLocacoes.WebApi._3Repository.v1.Repository.Migration
{
    public class MigrationRepository : IMigrationRepository
    {
        private readonly IConfiguration _configuration;
        private readonly PostgreSqlContextFactory _contextFactory;
        private readonly string _connectionString;

        public MigrationRepository(PostgreSqlContextFactory contextFactory, IConfiguration configuration)
        {
            _contextFactory = contextFactory;
            _configuration = configuration;
            _connectionString = _configuration["MyPostgreSqlConnection:MyPostgreSqlConnectionString"];
        }

        public async Task AddMigrationAllDataBases(List<string> databaseNames)
        {
            foreach (var database in databaseNames)
            {
                await AddMigrationAndApplyToDatabase(database);
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

        private async Task AddMigrationAndApplyToDatabase(string databaseName)
        {
            string addMigration = $"dotnet ef migrations add {DateTime.UtcNow.ToString("dd-MM-yyyy")}_UTC -c PostgreSqlContext -p MaisLocacoes.WebApi";

            string databaseUpdate = $"dotnet ef database update -c PostgreSqlContext -p MaisLocacoes.WebApi";

            using (var context = _contextFactory.CreateContext(databaseName))
            {
                // Adiciona a migração
                await RunCommandAsync(addMigration);

                // Aplica a migração ao banco de dados
                await RunCommandAsync(databaseUpdate);
            }
        }

        private async Task RunCommandAsync(string command)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = processInfo })
            {
                process.Start();

                // Aguarda até que o processo termine
                await process.WaitForExitAsync();

                // Exibe a saída padrão
                var output = await process.StandardOutput.ReadToEndAsync();
                Console.WriteLine(output);

                // Exibe a saída de erro, se houver
                var error = await process.StandardError.ReadToEndAsync();
                if (!string.IsNullOrWhiteSpace(error))
                {
                    Console.WriteLine($"Erro: {error}");
                }
            }
        }
    }
}