using MaisLocacoes.WebApi.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class ProductWasteRepository : IProductWasteRepository
    {
        private readonly PostgreSqlContext _context;

        public ProductWasteRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<ProductWasteEntity> CreateProductWaste(ProductWasteEntity productWasteEntity)
        {
            await _context.ProductWastes.AddAsync(productWasteEntity);
            await _context.SaveChangesAsync();
            return productWasteEntity;
        }

        public async Task<ProductWasteEntity> GetById(int id) => await _context.ProductWastes.FirstOrDefaultAsync(p => p.Id == id && p.Deleted == false);

        public async Task<int> UpdateProductWaste(ProductWasteEntity productWasteForUpdate)
        {
            _context.ProductWastes.Update(productWasteForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}