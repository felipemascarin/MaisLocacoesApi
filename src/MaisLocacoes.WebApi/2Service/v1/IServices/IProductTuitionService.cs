using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Custom;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using Repository.v1.Entity;

namespace Service.v1.IServices
{
    public interface IProductTuitionService
    {
        Task<ProductTuitionResponse> CreateProductTuition(ProductTuitionRequest productTuitionRequest);
        Task<bool> WithdrawProduct(int id);
        Task<bool> CancelWithdrawProduct(int id);
        Task<bool> RenewProduct(int id, RenewProductTuitionRequest renewRequest);
        Task<GetProductTuitionRentResponse> GetById(int id);
        Task<IEnumerable<GetProductTuitionRentProductTypeClientReponse>> GetAllByRentId(int rentId);
        Task<GetProductTuitionProductIdResponse> GetAllByProductId(int productId);
        Task<IEnumerable<GetProductTuitionRentProductTypeClientReponse>> GetAllToRemove();
        Task<bool> UpdateProductTuition(ProductTuitionRequest productTuitionRequest, int id);
        Task<bool> UpdateProductCode(string productCode, int id);
        Task<bool> UpdateStatus(string status, int id);
        Task<bool> DeleteById(int id);
        void FinishRentIfTheLast(ProductTuitionEntity productTuitionEntity);
        Task<ProductEntity> RetainProduct(ProductTuitionEntity productTuition, ProductEntity productEntity);
        Task<ProductEntity> ReleaseProduct(ProductTuitionEntity productTuition, ProductEntity productEntity);
    }
}