using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get.UserSchema;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity.UserSchema;
using Repository.v1.IRepository.UserSchema;
using Service.v1.IServices.UserSchema;
using System.Net;

namespace Service.v1.Services.UserSchema
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateUserResponse> CreateUser(UserRequest userRequest)
        {
            var existsUser = await _userRepository.UserExists(userRequest.Email, userRequest.Cpf);

            if (existsUser)
                throw new HttpRequestException("Cpf ou e-mail de usuário já cadastrado", null, HttpStatusCode.BadRequest);

            var userEntity = _mapper.Map<UserEntity>(userRequest);

            userEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            userEntity = await _userRepository.CreateUser(userEntity);

            return _mapper.Map<CreateUserResponse>(userEntity);
        }

        public async Task<GetUserResponse> GetByEmail(string email)
        {
            var userEntity = await _userRepository.GetByEmail(email) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            var userResponse = _mapper.Map<GetUserResponse>(userEntity);

            return userResponse;
        }
    }
}