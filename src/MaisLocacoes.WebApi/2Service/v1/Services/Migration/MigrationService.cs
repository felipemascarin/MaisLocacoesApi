using MaisLocacoes.WebApi._2Service.v1.IServices.Migration;
using MaisLocacoes.WebApi._3Repository.v1.IRepository.Migration;

namespace MaisLocacoes.WebApi._2Service.v1.Services.Migration
{
    public class MigrationService : IMigrationService
    {
        private readonly IMigrationRepository _migrationRepository;

        public MigrationService(IMigrationRepository migrationRepository)
        {
            _migrationRepository = migrationRepository;
        }

        public async Task AddMigrationAllDataBases()
        {
            var databaseNames = await _migrationRepository.GetDatabaseNamesAsync();

            await _migrationRepository.AddMigrationAllDataBases(databaseNames);
        }
    }
}