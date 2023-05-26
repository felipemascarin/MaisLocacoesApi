using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Utils.Enum;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using System.Text.RegularExpressions;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class ClientValidator : AbstractValidator<ClientRequest>
    {
        public ClientValidator()
        {
            RuleFor(client => client.Type)
                .Must(type => !string.IsNullOrEmpty(type))
                .WithMessage("O Tipo de cliente é obrigatório")
                .MaximumLength(255)
                .WithMessage("O Tipo de cliente ultrapassou o limite máximo de caracteres");

            RuleFor(client => client.Type)
                .Must(type => ClientType.ClientTypesEnum.Contains(type.ToLower()))
                .WithMessage("O Tipo de cliente inserido não existe")
                .When(client => !string.IsNullOrEmpty(client.Type));

            //Dados desse contexto mais relacionados à Pessoa Fìsica:
            RuleFor(client => client.Cpf)
                .Must(cpf => !string.IsNullOrEmpty(cpf))
                .WithMessage("O CPF deve ser informado para pessoa física")
                .When(client => client.Type == ClientType.ClientTypesEnum.ElementAt(0));

            RuleFor(client => client.Cpf)
                .Must(cpf => DocumentValidator.IsCpf(cpf))
                .WithMessage("O CPF informado é inválido")
                .When(client => !string.IsNullOrEmpty(client.Cpf) &&
                client.Type == ClientType.ClientTypesEnum.ElementAt(0));

            RuleFor(client => client.Cpf)
                .Must(cpf => DocumentValidator.IsCpf(cpf))
                .WithMessage("O CPF se informado deve ser válido")
                .When(client => !string.IsNullOrEmpty(client.Cpf) &&
                client.Type == ClientType.ClientTypesEnum.ElementAt(1));

            RuleFor(client => client.ClientName)
                .Must(clientName => !string.IsNullOrEmpty(clientName))
                .WithMessage("O Nome do cliente deve ser informado para pessoa física")
                .MaximumLength(255)
                .WithMessage("O Nome do cliente ultrapassou o limite máximo de caracteres")
                .When(client => client.Type == ClientType.ClientTypesEnum.ElementAt(0));

            RuleFor(client => client.ClientName)
                .MaximumLength(255)
                .WithMessage("O Nome do cliente ultrapassou o limite máximo de caracteres")
                .When(client => !string.IsNullOrEmpty(client.ClientName));

            RuleFor(client => client.Rg)
                .Must(rg => long.TryParse(rg, out var result) && result > 0)
                .WithMessage("RG se informado deve conter somente números")
                .MaximumLength(18)
                .WithMessage("RG ultrapassou o limite máximo de caracteres")
                .When(client => !string.IsNullOrEmpty(client.Rg));

            RuleFor(client => client.Cel)
                .Must(cel => long.TryParse(cel, out var result) && result > 0)
                .WithMessage("Celular é obrigatório e deve conter somente números")
                .MaximumLength(18)
                .WithMessage("Celular ultrapassou o limite máximo de caracteres");

            RuleFor(client => client.Tel)
                .Must(tel => long.TryParse(tel, out var result) && result > 0)
                .WithMessage("Telefone se informado deve conter somente números")
                .MaximumLength(18)
                .WithMessage("Telefone ultrapassou o limite máximo de caracteres")
                .When(client => !string.IsNullOrEmpty(client.Tel));

            RuleFor(client => client.Email)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .WithMessage("Email se informado deve ter formato de um e-mail")
                .MaximumLength(255)
                .WithMessage("Email ultrapassou o limite máximo de caracteres")
                .When(client => !string.IsNullOrEmpty(client.Email));

            RuleFor(client => client.BornDate)
                .Must(bornDate => DateTime.TryParse(bornDate.ToString(), out var result))
                .WithMessage("A Data de nascimento se inserida deve ser uma data válida")
                .Must(bornDate => bornDate < DateTime.Now)
                .WithMessage("A Data de nascimento se inserida deve ser data passada")
                .When(client => client.BornDate != null);

            RuleFor(client => client.CivilStatus)
                .MaximumLength(255)
                .WithMessage("Estado civil ultrapassou o limite máximo de caracteres")
                .When(client => !string.IsNullOrEmpty(client.CivilStatus));

            RuleFor(client => client.Career)
                .MaximumLength(255)
                .WithMessage("Profissão ultrapassou o limite máximo de caracteres")
                .When(client => !string.IsNullOrEmpty(client.Career));


            //Dados desse contexto mais relacionados à Pessoa Jurídica:
            RuleFor(client => client.Cnpj)
                .Must(cnpj => string.IsNullOrEmpty(cnpj))
                .WithMessage("O CNPJ não deve ser informado para pessoa física")
                .When(client => client.Type == ClientType.ClientTypesEnum.ElementAt(0));

            RuleFor(client => client.Cnpj)
                .Must(cnpj => !string.IsNullOrEmpty(cnpj))
                .WithMessage("O CNPJ é obrigatório para Pessoa Jurídica")
                .When(client => client.Type == ClientType.ClientTypesEnum.ElementAt(1));

            RuleFor(client => client.Cnpj)
                .Must(cnpj => DocumentValidator.IsCnpj(cnpj))
                .WithMessage("O CNPJ informado é inválido")
                .When(client => !string.IsNullOrEmpty(client.Cnpj) &&
                client.Type == ClientType.ClientTypesEnum.ElementAt(1));

            RuleFor(client => client.CompanyName)
                .Must(companyName => string.IsNullOrEmpty(companyName))
                .WithMessage("Razão social não deve ser informada para pessoa física")
                .When(client => client.Type == ClientType.ClientTypesEnum.ElementAt(0));

            RuleFor(client => client.CompanyName)
                .MaximumLength(255)
                .WithMessage("Razão social ultrapassou o limite máximo de caracteres")
                .When(client => client.Type == ClientType.ClientTypesEnum.ElementAt(1));

            RuleFor(client => client.StateRegister)
                .Must(stateRegister => string.IsNullOrEmpty(stateRegister))
                .WithMessage("A Inscrição Estadual não deve ser informada para pessoa física")
                .When(client => client.Type == ClientType.ClientTypesEnum.ElementAt(0));

            RuleFor(client => client.StateRegister)
                .Must(stateRegister => long.TryParse(stateRegister, out var result) && result > 0)
                .WithMessage("Inscrição Estadual se informada deve conter somente números")
                .MaximumLength(18)
                .WithMessage("Inscrição Estadual ultrapassou o limite máximo de caracteres")
                .When(client => !string.IsNullOrEmpty(client.StateRegister) &&
                 client.Type == ClientType.ClientTypesEnum.ElementAt(1));

            RuleFor(client => client.FantasyName)
                .Must(fantasyName => string.IsNullOrEmpty(fantasyName))
                .WithMessage("O Nome fantasia não deve ser informado para pessoa física")
                .When(client => client.Type == ClientType.ClientTypesEnum.ElementAt(0));

            RuleFor(client => client.FantasyName)
                .Must(fantasyName => !string.IsNullOrEmpty(fantasyName))
                .WithMessage("O Nome Fantasia é obrigatório para Pessoa Jurídica")
                .MaximumLength(255)
                .WithMessage("O Nome Fantasia ultrapassou o limite máximo de caracteres")
                .When(client => client.Type == ClientType.ClientTypesEnum.ElementAt(1));

            RuleFor(client => client.Segment)
                .Must(segment => string.IsNullOrEmpty(segment))
                .WithMessage("O Segmento da empresa não deve ser informado para pessoa física")
                .When(client => client.Type == ClientType.ClientTypesEnum.ElementAt(0));

            RuleFor(client => client.Segment)
                .MaximumLength(255)
                .WithMessage("Profissão ultrapassou o limite máximo de caracteres")
                .When(client => !string.IsNullOrEmpty(client.Segment) &&
                client.Type == ClientType.ClientTypesEnum.ElementAt(1));


            //URL's:
            RuleFor(client => client.CpfDocumentUrl)
                .MaximumLength(2048)
                .WithMessage("CpfDocumentUrl ultrapassou o limite máximo de caracteres")
                .When(client => !string.IsNullOrEmpty(client.CpfDocumentUrl));

            RuleFor(client => client.CnpjDocumentUrl)
                .MaximumLength(2048)
                .WithMessage("CnpjDocumentUrl ultrapassou o limite máximo de caracteres")
                .When(client => !string.IsNullOrEmpty(client.CnpjDocumentUrl));

            RuleFor(client => client.AddressDocumentUrl)
                .MaximumLength(2048)
                .WithMessage("AddressDocumentUrl ultrapassou o limite máximo de caracteres")
                .When(client => !string.IsNullOrEmpty(client.AddressDocumentUrl));

            RuleFor(client => client.ClientPictureUrl)
                .MaximumLength(2048)
                .WithMessage("ClientPictureUrl ultrapassou o limite máximo de caracteres")
                .When(client => !string.IsNullOrEmpty(client.ClientPictureUrl));


            RuleFor(client => client.Address).SetValidator(new AddressValidator());
        }
    }
}