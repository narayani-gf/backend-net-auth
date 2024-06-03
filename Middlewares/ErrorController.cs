using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace backendnet.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController() : Controller
{
    [Route("/error")]
    public IActionResult HandleErrorDevelpment([FromServices] IHostEnvironment hostEnvironment)
    {
        if(!hostEnvironment.IsDevelopment())
        {
            return BadRequest(new {mensaje = "No se ha podido procesar la petición. Intentélo nuevamente más tarde"});
        }

        var excepcionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        return Problem(
            detail: excepcionHandlerFeature.Error.StackTrace,
            title: excepcionHandlerFeature.Error.Message
        );
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}