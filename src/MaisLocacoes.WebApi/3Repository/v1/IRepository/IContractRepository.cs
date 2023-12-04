using MaisLocacoes.WebApi._3Repository.v1.Entity;

namespace MaisLocacoes.WebApi._3Repository.v1.IRepository
{
    public interface IContractRepository
    {
        Task<ContractEntity> CreateContract(ContractEntity contractEntity);
        Task<ContractEntity> GetById(int id);
        Task<IEnumerable<ContractEntity>> GetAll();
        Task<ContractEntity> GetContractInfoByRentId(int rentId);
        Task<int> GetTheLastVersion(int rentId);
        Task<ContractEntity> GetTheLastContract(int rentId);
        Task<int> UpdateContract(ContractEntity contractForUpdate);
    }
}
