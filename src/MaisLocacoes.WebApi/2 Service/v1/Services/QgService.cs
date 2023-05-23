using Repository.v1.IRepository;
using Service.v1.IServices;

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
    }
}