using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface ICompanyTuitionService
    {
        Task<CreateCompanyTuitionResponse> CreateCompanyTuition(CreateCompanyTuitionRequest companyTuitionRequest);
        Task<GetCompanyTuitionByIdResponse> GetCompanyTuitionById(int id);
        Task UpdateCompanyTuition(UpdateCompanyTuitionRequest companyTuitionRequest, int id);
        Task DeleteById(int id);
    }
}