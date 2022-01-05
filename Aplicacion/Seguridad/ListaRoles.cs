using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class ListaRoles
    {
        public class Ejecuta : IRequest<List<IdentityRole>>
        {
            
        }

        public class Manejador : IRequestHandler<Ejecuta, List<IdentityRole>>
        {
            private readonly Context context;
            public Manejador(Context context)
            {
                this.context = context;
            }
            public async Task<List<IdentityRole>> Handle(Ejecuta request, CancellationToken cancellationToken) => await this.context.Roles.ToListAsync();
        }
    }
}