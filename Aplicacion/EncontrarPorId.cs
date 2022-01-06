using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Persistencia;

namespace Aplicacion
{
    public class EncontrarPorId
    {
        public class Ejecutar : IRequest<Producto>
        {
            public Guid ID {get; set;}
        }

        public class Manejador : IRequestHandler<Ejecutar, Producto>
        {
            private readonly Context context;

            public Manejador(Context context)
            {
                this.context = context;
            }

            public async Task<Producto> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                // Encontrar Producto
                var producto = await this.context.Producto.FindAsync(request.ID);
                //Excepcion realizada con tu Middleware
                if(producto == null) throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontro el producto"});
                //Retornar Producto
                return producto;
            }
            // public async Task<Unit> Handle(Ejecutar request, CancellationToken cancellationToken)
            // {
            //     //Encontrar Producto
            //     var producto = await this.context.Producto.FindAsync(request.ID);
            //     //Excepcion realizada con tu Middleware
            //     if(producto == null) throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontro el producto"});
            //     //Retornar Producto
            //     return producto;
            // }
        }
    }
}