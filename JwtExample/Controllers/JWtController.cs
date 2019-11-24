using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JWtController : ControllerBase
    {

        private readonly ILogger<JWtController> _logger;
        private readonly JwtConfig _config;

        public JWtController(ILogger<JWtController> logger, IOptions<JwtConfig> config)
        {
            _logger = logger;
            _config = config.Value;
        }

        [HttpGet]
        [Authorize(Roles="Admin")]
        [Route("CheckAuth")]
        public IActionResult CheckAuth()
        {
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetToken")]
        public IActionResult GetToken()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin")
            };
            var key = Convert.FromBase64String(_config.Secret);
            var jwToken = new JwtSecurityToken(
                _config.Issuer,
                _config.Audience,
                expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
                claims: claims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwToken);
            return new OkObjectResult(token);
        }

    }
}