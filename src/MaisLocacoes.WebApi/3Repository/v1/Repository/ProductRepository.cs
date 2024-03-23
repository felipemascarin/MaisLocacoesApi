using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using MaisLocacoes.WebApi.Utils.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using static MaisLocacoes.WebApi.Domain.Models.v1.Response.Product.GetProductsForRentResponse;

namespace Repository.v1.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory;

        public ProductRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<ProductEntity> CreateProduct(ProductEntity productEntity)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(productEntity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return productEntity;
        }

        public async Task<ProductEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductEntity>()
                .Include(p => p.ProductType)
                .Include(p => p.ProductTuitions)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ProductEntity> GetByTypeCode(int typeId, string code)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductEntity>().Include(p => p.ProductType).FirstOrDefaultAsync(p => p.ProductTypeId == typeId && p.Code.ToLower() == code.ToLower());
        }

        public async Task<bool> ProductExists(int typeId, string code)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductEntity>().AnyAsync(p => p.ProductTypeId == typeId && p.Code.ToLower() == code.ToLower());
        }

        public async Task<bool> ProductExists(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductEntity>().AnyAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<ProductEntity>> GetProductsByPage(int items, int page, string query)
        {
            using var context = _contextFactory.CreateContext();
            if (query == null)
                return await context.Set<ProductEntity>().Include(p => p.ProductType).Where(p => p.Status != ProductStatus.ProductStatusEnum.ElementAt(3) /*inactive*/).Skip((page - 1) * items).Take(items).ToListAsync();
            else
                return await context.Set<ProductEntity>().Include(p => p.ProductType).Where(p => (
                     p.Code.ToLower().Contains(query.ToLower()) ||
                     p.Description.ToLower().Contains(query.ToLower()) ||
                     p.ProductType.Type.ToLower().Contains(query.ToLower())) &&
                     p.Status != ProductStatus.ProductStatusEnum.ElementAt(3) /*inactive*/)
                    .Skip((page - 1) * items).Take(items).ToListAsync();
        }

        public async Task<IEnumerable<GetProductForRentDtoResponse>> GetProductsForRent(int productTypeId)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductEntity>()
                .Include(p => p.ProductType).Where(p => p.Status == ProductStatus.ProductStatusEnum.ElementAt(0) /*free*/ && p.ProductType.Id == productTypeId)
                .Select(p => new GetProductForRentDtoResponse
                {
                    Code = p.Code,
                    Parts = p.Parts,
                    RentedParts = p.RentedParts
                }).ToListAsync();
        }

        public async Task<IEnumerable<ProductEntity>> GetProductsByProductCodeList(List<string> productCodeList)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductEntity>().Include(p => p.ProductType).Where(p => productCodeList.Contains(p.Code)).ToListAsync();
        }

        public async Task<ProductEntity> GetTheLastsCreated(int productTypeId)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductEntity>().OrderByDescending(p => p.CreatedAt).FirstOrDefaultAsync(p => p.ProductType.Id == productTypeId);
        }

        public async Task<IEnumerable<ProductEntity>> GetAllProducts()
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductEntity>().Include(p => p.ProductType).ToListAsync();
        }

        public async Task<int> UpdateProduct(ProductEntity productForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(productForUpdate).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteProduct(ProductEntity productForDelete)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(productForDelete).State = EntityState.Deleted;
            return await context.SaveChangesAsync();
        }
    }
}