using System;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion
{
    public class Insertar
    {
        public class Ejecutar : IRequest
        {
            public Guid? productoId {get; set;}
            public string nombre {get; set;}
            public string marca {get; set;}
            public string fabricante {get; set;}
            public decimal precio {get; set;}
            public string codigo_barra {get; set;}
        }

        public class Validaciones : AbstractValidator<Ejecutar>
        {
            public Validaciones()
            {
                RuleFor(n => n.nombre).NotEmpty();
                RuleFor(n => n.marca).NotEmpty();
                RuleFor(n => n.fabricante).NotEmpty();
                RuleFor(n => n.precio).NotEmpty();
                RuleFor(n => n.codigo_barra).NotEmpty();
            }
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
                //Creamos un identificador Global Unico
                Guid id = Guid.NewGuid();
                if(request.productoId != null){
                    id = request.productoId ?? Guid.NewGuid();
                }   
                //Crear propiedad (Porducto)
                var producto = new Producto
                {
                    id           = id,
                    nombre       = request.nombre,
                    marca        = request.marca,
                    fabricante   = request.fabricante,
                    precio       = request.precio,
                    codigo_barra = request.codigo_barra
                };

                if(request.nombre == "" || request.marca == "" || request.fabricante == "" || request.codigo_barra == ""){
                    throw new System.Exception("No se puedo agregar el producto");
                }
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