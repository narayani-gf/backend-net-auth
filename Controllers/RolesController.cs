using backendnet.Data;
using backendnet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendnet.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Administrador")]
public class RolesController(IdentityContext context) : Controller
{
    //Get: api/roles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserlRolDTO>>> GetRoles()
    {
        var roles = new List<UserlRolDTO>();
        
        foreach(var rol in await context.Roles.AsNoTracking().ToListAsync())
        {
            roles.Add(new UserlRolDTO
            {
                Id = rol.Id,
                Nombre = rol.Name!
            });
        }
        return roles;
    }
}