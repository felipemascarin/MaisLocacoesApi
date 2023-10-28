using MaisLocacoes.WebApi._3Repository.v1.Entity;

namespace MaisLocacoes.WebApi._3Repository.v1.IRepository
{
    public interface IContractRepository
    {
        Task<ContractEntity> CreateContract(ContractEntity contractEntity);
        Task<ContractEntity> GetById(int id);
        Task<IEnumerable<ContractEntity>> GetAll();
        Task<int> UpdateContract(ContractEntity contractForUpdate);
    }
}
