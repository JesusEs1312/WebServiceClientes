using System.Threading;
using System.Threading.Tasks;
using Dominio;
using MediatR;
using Persistencia;

namespace Aplicacion
{
    public class Insertar
    {
        public class Ejecutar : IRequest
        {
            public string nombre {get; set;}
            public string marca {get; set;}
            public string fabricante {get; set;}
            public decimal precio {get; set;}
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
                //Crear propiedad (Porducto)
                var producto = new Producto
                {
                    nombre       = request.nombre,
                    marca        = request.marca,
                    fabricante   = request.fabricante,
                    precio       = request.precio,
                    codigo_barra = request.codigo_barra
                };
                //Insertamos nuevo producto
                this.context.Add(producto);
                //Guardar los cambios en el Servidor de SQL
                var valor = await this.context.SaveChangesAsync();
                if(valor > 0) return Unit.Value;  
                throw new System.Exception("No se puedo agregar el producto");
            }
        }
    }
}