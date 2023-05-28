using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientService(IClientRepository clientRepository,
            IMapper mapper,
            IAddressService addressService,
            IHttpContextAccessor httpContextAccessor)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
            _addressService = addressService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ClientResponse> CreateClient(ClientRequest clientRequest)
        {
                ClientEntity existsClient;
                if (string.IsNullOrEmpty(clientRequest.Cnpj))
                    existsClient = await _clientRepository.GetByCpf(clientRequest.Cpf);
                else
                    existsClient = await _clientRepository.GetByCnpj(clientRequest.Cnpj);

                if (existsClient != null)
                    throw new HttpRequestException("Cliente já cadastrado", null, HttpStatusCode.BadRequest);

                var addressResponse = await _addressService.CreateAddress(clientRequest.Address);

                var clientEntity = _mapper.Map<ClientEntity>(clientRequest);

                clientEntity.AddressId = addressResponse.Id;
                clientEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

                clientEntity = await _clientRepository.CreateClient(clientEntity);

                var clientResponse = _mapper.Map<ClientResponse>(clientEntity);
                clientResponse.Address = addressResponse;

                return clientResponse;
        }

        public async Task<ClientResponse> GetById(int id)
        {
                var clientEntity = await _clientRepository.GetById(id) ??
                    throw new HttpRequestException("Cliente não encontrado", null, HttpStatusCode.NotFound);

                var clientAddressResponse = _mapper.Map<AddressResponse>(clientEntity.AddressEntity);

                var clientResponse = _mapper.Map<ClientResponse>(clientEntity);

                clientResponse.Address = clientAddressResponse;

                return clientResponse;
        }

        public async Task<IEnumerable<ClientResponse>> GetClientsByPage(int items, int page, string query)
        {
                if (items <= 0 || page <= 0)
                    throw new HttpRequestException("Informe o valor da página e a quantidade de itens corretamente", null, HttpStatusCode.BadRequest);

                var clientsEntityList = await _clientRepository.GetClientsByPage(items, page, query);

                var clientsEntityListLenght = clientsEntityList.ToList().Count;

                var clientsResponseList = _mapper.Map<IEnumerable<ClientResponse>>(clientsEntityList);

                for (int i = 0; i < clientsEntityListLenght; i++)
                {
                    clientsResponseList.ElementAt(i).Address = _mapper.Map<AddressResponse>(clientsEntityList.ElementAt(i).AddressEntity);
                }

                return clientsResponseList;
        }

        public async Task<IEnumerable<GetClientForRentResponse>> GetClientsForRent()
        {
                var clientForRentDtoResponse = await _clientRepository.GetClientsForRent();

                var clientForRentResponse = new List<GetClientForRentResponse>();

                foreach (var item in clientForRentDtoResponse)
                {
                    if (!string.IsNullOrEmpty(item.Cpf) && string.IsNullOrEmpty(item.Cnpj))
                        clientForRentResponse.Add(new GetClientForRentResponse
                        {
                            Id = item.Id,
                            Name = item.ClientName,
                            DocumentNumber = item.Cpf
                        });

                    if (!string.IsNullOrEmpty(item.Cnpj) && string.IsNullOrEmpty(item.Cpf))
                        clientForRentResponse.Add(new GetClientForRentResponse
                        {
                            Id = item.Id,
                            Name = item.FantasyName,
                            DocumentNumber = item.Cnpj
                        });
                }

                return clientForRentResponse.OrderBy(c => c.Name);
        }

        public async Task<bool> UpdateClient(ClientRequest clientRequest, int id)
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
                clientForUpdate.UpdatedAt = System.DateTime.UtcNow;
                clientForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

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
                clientForUpdate.UpdatedAt = System.DateTime.UtcNow;
                clientForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _clientRepository.UpdateClient(clientForUpdate) > 0) return true;
                else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var clientForDelete = await _clientRepository.GetById(id) ??
               throw new HttpRequestException("Cliente não encontrado", null, HttpStatusCode.NotFound);

            clientForDelete.Deleted = true;
            clientForDelete.UpdatedAt = System.DateTime.UtcNow;
            clientForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _clientRepository.UpdateClient(clientForDelete) > 0) return true;
            else return false;
        }
    }
}