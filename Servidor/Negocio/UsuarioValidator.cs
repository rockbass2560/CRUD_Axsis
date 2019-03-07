using FluentValidation;
using Servidor.Models;

namespace Servidor.Negocio
{
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator()
        {
            RuleFor(u => u.NombreUsuario).NotEmpty()
                .WithMessage("El nombre de usuario no debe estar vacio")
                .MinimumLength(7)
                .WithMessage("El nombre de usuario debe tener al menos 7 caracters");

            RuleFor(u => u.Contrasena).NotEmpty()
                .WithMessage("La contraseña no debe estar vacia")
                .MinimumLength(10)
                .WithMessage("La contraseña debe tener al menos 10 caracteres")
                .Matches("(?=.*?[0-9])(?=.*?[a-z])(?=.*[^0-9A-Za-z])(?=.*?[A-Z])")
                .WithMessage("La contraseña debe tener al menos 1 letra minuscula, 1 letra mayuscula, 1 numero y 1 caracter especial");

            RuleFor(u => u.Email).NotEmpty()
                .WithMessage("El Email no debe estar vacio")
                .EmailAddress()
                .WithMessage("El formato del email no es correcto");

            RuleFor(u => u.FechaCreacion).NotEmpty()
                .WithMessage("La fecha de creacion no debe de estar vacia");

            RuleFor(u => u.Sexo).NotEmpty()
                .WithMessage("El sexo no debe estar vacio")
                .Must(s => s=="femenino" || s=="masculino" )
                .WithMessage("Solo los valores femenino o masculino son validos");
        }
    }
}