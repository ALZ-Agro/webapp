using ALZAGRO.AppRendicionGastos.Application.Dtos;
using FluentValidation;

namespace ALZAGRO.AppRendicionGastos.Application.InputValidators {
    public class UserDtoValidator : AbstractValidator<UserDto> {

        public UserDtoValidator() {
            RuleFor(user => user.Username).NotEmpty()
               .WithMessage("Debe ingresar un nombre de usuario.");

            RuleFor(user => user.Username).Length(0, 100)
                .WithMessage("El campo usuario debe ser menor a 100 caracteres");

            RuleFor(user => user.Email).NotEmpty()
              .WithMessage("Debe ingresar un email.");

            RuleFor(user => user.Email).Length(0, 100)
                .WithMessage("El campo email debe ser menor a 100 caracteres");

            RuleFor(user => user.RoleId).NotEmpty()
                .WithMessage("Debe seleccionar un rol para el usuario.");
        }
    }
}