using Microsoft.AspNetCore.Http;
using Newtonsoft;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
namespace CteTarea8MVC.ClienteAzure
{

    

    public class ArticuloAZ
    {
        public String RespuestaSVC { get; set; }

        public String SendAlta(String Descripcion,double Cantidad , double Precio , String Foto )
        {
            /*
             * Ejecuta el servicio publicado en AWS.
             * Modo=Ins&Id=999&Descripcion=%22Calcetines%22&Cantidad=100&Precio=8765.23&Foto=%22XXX%22
             *https://t8-2020630308-af.azurewebsites.net/api/FnAltaCatalogoArticulo?code=t-V5epsKwh3EkMIOC1uCIWTgq2TffPLwD7FRO8aMEjhmAzFuWaAreQ==&Modo=Ins&Id=999&Descripcion=%22Calcetines%22&Cantidad=100&Precio=8765.23&Foto=%22XXX%22
             */
            String UrlServicio= "https://t8-2020630308-af.azurewebsites.net/api/FnAltaCatalogoArticulo?code=t-V5epsKwh3EkMIOC1uCIWTgq2TffPLwD7FRO8aMEjhmAzFuWaAreQ==" 
                                +"&Modo=Ins&Id=0&Descripcion=" + Descripcion  +"&Cantidad=" +Cantidad +"Precio= " + Precio + "&Foto=" +Foto;
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, UrlServicio);
            var response =client.Send(request);
            response.EnsureSuccessStatusCode();
            String respuesta;
            if (response.StatusCode.Equals("200"))
                respuesta = response.ReasonPhrase;
            else
                respuesta = response.StatusCode.ToString() ;


            return respuesta;
            
        }


        public async Task<String>  Consulta(String Descripcion)
        {
            /*
            * Ejecuta el servicio publicado en AWS.
            * Modo=Ins&Id=999&Descripcion=%22Calcetines%22&Cantidad=100&Precio=8765.23&Foto=%22XXX%22
            *https://t8-2020630308-af.azurewebsites.net/api/FnArticulos?code=V0fPd9QsaLaU5epyDXtO209PmPezQmJhZTV_fB8ajMSgAzFutldQjw==&Busqueda=cuadro
            */
            String UrlServicio = "https://t8-2020630308-af.azurewebsites.net/api/FnArticulos?code=V0fPd9QsaLaU5epyDXtO209PmPezQmJhZTV_fB8ajMSgAzFutldQjw==&Busqueda=" + Descripcion;
            var client = new HttpClient();

            Task<string> getStringTask =
                client.GetStringAsync(UrlServicio);

            DoIndependentWork();

            String MyRespuestaSVC = await getStringTask;
            
            return MyRespuestaSVC;

        }
        private void DoIndependentWork()
        {
            RespuestaSVC = "Procesando ..";
        }
    }

    
}
