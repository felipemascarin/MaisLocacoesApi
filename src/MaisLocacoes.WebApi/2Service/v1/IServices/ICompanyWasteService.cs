using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;

namespace Service.v1.IServices
{
    public interface ICompanyWasteService
    {
        Task<CompanyWasteResponse> CreateCompanyWaste(CompanyWasteRequest companyWasteRequest);
        Task<CompanyWasteResponse> GetById(int id);
        Task<bool> UpdateCompanyWaste(CompanyWasteRequest companyWasteRequest, int id);
        Task<bool> DeleteById(int id);
    }
}