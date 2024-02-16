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
                        return Ok("Autenticación exitosa");
                    }
                    catch (LdapException ldapEx)
                    {
                        return StatusCode(401, ldapEx.Message);
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
