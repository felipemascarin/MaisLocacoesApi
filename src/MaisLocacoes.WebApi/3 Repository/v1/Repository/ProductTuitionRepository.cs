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

        public async Task<ProductTuitionEntity> GetById(int id) => await _context.ProductTuitions.Include(p => p.RentEntity).Include(p => p.RentEntity.AddressEntity).FirstOrDefaultAsync(p => p.Id == id && p.Deleted == false);

        public async Task<IEnumerable<ProductTuitionEntity>> GetAllByRentId(int rentId) => await _context.ProductTuitions.Where(p => p.RentId == rentId && p.Deleted == false).OrderBy(p => p.FinalDateTime).ToListAsync();

        public async Task<IEnumerable<ProductTuitionEntity>> GetAllByProductTypeCode(int productTypeId, string productCode) => await _context.ProductTuitions.Include(p => p.RentEntity).Include(p => p.RentEntity.AddressEntity).Where(p => p.ProductTypeId == productTypeId && p.ProductCode == productCode && p.Deleted == false).OrderBy(p => p.InitialDateTime).ToListAsync();

        public async Task<bool> ProductTuitionExists(int rentId, int productTypeId, string productCode) => await _context.ProductTuitions.AnyAsync(p => p.RentId == rentId && p.ProductTypeId == productTypeId && p.ProductCode.ToLower() == productCode.ToLower() && p.Deleted == false);

        public async Task<int> UpdateProductTuition(ProductTuitionEntity productTuitionForUpdate)
        {
            _context.ProductTuitions.Update(productTuitionForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}