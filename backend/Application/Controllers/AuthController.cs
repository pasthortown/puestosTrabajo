using Microsoft.Extensions.Options;
using System.DirectoryServices.Protocols;
using Backend.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Backend.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Realiza la autenticación por LDAP.
        /// </summary>
        /// <param name="loginRequest">Credenciales de Autenticación.</param>
        /// <returns>Realiza la autenticación por LDAP.</returns>
        /// 
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var serverIP = _configuration["LDAPSettings:ServerIP"];
            var serverPortString = _configuration["LDAPSettings:ServerPort"];

            if (int.TryParse(serverPortString, out var serverPort))
            {
                using (var ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(serverIP, serverPort)))
                {
                    try
                    {
                        ldapConnection.Bind(new System.Net.NetworkCredential(loginRequest.Username, loginRequest.Password));
                        var respuesta = new { Mensaje = "Autenticación exitosa" };
                        return Ok(new { respuesta });
                    }
                    catch (LdapException ldapEx)
                    {
                        var respuesta = new { Mensaje = ldapEx.Message };
                        return StatusCode(401, respuesta);
                    }
                }
            }
            else
            {
                return StatusCode(500, "Error en la configuración de LDAPSettings:ServerPort");
            }
        }
    }

}
