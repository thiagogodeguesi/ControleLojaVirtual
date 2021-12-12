using ControleLojaVirtual.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ControleLojaVirtual.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly IConfiguration _configuration;

        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [Route("teste")]
        public IActionResult Teste()
        {
            return Ok("Teste ok");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ValidaToken([FromBody] Usuario user)
        {
            if(user.Nome == "Teste" && user.Senha == "1234")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Nome)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "lojavirtual.com",
                    audience: "lojavirtual.com",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new 
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            return BadRequest("Credenciais invalidas !");
        }
    }
}
