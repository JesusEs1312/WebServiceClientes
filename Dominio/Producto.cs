using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    public class Producto
    {
        public Guid id {get; set;}
        public string nombre {get; set;}
        public string marca {get; set;}
        public string fabricante {get; set;}
        
        [Column(TypeName= "decimal(18,4)")]
        public decimal precio {get; set;}
        public string codigo_barra {get; set;} 
    }
}