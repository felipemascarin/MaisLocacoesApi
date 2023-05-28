using MaisLocacoes.WebApi.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class ProductTuitionRepository : IProductTuitionRepository
    {
        private readonly PostgreSqlContext _context;

        public ProductTuitionRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<ProductTuitionEntity> CreateProductTuition(ProductTuitionEntity productTuitionEntity)
        {
            await _context.ProductTuitions.AddAsync(productTuitionEntity);
            await _context.SaveChangesAsync();
            return productTuitionEntity;
        }

        public async Task<ProductTuitionEntity> GetById(int id) => await _context.ProductTuitions.FirstOrDefaultAsync(p => p.Id == id && p.Deleted == false);

        public async Task<bool> ProductTuitionExists(int rentId, int productTypeId, string productCode) => await _context.ProductTuitions.AnyAsync(p => p.RentId == rentId && p.ProductTypeId == productTypeId && p.ProductCode.ToLower() == productCode.ToLower() && p.Deleted == false);

        public async Task<int> UpdateProductTuition(ProductTuitionEntity productTuitionForUpdate)
        {
            _context.ProductTuitions.Update(productTuitionForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}