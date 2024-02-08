using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Custom;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductTuition;
using Repository.v1.Entity;

namespace Service.v1.IServices
{
    public interface IProductTuitionService
    {
        Task<CreateProductTuitionResponse> CreateProductTuition(CreateProductTuitionRequest productTuitionRequest);
        Task WithdrawProduct(int id);
        Task CancelWithdrawProduct(int id);
        Task RenewProductTuition(int id, RenewProductTuitionRequest renewRequest);
        Task<GetProductTuitionByIdResponse> GetProductTuitionById(int id);
        Task<IEnumerable<GetAllProductTuitionByRentIdReponse>> GetAllProductTuitionByRentId(int rentId);
        Task<GetAllProductTuitionByProductIdResponse> GetAllProductTuitionByProductId(int productId);
        Task<IEnumerable<GetAllProductTuitionToRemoveReponse>> GetAllProductTuitionToRemove();
        Task<IEnumerable<GetAllProductPlacesReponse>> GetAllProductPlaces();
        Task UpdateProductTuition(UpdateProductTuitionRequest productTuitionRequest, int id);
        Task UpdateProductCode(string productCode, int id);
        Task UpdateStatus(string status, int id);
        Task DeleteById(int id);
        void FinishRentIfTheLast(ProductTuitionEntity productTuitionEntity);
        Task<ProductEntity> RetainProduct(ProductTuitionEntity productTuition, ProductEntity productEntity);
        Task<ProductEntity> ReleaseProduct(ProductTuitionEntity productTuition, ProductEntity productEntity);
    }
}