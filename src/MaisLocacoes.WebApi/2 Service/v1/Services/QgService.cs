using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.IRepository;
using Repository.v1.Repository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class QgService : IQgService
    {
        private readonly IQgRepository _qgRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QgService(IQgRepository qgRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _qgRepository = qgRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        //public async Task<bool> DeleteById(int id)
        //{
        //    var qgForDelete = await _qgRepository.GetById(id) ??
        //        throw new HttpRequestException("Mensalidade não encontrada", null, HttpStatusCode.NotFound);

        //    qgForDelete.Deleted = true;
        //    qgForDelete.UpdatedAt = System.DateTime.UtcNow;
        //    qgForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

        //    if (await _qgRepository.UpdateQg(qgForDelete) > 0) return true;
        //    else return false;
        //}
    }
}