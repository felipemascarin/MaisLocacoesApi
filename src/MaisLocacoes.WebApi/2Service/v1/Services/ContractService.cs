using AutoMapper;
using MaisLocacoes.WebApi._2Service.v1.IServices;
using MaisLocacoes.WebApi._3Repository.v1.Entity;
using MaisLocacoes.WebApi._3Repository.v1.IRepository;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Contract;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.IRepository;
using Repository.v1.IRepository.UserSchema;
using System.Net;
using TimeZoneConverter;

namespace MaisLocacoes.WebApi._2Service.v1.Services
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly IRentRepository _rentRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;
        private readonly string _cnpj;

        public ContractService(IContractRepository contractRepository,
            IRentRepository rentRepository,
            ICompanyRepository companyRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _contractRepository = contractRepository;
            _rentRepository = rentRepository;
            _companyRepository = companyRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
            _cnpj = JwtManager.GetCnpjByToken(_httpContextAccessor);
        }

        public async Task<CreateContractResponse> CreateContract(CreateContractRequest contractRequest)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            contractRequest = TimeZoneConverter<CreateContractRequest>.ConvertToTimeZoneLocal(contractRequest, _timeZone);

            if (!await _rentRepository.RentExists(contractRequest.RentId.Value))
                throw new HttpRequestException("Locação não encontrada", null, HttpStatusCode.NotFound);

            var contractEntity = _mapper.Map<ContractEntity>(contractRequest);

            contractEntity.GuidId = Guid.NewGuid();
            contractEntity.CreatedBy = _email;
            contractEntity.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            contractEntity = await _contractRepository.CreateContract(contractEntity);

            var contractResponse = _mapper.Map<CreateContractResponse>(contractEntity);

            return contractResponse;
        }

        public async Task<GetContractByIdResponse> GetContractById(int id)
        {
            var contractEntity = await _contractRepository.GetById(id) ??
                throw new HttpRequestException("Contrato não encontrado", null, HttpStatusCode.NotFound);

            var contractResponse = _mapper.Map<GetContractByIdResponse>(contractEntity);

            return contractResponse;
        }

        public async Task<IEnumerable<GetAllContractsResponse>> GetAllContracts()
        {
            var contractsEntityList = await _contractRepository.GetAll();

            var contractsResponseList = _mapper.Map<IEnumerable<GetAllContractsResponse>>(contractsEntityList);

            return contractsResponseList;
        }

        public async Task<GetContractInfoByRentIdResponse> GetContractInfoByRentId(int rentId)
        {
            var contractEntity = await _contractRepository.GetContractInfoByRentId(rentId);

            var companyEntity = await _companyRepository.GetByCnpj(_cnpj);

            var contractInfoResponse = _mapper.Map<GetContractInfoByRentIdResponse>(contractEntity);
            contractInfoResponse.Company = _mapper.Map<GetContractInfoByRentIdResponse.ContractCompany>(companyEntity);

            contractInfoResponse.Rent.InitialRentDate = contractInfoResponse.ProductTuitions.OrderBy(p => p.InitialDateTime).First().InitialDateTime;
            contractInfoResponse.Rent.FinalRentDate = contractInfoResponse.ProductTuitions.OrderByDescending(p => p.FinalDateTime).First().FinalDateTime;

            return contractInfoResponse;
        }

        public async Task UpdateContract(UpdateContractRequest contractRequest, int id)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            contractRequest = TimeZoneConverter<UpdateContractRequest>.ConvertToTimeZoneLocal(contractRequest, _timeZone);

            var contractForUpdate = await _contractRepository.GetById(id) ??
                throw new HttpRequestException("Contrato não encontrado", null, HttpStatusCode.NotFound);

            if (contractRequest.RentId != contractForUpdate.RentId)
            {
                if (!await _rentRepository.RentExists(contractRequest.RentId.Value))
                    throw new HttpRequestException("Locação não encontrada", null, HttpStatusCode.NotFound);
            }

            contractForUpdate.RentId = contractRequest.RentId.Value;
            contractForUpdate.UrlSignature = contractRequest.UrlSignature;
            contractForUpdate.SignedAt = contractRequest.SignedAt;
            contractForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            contractForUpdate.UpdatedBy = _email;

            await _contractRepository.UpdateContract(contractForUpdate);
        }

        public async Task DeleteById(int id)
        {
            var contractForDelete = await _contractRepository.GetById(id) ??
                throw new HttpRequestException("Contrato não encontrado", null, HttpStatusCode.NotFound);

            contractForDelete.Deleted = true;
            contractForDelete.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            contractForDelete.UpdatedBy = _email;

            await _contractRepository.UpdateContract(contractForDelete);
        }
    }
}
