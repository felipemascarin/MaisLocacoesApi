using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Custom;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using Repository.v1.Entity;

namespace Service.v1.IServices
{
    public interface IProductTuitionService
    {
        Task<CreateProductTuitionResponse> CreateProductTuition(CreateProductTuitionRequest productTuitionRequest);
        Task<bool> WithdrawProduct(int id);
        Task<bool> CancelWithdrawProduct(int id);
        Task<bool> RenewProductTuition(int id, RenewProductTuitionRequest renewRequest);
        Task<GetProductTuitionByIdResponse> GetProductTuitionById(int id);
        Task<IEnumerable<GetAllProductTuitionByRentIdReponse>> GetAllProductTuitionByRentId(int rentId);
        Task<GetAllProductTuitionByProductIdResponse> GetAllProductTuitionByProductId(int productId);
        Task<IEnumerable<GetAllProductTuitionToRemoveReponse>> GetAllProductTuitionToRemove();
        Task<bool> UpdateProductTuition(UpdateProductTuitionRequest productTuitionRequest, int id);
        Task<bool> UpdateProductCode(string productCode, int id);
        Task<bool> UpdateStatus(string status, int id);
        Task<bool> DeleteById(int id);
        void FinishRentIfTheLast(ProductTuitionEntity productTuitionEntity);
        Task<ProductEntity> RetainProduct(ProductTuitionEntity productTuition, ProductEntity productEntity);
        Task<ProductEntity> ReleaseProduct(ProductTuitionEntity productTuition, ProductEntity productEntity);
    }
}