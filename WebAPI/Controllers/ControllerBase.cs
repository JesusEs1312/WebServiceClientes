using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Controllers
{
    
    [Route("api/[controller]")]//http://localhost:5000/api/producto
    [ApiController]
    public class MiControllerBase : ControllerBase
    {
           private IMediator _mediator;
           protected IMediator mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>()); 
    }
}