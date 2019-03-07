using Servidor.Helpers;
using System.Data.Entity;

namespace Servidor.Models
{
    public class AxsisSeedInitializer : CreateDatabaseIfNotExists<Axsis>
    {
        protected override void Seed(Axsis context)
        {
            //Usuarios por default

            string vCode, pass;

            (pass, vCode) = PasswordHelper.GenerateNewEncodedPassword("Pa$$w0rd2560");

            Usuario admin = new Usuario
            {
                NombreUsuario = "administrador",
                Contrasena = pass,
                Email = "admin@correo.com",
                Estatus = true,
                Sexo = "masculino",
                VCode = vCode
            };

            context.Usuarios.Add(admin);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}