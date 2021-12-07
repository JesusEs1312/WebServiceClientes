using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion
{
    public class Consulta
    {
        public class Ejecuta : IRequest<List<Producto>>
        {
            //Sin parametros
        }

        public class Manejador : IRequestHandler<Ejecuta, List<Producto>>
        {
            private readonly Context context;
            public Manejador(Context context)
            {
                this.context = context;
            }
            public async Task<List<Producto>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var productos = await context.Producto.ToListAsync();//Devolver los datos en una lista
                return productos;
            }
        }

    }
}