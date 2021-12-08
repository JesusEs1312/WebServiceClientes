using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]//http://localhost:5000/api/producto
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductoController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        //Obtener todos los datos
        [HttpGet]
        public async Task<ActionResult<List<Producto>>> Get()
        {
            //Llamar al mediador para obtener los datos
            return await this.mediator.Send(new Consulta.Ejecuta());
        }

        //Insertar nuevo producto
        [HttpPost]
        public async Task<ActionResult<Unit>> Insertar(Insertar.Ejecutar data)
        {
            return await mediator.Send(data);
        }

        //Modificar producto
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Modificar(int id, Modificar.Ejecutar data)
        {
            data.id = id;
            return await this.mediator.Send(data);
        } 
    }
}