using Servidor.Helpers;
using Servidor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servidor.Negocio
{
    public class UsuarioDO
    {
        private Axsis axsis;
        private UsuarioValidator usuarioValidator;

        public UsuarioDO()
        {
            axsis = new Axsis();
            usuarioValidator = new UsuarioValidator();
        }

        public dynamic Obtener(int id)
        {
            var usuario = axsis.Usuarios.FirstOrDefault(u => u.ID == id);
            return new
            {
                usuario.NombreUsuario,
                usuario.Email,
                usuario.Estatus,
                usuario.FechaCreacion,
                usuario.ID,
                usuario.Sexo
            };
        }

        public IQueryable<dynamic> ObtenerTodos()
        {
            return axsis.Usuarios.Select(
                usuario => new
                {
                    usuario.NombreUsuario,
                    usuario.Email,
                    usuario.Estatus,
                    usuario.FechaCreacion,
                    usuario.ID,
                    usuario.Sexo
                }
            );
        }

        public (bool, IEnumerable<string>) Registrar(Usuario usuario)
        {
            bool result = false;

            usuario.Estatus = true;
            var validationResults = usuarioValidator.Validate(usuario);
            List<string> errores = new List<string>();

            if (validationResults.IsValid)
            {
                //Validar que no exista otro usuario igual
                Usuario usuarioBD = axsis.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuario.NombreUsuario);

                if (usuarioBD != null) //existe un usuario con el mismo nombre
                {
                    errores.Add("El nombre de usuario ya existe en el sistema");
                    return (false, errores);
                }

                usuarioBD = axsis.Usuarios.FirstOrDefault(u => u.Email == usuario.Email);

                if (usuarioBD != null)
                {
                    errores.Add("El email ya existe en el sistema");
                    return (false, errores);
                }

                //Encriptar contraseña
                (usuario.Contrasena, usuario.VCode) = PasswordHelper.GenerateNewEncodedPassword(usuario.Contrasena);

                axsis.Usuarios.Add(usuario);
                result = axsis.SaveChanges() > 0;
            }
            else
            {
                errores.AddRange(validationResults.Errors.Select(e => e.ErrorMessage));
            }
            
            return (result, errores);
        }

        public (bool, IEnumerable<string>) ActualizarUsuario(Usuario usuario)
        {
            bool result = false;
            List<string> errores = new List<string>();
            var usuarioBD = axsis.Usuarios.Find(usuario.ID);

            if (usuarioBD != null)
            {

                bool mismoCorreo = usuarioBD.Email == usuario.Email;
                bool mismoUsuario = usuarioBD.NombreUsuario == usuario.NombreUsuario;

                //Sustituimos nuevos valores
                usuarioBD.Contrasena = usuario.Contrasena;
                usuarioBD.Sexo = usuario.Sexo;
                usuarioBD.Email = usuario.Email;
                usuarioBD.NombreUsuario = usuario.NombreUsuario;

                var validationResults = usuarioValidator.Validate(usuarioBD);

                if (validationResults.IsValid)
                {

                    if (!mismoUsuario)
                    {
                        //Validar que no exista otro usuario igual
                        Usuario usarioResult = axsis.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuario.NombreUsuario);

                        if (usarioResult != null) //existe un usuario con el mismo nombre
                        {
                            errores.Add("El nombre de usuario ya existe en el sistema");
                            return (false, errores);
                        }
                    }

                    if (!mismoCorreo)
                    {
                        Usuario usarioResult = axsis.Usuarios.FirstOrDefault(u => u.Email == usuario.Email);

                        if (usarioResult != null)
                        {
                            errores.Add("El email ya existe en el sistema");
                            return (false, errores);
                        }
                    }

                    //Se encripta nueva contraseña
                    String nuevoSalty;                    
                    (usuarioBD.Contrasena, nuevoSalty) = PasswordHelper.GenerateNewEncodedPassword(usuarioBD.Contrasena);
                    usuarioBD.VCode = nuevoSalty;

                    axsis.Entry(usuarioBD).State = System.Data.Entity.EntityState.Modified;
                    result = axsis.SaveChanges() > 0;
                }
                else
                {
                    errores.AddRange(validationResults.Errors.Select(e => e.ErrorMessage));
                }
            }

            return (result, errores);
        }

        public bool DeshabilitarUsuario(int id)
        {
            bool result = false;

            var usuario = axsis.Usuarios.Find(id);

            if (usuario != null)
            {
                //Deshabilitar
                usuario.Estatus = false;
                axsis.Entry(usuario).State = System.Data.Entity.EntityState.Modified;

                result = axsis.SaveChanges() > 0;
            }

            return result;
        }
    }
}