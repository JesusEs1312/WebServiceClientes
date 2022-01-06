using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;

namespace Aplicacion
{
    public class Modificar
    {
        public class Ejecutar : IRequest
        {
            public Guid id {get; set;}
            public string nombre {get; set;}
            public string marca {get; set;}
            public string fabricante {get; set;}
            public decimal? precio {get; set;}
            public string codigo_barra {get; set;} 
        }

        public class Manejador : IRequestHandler<Ejecutar>
        {
            private readonly Context context;

            public Manejador(Context context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                //Encontrar producto
                var producto = await this.context.Producto.FindAsync(request.id);
                //Excepcion realizada con tu Middleware
                if(producto == null) throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontro el producto"});
                //Actualizar Datos
                producto.nombre       = request.nombre ?? producto.nombre;
                producto.marca        = request.marca ?? producto.marca;
                producto.fabricante   = request.fabricante ?? producto.fabricante;
                producto.precio       = request.precio ?? producto.precio; 
                producto.codigo_barra = request.codigo_barra ?? producto.codigo_barra;
                //Guardar la los cambios en la base de datos
                var valor = await context.SaveChangesAsync();
                if(valor > 0) return Unit.Value;
                throw new System.Exception("No se guardaron los cambios");
            }
        }
    }
}