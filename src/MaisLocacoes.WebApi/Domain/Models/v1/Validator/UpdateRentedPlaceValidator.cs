using FluentValidation;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;

namespace MaisLocacoes.WebApi.Domain.Models.v1.Validator
{
    public class UpdateRentedPlaceValidator : AbstractValidator<UpdateRentedPlaceRequest>
    {
        public UpdateRentedPlaceValidator()
        {
            RuleFor(rentedPlace => rentedPlace.Latitude)
                .Must(latitude => double.TryParse(latitude.ToString(), out var result))
                .WithMessage("Valor de Latitude se inserido deve ser válido")
                .When(rentedPlace => rentedPlace.Latitude != null);

            RuleFor(rentedPlace => rentedPlace.Longitude)
                .Must(longitude => double.TryParse(longitude.ToString(), out var result))
                .WithMessage("Valor de Longitude se inserido deve ser válido ")
                .When(rentedPlace => rentedPlace.Longitude != null);
        }
    }
}