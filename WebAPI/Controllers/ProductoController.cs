using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]//http://localhost:5000/api/producto
    [ApiController]
    public class ProductoController : MiControllerBase
    {
        //Obtener todos los productos
        [HttpGet]
        public async Task<ActionResult<List<Producto>>> Get() => await this.mediator.Send(new Consulta.Ejecuta());

        //Insertar nuevo producto
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<ActionResult<Unit>> Insertar(Insertar.Ejecutar data) => await mediator.Send(data);

        //Modificar producto
        [Authorize(Roles ="Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Modificar(Guid id, Modificar.Ejecutar data)
        {
            data.id = id;
            return await this.mediator.Send(data);
        }

        //Eliminar producto
        [Authorize(Roles ="Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id) => await mediator.Send(new Eliminar.Ejecutar{id = id});

        //Encontrar por Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> EncontrarPorId(Guid id) => await mediator.Send(new EncontrarPorId.Ejecutar{ID = id});
    }
}