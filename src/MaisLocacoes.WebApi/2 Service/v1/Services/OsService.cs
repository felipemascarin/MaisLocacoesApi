using Repository.v1.IRepository;
using Service.v1.IServices;

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
    }
}