using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;
using TimeZoneConverter;

namespace Service.v1.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;

        public ClientService(IClientRepository clientRepository,
            IMapper mapper,
            IAddressService addressService,
            IHttpContextAccessor httpContextAccessor)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
            _addressService = addressService;
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateClientResponse> CreateClient(CreateClientRequest clientRequest)
        {
            ClientEntity existsClient;
            if (string.IsNullOrEmpty(clientRequest.Cnpj))
                existsClient = await _clientRepository.GetByCpf(clientRequest.Cpf);
            else
                existsClient = await _clientRepository.GetByCnpj(clientRequest.Cnpj);

            if (existsClient != null)
                throw new HttpRequestException("Cliente já cadastrado", null, HttpStatusCode.BadRequest);

            var addressResponse = await _addressService.CreateAddress(clientRequest.Address);

            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            clientRequest = TimeZoneConverter<CreateClientRequest>.ConvertToTimeZoneLocal(clientRequest, _timeZone);

            var clientEntity = _mapper.Map<ClientEntity>(clientRequest);

            clientEntity.AddressId = addressResponse.Id;
            clientEntity.BornDate = clientEntity.BornDate.Value.Date;
            clientEntity.CreatedBy = _email;
            clientEntity.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            clientEntity = await _clientRepository.CreateClient(clientEntity);

            var clientResponse = _mapper.Map<CreateClientResponse>(clientEntity);

            return clientResponse;
        }

        public async Task<GetClientByIdResponse> GetClientById(int id)
        {
            var clientEntity = await _clientRepository.GetById(id) ??
                throw new HttpRequestException("Cliente não encontrado", null, HttpStatusCode.NotFound);

            var clientResponse = _mapper.Map<GetClientByIdResponse>(clientEntity);

            return clientResponse;
        }

        public async Task<GetClientByIdDetailsResponse> GetClientByIdDetails(int id)
        {
            var clientEntity = await _clientRepository.GetByIdDetails(id) ??
                throw new HttpRequestException("Cliente não encontrado", null, HttpStatusCode.NotFound);

            var clientResponse = _mapper.Map<GetClientByIdDetailsResponse>(clientEntity);

            return clientResponse;
        }

        public async Task<GetClientByCpfResponse> GetClientByCpf(string cpf)
        {
            var clientEntity = await _clientRepository.GetByCpf(cpf) ??
                throw new HttpRequestException("Cliente não encontrado", null, HttpStatusCode.NotFound);

            var clientResponse = _mapper.Map<GetClientByCpfResponse>(clientEntity);

            return clientResponse;
        }

        public async Task<GetClientByCnpjResponse> GetClientByCnpj(string cnpj)
        {
            var clientEntity = await _clientRepository.GetByCnpj(cnpj) ??
                throw new HttpRequestException("Cliente não encontrado", null, HttpStatusCode.NotFound);

            var clientResponse = _mapper.Map<GetClientByCnpjResponse>(clientEntity);

            return clientResponse;
        }

        public async Task<IEnumerable<GetClientsByPageResponse>> GetClientsByPage(int items, int page, string query)
        {
            if (items <= 0 || page <= 0)
                throw new HttpRequestException("Informe o valor da página e a quantidade de itens corretamente", null, HttpStatusCode.BadRequest);

            var clientsEntityList = await _clientRepository.GetClientsByPage(items, page, query);

            var clientsResponseList = _mapper.Map<IEnumerable<GetClientsByPageResponse>>(clientsEntityList);

            return clientsResponseList;
        }

        public async Task<IEnumerable<GetClientsForRentResponse>> GetClientsForRent()
        {
            var clientForRentDtoResponse = await _clientRepository.GetClientsForRent();

            var clientForRentResponse = new List<GetClientsForRentResponse>();

            foreach (var item in clientForRentDtoResponse)
            {
                if (!string.IsNullOrEmpty(item.Cpf) && string.IsNullOrEmpty(item.Cnpj))
                    clientForRentResponse.Add(new GetClientsForRentResponse
                    {
                        Id = item.Id,
                        Name = item.ClientName,
                        DocumentNumber = item.Cpf
                    });

                if (!string.IsNullOrEmpty(item.Cnpj))
                    clientForRentResponse.Add(new GetClientsForRentResponse
                    {
                        Id = item.Id,
                        Name = item.FantasyName,
                        DocumentNumber = item.Cnpj
                    });
            }

            return clientForRentResponse.OrderBy(c => c.Name);
        }

        public async Task<bool> UpdateClient(UpdateClientRequest clientRequest, int id)
        {
            var clientForUpdate = await _clientRepository.GetById(id) ??
                    throw new HttpRequestException("Cliente não encontrado", null, HttpStatusCode.NotFound);

            if (!string.IsNullOrEmpty(clientRequest.Cnpj))
            {
                if (clientRequest.Cnpj != clientForUpdate.Cnpj)
                {
                    var existsCnpj = await _clientRepository.GetByCnpj(clientRequest.Cnpj);
                    if (existsCnpj != null)
                        throw new HttpRequestException("O CNPJ novo já está cadastrado em outro cliente", null, HttpStatusCode.BadRequest);
                }
            }

            if (string.IsNullOrEmpty(clientRequest.Cnpj))
            {
                if (clientRequest.Cpf != clientForUpdate.Cpf)
                {
                    var existsCpf = await _clientRepository.GetByCpf(clientRequest.Cpf);
                    if (existsCpf != null)
                        throw new HttpRequestException("O CPF novo já está cadastrado em outro cliente", null, HttpStatusCode.BadRequest);
                }
            }

            clientForUpdate.Type = clientRequest.Type;
            clientForUpdate.Cpf = clientRequest.Cpf;
            clientForUpdate.Rg = clientRequest.Rg;
            clientForUpdate.Cnpj = clientRequest.Cnpj;
            clientForUpdate.CompanyName = clientRequest.CompanyName;
            clientForUpdate.ClientName = clientRequest.ClientName;
            clientForUpdate.StateRegister = clientRequest.StateRegister;
            clientForUpdate.FantasyName = clientRequest.FantasyName;
            clientForUpdate.Cel = clientRequest.Cel;
            clientForUpdate.Tel = clientRequest.Tel;
            clientForUpdate.Email = clientRequest.Email;
            clientForUpdate.BornDate = clientRequest.BornDate;
            clientForUpdate.Career = clientRequest.Career;
            clientForUpdate.CivilStatus = clientRequest.CivilStatus;
            clientForUpdate.Segment = clientRequest.Segment;
            clientForUpdate.CpfDocumentUrl = clientRequest.CpfDocumentUrl;
            clientForUpdate.CnpjDocumentUrl = clientRequest.CnpjDocumentUrl;
            clientForUpdate.AddressDocumentUrl = clientRequest.AddressDocumentUrl;
            clientForUpdate.ClientPictureUrl = clientRequest.ClientPictureUrl;
            clientForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            clientForUpdate.UpdatedBy = _email;

            if (!await _addressService.UpdateAddress(clientRequest.Address, clientForUpdate.AddressEntity.Id))
                throw new HttpRequestException("Não foi possível salvar endereço antes de salvar o cliente", null, HttpStatusCode.InternalServerError);

            if (await _clientRepository.UpdateClient(clientForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> UpdateStatus(string status, int id)
        {
            var clientForUpdate = await _clientRepository.GetById(id) ??
                throw new HttpRequestException("Cliente não encontrado", null, HttpStatusCode.NotFound);

            clientForUpdate.Status = status;
            clientForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            clientForUpdate.UpdatedBy = _email;

            if (await _clientRepository.UpdateClient(clientForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var clientForDelete = await _clientRepository.GetById(id) ??
               throw new HttpRequestException("Cliente não encontrado", null, HttpStatusCode.NotFound);

            clientForDelete.Deleted = true;
            clientForDelete.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            clientForDelete.UpdatedBy = _email;

            if (await _clientRepository.UpdateClient(clientForDelete) > 0) return true;
            else return false;
        }
    }
}