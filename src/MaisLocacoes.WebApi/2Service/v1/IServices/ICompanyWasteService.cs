using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface ICompanyWasteService
    {
        Task<CreateCompanyWasteResponse> CreateCompanyWaste(CreateCompanyWasteRequest companyWasteRequest);
        Task<GetCompanyWasteByIdResponse> GetCompanyWasteById(int id);
        Task<bool> UpdateCompanyWaste(UpdateCompanyWasteRequest companyWasteRequest, int id);
        Task<bool> DeleteById(int id);
    }
}