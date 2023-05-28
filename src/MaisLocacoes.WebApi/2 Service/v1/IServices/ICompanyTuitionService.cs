using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;

namespace Service.v1.IServices
{
    public interface ICompanyTuitionService
    {
        Task<CompanyTuitionResponse> CreateCompanyTuition(CompanyTuitionRequest companyTuitionRequest);
        Task<CompanyTuitionResponse> GetById(int id);
        Task<bool> UpdateCompanyTuition(CompanyTuitionRequest companyTuitionRequest, int id);
        Task<bool> DeleteById(int id);
    }
}