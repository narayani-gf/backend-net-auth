using System.Security.Claims;
using backendnet.Models;
using backendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backendnet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(UserManager<CustomIdentityUser> userManager, JwtTokenService jwtTokenService) : Controller
{
    //POST api/auth
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] LoginDTO loginDTO)
    {
        //Verificar información con identity
        var usuario = await userManager.FindByEmailAsync(loginDTO.Email);
        if(usuario is null || !await userManager.CheckPasswordAsync(usuario, loginDTO.Password))
        {
            //Regresa 401 Acceso no autorizado
            return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos"});
        }
        //Valores indicarán el usuario autenticado en cada petición usando el token
        var roles = await userManager.GetRolesAsync(usuario);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, usuario.Email!),
            new(ClaimTypes.GivenName, usuario.Nombre),
            new(ClaimTypes.Role, roles.First()),
        };
        //Creación token
        var jwt = jwtTokenService.GeneraToken(claims);
        //Regresa token de acceso al usuario con validez de 20 minutos
        return Ok(new 
        {
            usuario.Email,
            usuario.Nombre,
            rol = string.Join(",", roles),
            jwt
        });
    }
    //GET: api/auth/tiempo
    [Authorize]
    [HttpGet("tiempo")]
    public IActionResult GetTiempo()
    {
        string? tiempo = jwtTokenService.TiempoRestanteToken();
        if(tiempo is null)
            return BadRequest();
        return Ok(tiempo);
    }
}

