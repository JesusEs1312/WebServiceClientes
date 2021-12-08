using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia;

namespace Aplicacion
{
    public class Eliminar
    {
        public class Ejecutar : IRequest
        {
            public int id {get; set;}
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
                if(producto == null) throw new System.Exception("No existe el producto en la base de datos");
                //Eliminar producto
                this.context.Remove(producto);
                //Guardar los cambios en la base de datos
                var valor = await this.context.SaveChangesAsync();
                if (valor > 0) return Unit.Value;
                throw new System.Exception("No se pudo eliminar el producto");
            }
        }
    }
}