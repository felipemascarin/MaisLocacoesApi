namespace MaisLocacoes.WebApi._3Repository.v1.IRepository.Migration
{
    public interface IMigrationRepository
    {
        Task AddMigrationAllDataBases(List<string> databaseNames);
        Task<List<string>> GetDatabaseNamesAsync();
    }
}
