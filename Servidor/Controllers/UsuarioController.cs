using Servidor.Models;
using Servidor.Negocio;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Servidor.Controllers
{
    [Authorize]
    [RoutePrefix("api/usuarios")]
    [EnableCors("*", "*", "*")]
    public class UsuarioController : ApiController
    {
        private UsuarioDO usuarioDO;

        public UsuarioController()
        {
            usuarioDO = new UsuarioDO();
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetUsuario(int id)
        {
            try
            {
                return usuarioDO.Obtener(id);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var usuarios = usuarioDO.ObtenerTodos();

                return Ok(usuarios);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("")]
        public IHttpActionResult RegistrarUsuario(Usuario usuario)
        {
            try
            {
                if (usuario == null)
                    throw new HttpResponseException(HttpStatusCode.BadRequest);

                bool result;
                IEnumerable<string> errores;

                (result, errores) = usuarioDO.Registrar(usuario);

                if (result)
                {
                    return Created($"/usuarios/{usuario.ID}", usuario);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, errores, new JsonMediaTypeFormatter(), new MediaTypeHeaderValue("application/json"));
                }
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult ActualizarUsuario(Usuario usuario)
        {
            try
            {
                if (usuario == null)
                    throw new HttpResponseException(HttpStatusCode.BadRequest);

                bool result;
                IEnumerable<string> errores;

                (result, errores) = usuarioDO.ActualizarUsuario(usuario);

                if (result)
                {
                    return Created($"/usuarios/{usuario.ID}", usuario);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, errores, new JsonMediaTypeFormatter(), new MediaTypeHeaderValue("application/json"));
                }
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut, Route("{id}")]
        public IHttpActionResult DeshabilitarUsuario(int id)
        {
            try
            {
                if (usuarioDO.DeshabilitarUsuario(id))
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                else
                {
                    return BadRequest("Usuario no encontrado");
                }
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
