using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Servidor.Controllers
{
    public class TokenValidationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            string token;
            bool tokenExists;
            Task<HttpResponseMessage> resultMethod=null;

            (tokenExists, token) = request.TryRetrieveToken();

            try
            {
                if (!tokenExists) //No hay token
                {
                    statusCode = HttpStatusCode.Unauthorized;
                    resultMethod = base.SendAsync(request, cancellationToken);
                }
                else //Token encontrado
                {
                    //Llave que debería estar escondida en producción
                    var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
                    var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
                    var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];

                    var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
                    var tokenHandler = new JwtSecurityTokenHandler();
                    TokenValidationParameters validationParameters = new TokenValidationParameters()
                    {
                        ValidAudience = audienceToken,
                        ValidIssuer = issuerToken,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = securityKey,
                        LifetimeValidator = (notBefore, expires, securityTok, validationParams) => expires != null && DateTime.UtcNow < expires
                    };

                    SecurityToken securityToken;
                    Thread.CurrentPrincipal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                    HttpContext.Current.User = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                    resultMethod = base.SendAsync(request, cancellationToken);
                }
            }
            catch(SecurityTokenValidationException)
            {
                statusCode = HttpStatusCode.Unauthorized;
                resultMethod = Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
            }
            catch(Exception)
            {
                statusCode = HttpStatusCode.InternalServerError;
                resultMethod = Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
            }

            return resultMethod;
        }
    }
}