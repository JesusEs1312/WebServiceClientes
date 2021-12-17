using System;
using System.Net;

namespace Aplicacion.ManejadorError
{
    public class ManejadorExcepcion : Exception
    {
        public HttpStatusCode status {get;}
        public Object errores {get;}
        public ManejadorExcepcion(HttpStatusCode status, Object errores = null)
        {
            this.status  = status;
            this.errores = errores;
        }
    }
}