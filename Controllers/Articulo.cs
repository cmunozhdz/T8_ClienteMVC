using CteTarea8MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using System.IO;
using System;
using System.Web;
using CteTarea8MVC.ClienteAzure;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;
using System.Net;




namespace CteTarea8MVC.Controllers
{
    public class ArticuloController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment; //variable que le los parametro de la ruta del server
        private readonly RestApiClient _restApiClient;
        public ArticuloController(IWebHostEnvironment webHost )
        {
            _webHostEnvironment = webHost;
        }                                                        //
        [HttpPost]
        public ActionResult Agregar(Articulo inArticulo)
        {
            //se procesa el guardado y Alta de Achivo
            //inArticulo.SubirArchivo(inArticulo.f)
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }

        }
        // GET: ArticuloController
        public async Task<IActionResult>  Index()
        {
            TempData["BusquedaData"] = "";
            List<Articulo> Lista = new List<Articulo>();
            Articulo _mArt = new();
            
            //String UrlServicio = "https://t8-2020630308-af.azurewebsites.net/api/FnArticulos?code=V0fPd9QsaLaU5epyDXtO209PmPezQmJhZTV_fB8ajMSgAzFutldQjw==&Busqueda="; //+ Descripcion;
            RestApiClient _restApiClient = new RestApiClient("https://t8-2020630308-af.azurewebsites.net/api/");
            string result = await _restApiClient.PostAsync("FnArticulos?code=V0fPd9QsaLaU5epyDXtO209PmPezQmJhZTV_fB8ajMSgAzFutldQjw==&Busqueda=", "{}");
            if (result!=null && !result.StartsWith("[{"))
                {
                    _mArt = new();
                    _mArt.Id = 0;
                    _mArt.Cantidad = 0;
                    _mArt.Descripcion = result;
                    _mArt.Precio = 0;
                    _mArt.FotoUrl = "";
                    Lista.Add(_mArt);
                }
            else
            {
                if (result!=null)
                {
                     Lista = JsonConvert.DeserializeObject<List<Articulo>> (result);
                    //Persona persona = 

                }

            }

            
            ViewData.Model = Lista;

            return View();

        }

        /// <summary>
        /// Consulta los datos del catalogo , 
        /// </summary>
        /// <param name="collection">Busqueda:Indica el nombre del articulo a buscar.</param>
        /// <returns></returns>
        [HttpPost]// GET: ArticuloController
        public async Task<IActionResult> Index(IFormCollection collection)
        {
            List<Articulo> Lista = new List<Articulo>();
            Articulo _mArt = new();
           


            //String UrlServicio = "https://t8-2020630308-af.azurewebsites.net/api/FnArticulos?code=V0fPd9QsaLaU5epyDXtO209PmPezQmJhZTV_fB8ajMSgAzFutldQjw==&Busqueda="; //+ Descripcion;
            String Busqueda = collection["Busqueda"];
            TempData["TextData"] += Busqueda;

            RestApiClient _restApiClient = new RestApiClient("https://t8-2020630308-af.azurewebsites.net/api/");
            string result = await _restApiClient.PostAsync("FnArticulos?code=V0fPd9QsaLaU5epyDXtO209PmPezQmJhZTV_fB8ajMSgAzFutldQjw==&Busqueda=" + Busqueda , "{}");
            //Correcion del json para validar una lista
            if (result != null && !result.StartsWith("["))
            {
                _mArt = new();
                _mArt.Id = 0;
                _mArt.Cantidad = 0;
                _mArt.Descripcion = result;
                _mArt.Precio = 0;
                _mArt.FotoUrl = "";
                Lista.Add(_mArt);
            }
            else
            {
                if (result != null)
                {
                    Lista = JsonConvert.DeserializeObject<List<Articulo>>(result);
                    //Persona persona = 

                }
            }

            ViewData.Model = Lista;

            return View();

        }


        // GET: ArticuloController/Details/5
        public ActionResult Details(int id)
        {
            
            return View();
        }

        // GET: ArticuloController/Create
        public async Task<IActionResult> Create(int id)
        {
            //Valida si el articulo existe.
            if (id!=0)
            {
                //El producto es nuevo
                //https://t8-2020630308-af.azurewebsites.net/api/FnGetArticulo?code=lrfCjESazk1KEbdD5rzAcPOto5IGcuGPQcuaasEQ8TmLAzFuYJoefg==
                RestApiClient _restApiClient = new RestApiClient("https://t8-2020630308-af.azurewebsites.net/api/");
                String EndPoint = "FnGetArticulo?code=lrfCjESazk1KEbdD5rzAcPOto5IGcuGPQcuaasEQ8TmLAzFuYJoefg==&Id=" + id;
                String result = await _restApiClient.PostAsync(EndPoint, "{}");

                if (result != null && result.StartsWith("Error"))
                {
                    return BadRequest();
                }
                else
                {
                    Articulo _Art = JsonConvert.DeserializeObject<Articulo>(result);
                    ViewData.Model = _Art;
                    
                    return View();
                }
                
            }
            else
            {
                return View();
            }

            
        }

        // POST: ArticuloController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>  Create(IFormCollection collection)
        {
            double dCantidad=0;
            double dPrecio=0;
            String _Descripcion="";
            int Id = 0;
            bool Esvalido;

            try
            {
                var _Tmp= collection["_File"];
                //IFormFile _File;// = _Tmp;
                
                
                _Descripcion = collection["Descripcion"];
                String _Cantidad = collection["Cantidad"];
                String _Precio = collection["Precio"];
                String  _Id = collection["Id"];
                String File64Bits;
                Double.TryParse(_Cantidad,out dCantidad);
                Double.TryParse(_Precio, out dPrecio);
                int.TryParse(_Id, out Id);
                String EndPoint;
                String Modo = "INS";
                Esvalido = false;
                TempData["TempData"] = "Proporcione un archivo.";
                foreach (var file in collection.Files)
                {
                    if (file.Length ==0)
                    {
                        TempData["TempData"] = "El archivo está vacio.";
                        Esvalido = false;
                    }
                    else
                    {
                        //Se  Carga el archivo.
                        try
                        {
                            TempData["TempData"] = "";
                            var _Path = Path.Combine(_webHostEnvironment.WebRootPath, "Files");  //define la ruta donde se subira el archivo.
                                                                                                     //Validamos la carpeta y si no existe la crea.
                            if (!Directory.Exists(_Path))
                            {
                                Directory.CreateDirectory(_Path); //Creamos directorio. 
                            }
                            String _FullPath = Path.Combine(_Path, file.FileName);
                            String _UrlPath = "/Files/" + file.FileName;
                            
                            

                            
                            
                            
                            using (var Stream = new FileStream(_FullPath, FileMode.Create))
                            {
                                //Creamos el archivo en el servidor.
                                file.CopyTo(Stream);
                                Esvalido = true;
                                TempData["TempData"] = _FullPath;

                                //Byte[] bytes = File.ReadAllBytes(Stream);
                                // File64Bits = Convert.ToBase64String(bytes);
                                //file.OpenReadStream()
                            }
                            RestApiClient _restApiClient = new RestApiClient("https://t8-2020630308-af.azurewebsites.net/api/");
                            Modo = Id == 0 ? "INS" : "upd"; //MODO INSER CUANDO EL ID ES 0 POR EL CONTRARIO EN UPD

                            EndPoint = "FnAltaCatalogoArticulo?code=t-V5epsKwh3EkMIOC1uCIWTgq2TffPLwD7FRO8aMEjhmAzFuWaAreQ==&Modo=" + Modo + "&Id=" +Id +"&Descripcion="
                                                    + _Descripcion + "&Cantidad=" + dCantidad + "&Precio=" + dPrecio + "&Foto=" + _UrlPath;

                            
                            


                            String result = await _restApiClient.PostAsync(EndPoint, "{}");
                            
                            if (result != null && result.StartsWith("OK:") )
                            {
                                TempData["TempData"] = "";
                                Esvalido = true ;
                            }
                            else
                            {
                                Esvalido = false ;
                                TempData["TempData"] = result;
                            }



                        }
                        catch (Exception e)
                        {
                            Esvalido = false;
                        }

                    }

                }
                   

                 


               // return RedirectToAction(nameof(Index));
            }
            catch (Exception e2)
            {
                Esvalido = false; 
            }

            if (Esvalido)
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index));


            else
            {
                Articulo _Articulo=new Articulo();
                _Articulo.Id = Id;


                _Articulo.Descripcion = _Descripcion;
                _Articulo.Cantidad = dCantidad;
                _Articulo.Precio = dPrecio;

                ViewData.Model = _Articulo;
                return View();
            }
                


                
                

        }

        // GET: ArticuloController/Edit/5

        // POST: ArticuloController/Edit/5


        // GET: ArticuloController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            
            RestApiClient _restApiClient = new RestApiClient("https://t8-2020630308-af.azurewebsites.net/api/");
            String EndPoint = "FnGetArticulo?code=lrfCjESazk1KEbdD5rzAcPOto5IGcuGPQcuaasEQ8TmLAzFuYJoefg==&Id=" + id;
            String result = await _restApiClient.PostAsync(EndPoint, "{}");

            if (result != null && result.StartsWith("Error"))
            {
                return BadRequest();
            }
            else
            {
                Articulo _Art = JsonConvert.DeserializeObject<Articulo>(result);
                ViewData.Model = _Art;

                return View();
            }

            
        }

        // POST: ArticuloController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            Boolean EsError = false;
            
            try
            {
                if (id!=0)
                {
                    
                    String Modo = "dlt"; //MODO de borrado.
                    RestApiClient _restApiClient = new RestApiClient("https://t8-2020630308-af.azurewebsites.net/api/");
                    String EndPoint = "FnAltaCatalogoArticulo?code=t-V5epsKwh3EkMIOC1uCIWTgq2TffPLwD7FRO8aMEjhmAzFuWaAreQ==&Modo=" + Modo + "&Id=" + id;
                    String result = await _restApiClient.PostAsync(EndPoint, "{}");

                    if (result != null && result.StartsWith("OK:"))
                    {
                        TempData["TempData"] = "";
                        EsError = false;
                    }
                    else
                    {
                        EsError = true;
                        TempData["TempData"] = result;
                    }

                } else
                {
                    EsError = true;
                    TempData["TempData"] = "El id del articulo es obligatorio.";
                }



            }
            catch (Exception e)
            {
                TempData["ErrData"] = e.ToString();
                EsError = true;
            }
            if (EsError == false  ) {
                return RedirectToAction(nameof(Index));
            } else {
                
                return View();
            }
                
        }


    }
}
