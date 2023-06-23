using MaisLocacoes.WebApi.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace MaisLocacoes.WebApi._3_Repository.v1.Repository
{
    public class ProductTuitionValueRepository : IProductTuitionValueRepository
    {
        private readonly PostgreSqlContext _context;

        public ProductTuitionValueRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<ProductTuitionValueEntity> CreateProductTuitionValue(ProductTuitionValueEntity productTuitionValueEntity)
        {
            await _context.ProductTuitionValues.AddAsync(productTuitionValueEntity);
            _context.SaveChanges();
            return productTuitionValueEntity;
        }

        public async Task<ProductTuitionValueEntity> GetById(int id) => await _context.ProductTuitionValues.Include(p => p.ProductTypeEntity).FirstOrDefaultAsync(p => p.Id == id);

        public async Task<bool> ProductTuitionValueExists(int productTypeId, int quantityPeriod, string timePeriod) => await _context.ProductTuitionValues.AnyAsync(p => p.ProductTypeId == productTypeId && p.QuantityPeriod == quantityPeriod && p.TimePeriod == timePeriod);
     
        public async Task<IEnumerable<ProductTuitionValueEntity>> GetAllByProductTypeId(int productTypeId) => await _context.ProductTuitionValues.Where(p => p.ProductTypeId == productTypeId).OrderBy(p => p.TimePeriod).ToListAsync();

        public async Task<int> UpdateProductTuitionValue(ProductTuitionValueEntity productTuitionValueForUpdate)
        {
            _context.ProductTuitionValues.Update(productTuitionValueForUpdate);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteProductTuitionValue(ProductTuitionValueEntity productTuitionValueForDelete)
        {
            _context.ProductTuitionValues.Remove(productTuitionValueForDelete);
            return await _context.SaveChangesAsync();
        }
    }
}