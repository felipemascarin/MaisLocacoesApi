using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class RentedPlaceValidator : AbstractValidator<RentedPlaceRequest>
    {
        public RentedPlaceValidator()
        {
            RuleFor(rentedPlace => rentedPlace.ProductId)
               .Must(productId => int.TryParse(productId.ToString(), out var result) &&
                result > 0)
               .WithMessage("Id do tipo de produto deve ser um número inteiro maior que 0");

            RuleFor(rentedPlace => rentedPlace.RentId)
               .Must(rentId => int.TryParse(rentId.ToString(), out var result) &&
                result > 0)
               .WithMessage("Id da locação se inserido deve ser um número inteiro maior que 0")
               .When(rentedPlace => rentedPlace.RentId != null);

            RuleFor(rentedPlace => rentedPlace.QgId)
              .Must(qgId => int.TryParse(qgId.ToString(), out var result) &&
               result > 0)
              .WithMessage("Id do QG se inserido deve ser um número inteiro maior que 0")
              .When(rentedPlace => rentedPlace.QgId != null);

            RuleFor(rentedPlace => rentedPlace.QgId)
             .Null()
             .WithMessage("Id do QG deve ser nulo se inserido Id da locação")
             .When(rentedPlace => rentedPlace.RentId != null);

            RuleFor(rentedPlace => rentedPlace.RentId)
             .Null()
             .WithMessage("Id da locação deve ser nulo se inserido Id do QG")
             .When(rentedPlace => rentedPlace.QgId != null);

            RuleFor(rentedPlace => rentedPlace.Latitude)
                .Must(latitude => double.TryParse(latitude.ToString(), out var result))
                .WithMessage("Valor de Latitude se inserido deve ser válido")
                .When(rentedPlace => rentedPlace.Latitude != null);

            RuleFor(rentedPlace => rentedPlace.Longitude)
                .Must(longitude => double.TryParse(longitude.ToString(), out var result))
                .WithMessage("Valor de Longitude se inserido deve ser válido ")
                .When(rentedPlace => rentedPlace.Longitude != null);

            RuleFor(rentedPlace => rentedPlace.ArrivalDate)
                .Must(arrivalDate => DateTime.TryParse(arrivalDate.ToString(), out var result))
                .WithMessage("A Data de partida se inserida deve ser uma data válida")
                .When(client => client.ArrivalDate != null);

            RuleFor(rentedPlace => rentedPlace.ProductParts)
               .Must(productParts => int.TryParse(productParts.ToString(), out var result) &&
                result > 0)
               .WithMessage("A quantidade de peças do produto deve ser um número inteiro maior que 0");
        }
    }
}