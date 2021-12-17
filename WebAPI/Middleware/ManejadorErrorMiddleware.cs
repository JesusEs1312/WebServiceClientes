using System;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebAPI.Middleware
{
    public class ManejadorErrorMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ManejadorErrorMiddleware> logger;

        public ManejadorErrorMiddleware(RequestDelegate next, ILogger<ManejadorErrorMiddleware> logger)
        {
            this.next   = next;
            this.logger = logger;
        }

        public async Task Invocar(HttpContext context)
        {
            try
            {
                //Pasar el contexto de la webApi(Controller) a la Aplicacion
                await this.next(context);   
            }
            catch (System.Exception ex)
            {
                 await ManejadorExcepcionAsincrono(context, ex, logger);
            }
        }

        private async Task ManejadorExcepcionAsincrono(HttpContext context, Exception ex, ILogger<ManejadorErrorMiddleware> loggger)
        {
            object errores = null;
            switch(ex)
            {
                case ManejadorExcepcion me:
                    loggger.LogError(ex, "Manejador Error");
                    errores = me.errores;
                    context.Response.StatusCode = (int)me.status;
                    break;
                case Exception e:
                    loggger.LogError(ex, "Error del servidor");
                    errores = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    break;
            }
            context.Response.ContentType = "application/json";
            if(errores != null)
            {
                //Serializar Errores
                var resultado = JsonConvert.SerializeObject(new {errores});
                await context.Response.WriteAsync(resultado);
            }
        }
    }
}