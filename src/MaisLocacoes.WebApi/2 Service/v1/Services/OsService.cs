using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class OsService : IOsService
    {
        private readonly IOsRepository _osRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OsService(IOsRepository osRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _osRepository = osRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> DeleteById(int id)
        {
            var osForDelete = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota de serviço não encontrada", null, HttpStatusCode.NotFound);

            osForDelete.Deleted = true;
            osForDelete.UpdatedAt = System.DateTime.UtcNow;
            osForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _osRepository.UpdateOs(osForDelete) > 0) return true;
            else return false;
        }
    }
}