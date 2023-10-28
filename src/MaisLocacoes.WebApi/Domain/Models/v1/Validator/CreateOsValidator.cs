using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class CreateOsValidator : AbstractValidator<CreateOsRequest>
    {
        public CreateOsValidator()
        {
            RuleFor(os => os.Type)
                .Must(type => !string.IsNullOrEmpty(type))
                .WithMessage("O Tipo de nota de serviço é obrigatório")
                .MaximumLength(255)
                .WithMessage("O Tipo de nota de serviço ultrapassou o limite máximo de caracteres");

            RuleFor(os => os.Type)
                .Must(type => OsTypes.OsTypesEnum.Contains(type.ToLower()))
                .WithMessage("O Tipo de nota de serviço inserido não existe")
                .When(os => !string.IsNullOrEmpty(os.Type));

            RuleFor(os => os.DeliveryCpf)
                .Must(deliveryCpf => DocumentValidator.IsCpf(deliveryCpf))
                .WithMessage("O CPF do intregador inserido é inválido")
                .When(os => !string.IsNullOrEmpty(os.DeliveryCpf));

            RuleFor(os => os.ProductTuitionId)
                .Must(productTuitionId => int.TryParse(productTuitionId.ToString(), out var result) &&
                 result > 0)
                .WithMessage("Id do ProductTuition deve ser um número inteiro maior que 0");

            RuleFor(os => os.FinalDateTime)
                .Null()
                .WithMessage("A data final se inserida deve ser maior que a Data de início")
                .When(os => os.FinalDateTime < os.InitialDateTime);

            RuleFor(os => os.DeliveryObservation)
               .MaximumLength(1000)
               .WithMessage("Observação do intregador ultrapassou o limite máximo de caracteres")
               .When(os => !string.IsNullOrEmpty(os.DeliveryObservation));
        }
    }
}