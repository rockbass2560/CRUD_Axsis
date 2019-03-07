using Servidor.Helpers;
using Servidor.Models;
using Servidor.Models.Requests;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Servidor.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/auth")]
    [EnableCors("*", "*", "*")]
    public class AuthenticationController : ApiController
    {
        private Axsis axsis;

        public AuthenticationController()
        {
            axsis = new Axsis();
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var usuario = axsis.Usuarios.FirstOrDefault(u => u.NombreUsuario == login.NombreUsuario);

            if (usuario != null)
            {
                
                var contraseñaLogin = login.Contrasena;
                var vCode = usuario.VCode;
                var contraseñaGenerada = PasswordHelper.EncodedPassword(contraseñaLogin, vCode);
                
                //Contraseña correcta
                if (usuario.Contrasena == contraseñaGenerada)
                {
                    var tokenResult = TokenGenerator.GenerateTokenJwt(usuario.NombreUsuario);
                    return Ok(new { token = tokenResult });
                }
            }

            return Unauthorized();
        }
    }
}
