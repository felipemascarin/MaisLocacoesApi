using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.DataBase.Context;
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
            using var _context = _contextFactory.CreateContext();
        }

        public async Task<ProductEntity> CreateProduct(ProductEntity productEntity)
        {
            await _context.Products.AddAsync(productEntity);
            _context.SaveChanges();
            return productEntity;
        }

        public async Task<ProductEntity> GetById(int id) => await _context.Products.Include(p => p.ProductTypeEntity).FirstOrDefaultAsync(p => p.Id == id && p.Deleted == false);

        public async Task<ProductEntity> GetByTypeCode(int typeId, string code) => await _context.Products.Include(p => p.ProductTypeEntity).FirstOrDefaultAsync(p => p.ProductTypeId == typeId && p.Code.ToLower() == code.ToLower() && p.Deleted == false);
        
        public async Task<bool> ProductExists(int typeId, string code) => await _context.Products.AnyAsync(p => p.ProductTypeId == typeId && p.Code.ToLower() == code.ToLower() && p.Deleted == false);
        
        public async Task<bool> ProductExists(int id) => await _context.Products.AnyAsync(p => p.Id == id && p.Deleted == false);

        public async Task<IEnumerable<ProductEntity>> GetProductsByPage(int items, int page, string query)
        {
            if (query == null)
                return await _context.Products.Include(p => p.ProductTypeEntity).Where(p => p.Deleted == false).Skip((page - 1) * items).Take(items).ToListAsync();
            else
                return await _context.Products.Include(p => p.ProductTypeEntity).Where(p => p.Deleted == false && (
                     p.Code.ToLower().Contains(query.ToLower()) ||
                     p.Description.ToLower().Contains(query.ToLower()) ||
                     p.ProductTypeEntity.Type.ToLower().Contains(query.ToLower())))
                    .Skip((page - 1) * items).Take(items).ToListAsync();
        }
         
        public async Task<IEnumerable<GetProductForRentDtoResponse>> GetProductsForRent(int productTypeId)
            => await _context.Products
                .Include(p => p.ProductTypeEntity).Where(p => p.Status == ProductStatus.ProductStatusEnum.ElementAt(0) && p.ProductTypeEntity.Id == productTypeId && p.Deleted == false)
                .Select(p => new GetProductForRentDtoResponse
                {
                    Code = p.Code,
                    Parts = p.Parts,
                    RentedParts = p.RentedParts
                }).ToListAsync();

        public async Task<IEnumerable<ProductEntity>> GetProductsByProductCodeList(List<string> productCodeList) => await _context.Products.Include(p => p.ProductTypeEntity).Where(p => productCodeList.Contains(p.Code)).ToListAsync();

        public async Task<ProductEntity> GetTheLastsCreated(int productTypeId) => await _context.Products.OrderByDescending(p => p.CreatedAt).FirstOrDefaultAsync(p => p.ProductTypeEntity.Id == productTypeId && p.Deleted == false);

        public async Task<int> UpdateProduct(ProductEntity productForUpdate)
        {
            _context.Products.Update(productForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}