using ALZAGRO.AppRendicionGastos.Application.Dtos;
using FluentValidation;

namespace ALZAGRO.AppRendicionGastos.Application.InputValidators {
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto> {

        public ChangePasswordDtoValidator() {

            RuleFor(changePassword => changePassword.CurrentPassword).NotEmpty()
                   .WithMessage("Debe ingresar su contraseña actual.");

            RuleFor(changePassword => changePassword.CurrentPassword).Length(6,10)
                   .WithMessage("El campo contraseña actual debe ser mayor a 6 caracteres.");

            RuleFor(changePassword => changePassword.NewPassword).NotEmpty()
                  .WithMessage("Debe ingresar una contraseña nueva.");

            RuleFor(changePassword => changePassword.NewPassword).Length(6, 10)
                  .WithMessage("El campo contraseña debe ser mayor a 6 caracteres.");

            RuleFor(changePassword => changePassword.ConfirmPassword).NotEmpty()
                  .WithMessage("Debe ingresar confirmaci&oacute;n de contraseña.");

            RuleFor(changePassword => changePassword.ConfirmPassword).Length(6, 10)
                  .WithMessage("El campo confirmar contraseña debe ser mayor a 6 caracteres.");

            RuleFor(changePassword => changePassword.ConfirmPassword).Equal(changePassword=>changePassword.NewPassword)
                  .WithMessage("Las nuevas contraseñas no coinciden.");
        }
    }
}