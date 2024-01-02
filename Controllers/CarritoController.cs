using CteTarea8MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CteTarea8MVC.ClienteAzure;
using Newtonsoft.Json;


namespace CteTarea8MVC.Controllers
{
    public class CarritoController : Controller
    {
        // GET: CarritoController
        public async Task<IActionResult> Index()
        {
            var IdCarrito = HttpContext.Session.GetString("IdCarrito");
            String Busqueda = IdCarrito != null ? IdCarrito:""; // Asigna la sesion al valor a buscar cuando est{a inicializado
            

            List<ItemCarrito> Lista = new List<ItemCarrito>();
            ItemCarrito _item;
            //Extrae el valor del carrito Actual. invconcando el servicio
            //https://t8-2020630308-af.azurewebsites.net/api/FnVerCarrito?code=cIp--2ZpLMQjYIH6AdrSoBxs6QMAV2I4HfjzixbBjtLeAzFuc7s97A==&IdCarrito= 1
            
            if (!Busqueda.Equals("")) {

            

                RestApiClient _restApiClient = new RestApiClient("https://t8-2020630308-af.azurewebsites.net/api");
                string result = await _restApiClient.PostAsync("FnVerCarrito?code=cIp--2ZpLMQjYIH6AdrSoBxs6QMAV2I4HfjzixbBjtLeAzFuc7s97A==&IdCarrito= " + Busqueda, "{}");
                if (result != null && !result.StartsWith("["))
                {
                    _item = new ItemCarrito();
                    _item.Descripcion = result;
                    Lista.Add(_item );
                }
                else
                {
                    if (result != null)
                    {
                    
                        Lista = JsonConvert.DeserializeObject<List<ItemCarrito>>(result);
                        //Persona persona = 

                    }
                }
                if (Lista.Count ==0 )
                {
                    _item = new ItemCarrito();
                    _item.Descripcion = "El Carrito está vacio";
                    Lista.Add(_item);
                }
            }
            else
            {
                _item = new ItemCarrito();
                _item.Descripcion = "El Carrito no ha sido inicializado, por favor crear uno.";
                Lista.Add(_item);
            }
            ViewData.Model = Lista;
            return View();
        }

        // GET: CarritoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CarritoController/Create
        public ActionResult Create()
        {
            
            return View();
        }
        public async Task<IActionResult> BorrarCarrito()
        {
            String idCarrito = HttpContext.Session.GetString("IdCarrito");
            if (String.IsNullOrEmpty(idCarrito)) {
                TempData["ErrCarritoBorrar"] = "Seleccione un carrito a eliminar";
            }
            
                
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BorrarCarrito(IFormCollection collection) 
        {
            
            String idCarrito = HttpContext.Session.GetString("IdCarrito");
            if (String.IsNullOrEmpty(idCarrito)) {
                TempData["ErrCarritoBorrar"] = "Seleccione un carrito a eliminar";
                return View();
            }
            else
            {
                //Preparamos la invocacion del sericio. 
                //https://t8-2020630308-af.azurewebsites.net/api/FnDeleteCarrito?code=cFGC5aH2tOIkguYZIe3s61z1xo0L3-uAl0W8cb0p6fZ9AzFusMFrfg==
                TempData["ErrCarritoBorrar"] = "";
                RestApiClient _restApiClient = new RestApiClient("https://t8-2020630308-af.azurewebsites.net/api");
                string result = await _restApiClient.PostAsync("FnDeleteCarrito?code=cFGC5aH2tOIkguYZIe3s61z1xo0L3-uAl0W8cb0p6fZ9AzFusMFrfg==&IdCarrito=" + idCarrito, "{}");
                if (result.StartsWith("Error")) {
                    //muestra la falla en el proceso.
                    TempData["ErrCarritoBorrar"] = result;
                    return View();
                } 
                else
                {
                    HttpContext.Session.SetString("IdCarrito","");
                    return RedirectToAction(nameof(Index));
                }
            }
            
        }
        public async Task<IActionResult> CarritoNuevo()
        {
            //Invoca el servicio que crea el carrito y lo almacena en un sesion nueva.
            //https://t8-2020630308-af.azurewebsites.net/api/FnAltaCarrito?code=bcFhcWAfoyLcP4DWlkng5d3Nr7xtqQ7lezN01voKAKLmAzFu-uZyEQ==
            RestApiClient _restApiClient = new RestApiClient("https://t8-2020630308-af.azurewebsites.net/api");
            String IdCarrito = "3";
            string result = await _restApiClient.PostAsync("FnAltaCarrito?code=bcFhcWAfoyLcP4DWlkng5d3Nr7xtqQ7lezN01voKAKLmAzFu-uZyEQ==&IdCarrito="+ IdCarrito , "{}");
            if (result!=null && result.Contains("Registro Actualizados")) {
                //se Agrego el carrito y se actualiza la sesion.

                HttpContext.Session.SetString("IdCarrito", IdCarrito);
            }
            else
            {
                if (result != null && result.Contains("Duplicate entry"))
                {
                    //El carrito ya existe por lo tanto aplica la sesion.

                    HttpContext.Session.SetString("IdCarrito", IdCarrito);
                } 
                else
                {
                    IdCarrito = "";
                }
            }
            
            if (IdCarrito.Equals(""))
            {
                return BadRequest();
            }
            else
            {
                
                return RedirectToAction(nameof(Index));
            }
            
        }
        // POST: CarritoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            try
            {

                //int idCarrito = collection[];
                String  _idArticulo = collection["IdArticulo"];
                String  _Cantidad = collection["Cantidad"] ;
                String  _Precio = collection["Precio"];
                String idCarrito = HttpContext.Session.GetString("IdCarrito");
                int idArticulo;
                double Cantidad;
                double Precio;

                int.TryParse(_idArticulo,out idArticulo);
                double.TryParse(_Precio,out Precio);
                double.TryParse(_Cantidad, out Cantidad);
                String EndPoint;
                     //.ge.SetString("IdCarrito"  IdCarrito);
                RestApiClient _restApiClient = new RestApiClient("https://t8-2020630308-af.azurewebsites.net/api");
                //String EndPoint = "FnAgregarItemCarrito?code=4uw9OHi5S2j1EroWEg83orMXs3w5XGCwz-PtXY0Klhw1AzFumUv6CA==&IdCarrito=" + idCarrito + "&IdArticulo=" + idArticulo + "&Cantidad=" + Cantidad + "&Precio=" + Precio;
                if ( String.Equals(TempData["Action"].ToString() ,"INS"))
                {
                    EndPoint = "FnAgregarItemCarrito?code=4uw9OHi5S2j1EroWEg83orMXs3w5XGCwz-PtXY0Klhw1AzFumUv6CA==&IdCarrito=" + idCarrito + "&IdArticulo=" + idArticulo + "&Cantidad=" + Cantidad + "&Precio=" + Precio;
                }
                else
                {
                    EndPoint = "FnUpdateItemCarrito?code=DCMgFpNnOuWrJzoFUap-WPP78iFRwtXjK6hVNGT7ij8WAzFunsP3vw==&IdCarrito=" + idCarrito + "&IdArticulo=" + idArticulo + "&Cantidad=" + Cantidad;
                }
                string result = await _restApiClient.PostAsync(EndPoint, "{}");
                
                if ( !result.StartsWith("Error") )
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["TextData"] = result ;
                    ItemCarrito _ItemCarrito = new ItemCarrito();
                    _ItemCarrito.IdArticulo = idArticulo;
                    _ItemCarrito.Precio = Precio;
                    _ItemCarrito.Cantidad = Cantidad;
                    _ItemCarrito.Descripcion = collection["Descripcion"];
                    _ItemCarrito.SFoto = collection["SFoto"];
                    ViewData.Model = _ItemCarrito;
                    return View();
                }
                
            }
            catch
            {
                return View();
            }
        }

        // GET: CarritoController/Edit/5

        public async Task<IActionResult>  Edit(int id)
        {
            //Consulta los datosl del arcticulo
            bool esError = false;
            String idCarrito = HttpContext.Session.GetString("IdCarrito");
            if (!String.IsNullOrEmpty(idCarrito))  {
                ItemCarrito _ItemCarrito = new ItemCarrito();
                int _IdCarrito = 0; 
                int.TryParse(idCarrito,out _IdCarrito);
                _ItemCarrito._IdCarrito = _IdCarrito;
                if (id == 0) {
                    String _IdArticulo =HttpContext.Request.Query["IdArticulo"].ToString();
                    int.TryParse(_IdArticulo,out id);        
                }
                TempData["TextData"] = "";
                //Buscamos el articulo en el carrito.
                //https://t8-2020630308-af.azurewebsites.net/api/FnGetItemCarrito?code=zcKnj5y--hLw-PnMlHimlNTcXeRECKNfInLW2h_smGJoAzFuYnpCZg==
                RestApiClient _restApiClient = new RestApiClient("https://t8-2020630308-af.azurewebsites.net/api");
                String EndPoint = "FnGetItemCarrito?code=zcKnj5y--hLw-PnMlHimlNTcXeRECKNfInLW2h_smGJoAzFuYnpCZg==&IdArticulo=" + id + "&IdCarrito=" + _IdCarrito;
                String result = await _restApiClient.PostAsync(EndPoint, "{}");
                if (result != null && result.StartsWith("Error"))
                {
                    TempData["TextData"] = result;
                    esError = true;
                }
                else
                {
                    _ItemCarrito= JsonConvert.DeserializeObject<ItemCarrito>(result);
                    if (_ItemCarrito.IdArticulo== -1 )
                    {
                        /*El prodocuto no está en el carrito por lo tanto se trae del catalogo de articulos.
                         * https://t8-2020630308-af.azurewebsites.net/api/FnGetArticulo?code=lrfCjESazk1KEbdD5rzAcPOto5IGcuGPQcuaasEQ8TmLAzFuYJoefg==&Id=10
                         * 
                         */
                        _restApiClient = new RestApiClient("https://t8-2020630308-af.azurewebsites.net/api");
                        EndPoint = "FnGetArticulo?code=lrfCjESazk1KEbdD5rzAcPOto5IGcuGPQcuaasEQ8TmLAzFuYJoefg==&Id=" + id;
                        result = await _restApiClient.PostAsync(EndPoint, "{}");
                        if (result != null && result.StartsWith("Error"))
                        {
                            TempData["TextData"] = result;
                            esError = true;
                        } else
                        {
                            Articulo _ArticuloCatalogo;
                            _ArticuloCatalogo = JsonConvert.DeserializeObject<Articulo>(result);
                            _ItemCarrito.IdArticulo = _ArticuloCatalogo.Id;
                            _ItemCarrito.Descripcion = _ArticuloCatalogo.Descripcion;
                            _ItemCarrito.SFoto = _ArticuloCatalogo.FotoUrl;
                            _ItemCarrito.Precio = _ArticuloCatalogo.Precio;
                            _ItemCarrito.Cantidad = 1;
                            TempData["Img"] = _ItemCarrito.SFoto;
                            TempData["Action"] = "INS"; //El registro no existe por lo tanto se debe agregar.
                        }
                    } else
                    {
                        TempData["Img"] = _ItemCarrito.SFoto;
                        TempData["Action"] = "UPD"; //El registro ya existe por lo tanto se debe actualizar
                    }
                    
                }
                //Buscamos el articlo en el servicio 
                // https://t8-2020630308-af.azurewebsites.net/api/FnGetArticulo?code=lrfCjESazk1KEbdD5rzAcPOto5IGcuGPQcuaasEQ8TmLAzFuYJoefg==&Id=10
                //Configuramos el endpoint
                
                //String EndPoint = "FnAgregarItemCarrito?code=4uw9OHi5S2j1EroWEg83orMXs3w5XGCwz-PtXY0Klhw1AzFumUv6CA==&IdCarrito=" + idCarrito + "&IdArticulo=" + idArticulo + "&Cantidad=" + Cantidad + "&Precio=" + Precio;

                if(_ItemCarrito.IdArticulo > 0)
                {
                    ViewData.Model = _ItemCarrito;
                }
                else
                {
                    TempData["TextData"] = "Error: El articulo " + id + "no se encontro";
                }

                
                
            }
            else
            {
                TempData["TextData"] = "Error: Seleccione un carrito  valido";

            }

            
            return View();

            
        }

        // POST: CarritoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Edit(int id, IFormCollection collection)
        {
            bool EsError = false ; 
            if (!ModelState.IsValid)
            {
                TempData["TextData"] = "Model State es invalido";
                EsError = true ;
            }

            try
            {

                //int idCarrito = collection[];
                //String _idArticulo = id;
                ItemCarrito _ItemCarrito;
                String _Cantidad = collection["Cantidad"];
                String _Precio = collection["Precio"];
                String idCarrito = HttpContext.Session.GetString("IdCarrito");
                int idArticulo;
                double Cantidad;
                double Precio;
                if (id != 0)
                {
                    idArticulo = id;
                }
                else
                {
                    int.TryParse(collection["IdArticulo"], out idArticulo);
                }

                //int.TryParse(_idArticulo, out idArticulo);
                double.TryParse(_Precio, out Precio);
                double.TryParse(_Cantidad, out Cantidad);
                //.ge.SetString("IdCarrito"  IdCarrito);
                //Vaida si el articulo ya está en el carrito.
                //consultando servicio
                //https://t8-2020630308-af.azurewebsites.net/api/FnGetItemCarrito?code=zcKnj5y--hLw-PnMlHimlNTcXeRECKNfInLW2h_smGJoAzFuYnpCZg==
                
                //String EndPoint = "FnAgregarItemCarrito?code=4uw9OHi5S2j1EroWEg83orMXs3w5XGCwz-PtXY0Klhw1AzFumUv6CA==&IdCarrito=" + idCarrito + "&IdArticulo=" + idArticulo + "&Cantidad=" + Cantidad + "&Precio=" + Precio;
                if (Cantidad > 0) {
                    RestApiClient _restApiClient = new RestApiClient("https://t8-2020630308-af.azurewebsites.net/api");
                    String EndPoint;
                    if (String.Equals(TempData["Action"].ToString() ,"INS")) {
                        EndPoint = "FnAgregarItemCarrito?code=4uw9OHi5S2j1EroWEg83orMXs3w5XGCwz-PtXY0Klhw1AzFumUv6CA==&IdCarrito=" + idCarrito + "&IdArticulo=" + idArticulo + "&Cantidad=" + Cantidad + "&Precio=" + Precio;
                    } //El registro no existe por lo tanto se debe agregar.
                    else
                    {
                        EndPoint = "FnUpdateItemCarrito?code=DCMgFpNnOuWrJzoFUap-WPP78iFRwtXjK6hVNGT7ij8WAzFunsP3vw==&IdCarrito=" + idCarrito + "&IdArticulo=" + idArticulo + "&Cantidad=" + Cantidad;
                    
                    }

                    string result = await _restApiClient.PostAsync(EndPoint, "{}");

                    if (!result.StartsWith("Error"))
                    {
                        TempData["TextData"] = "";
                        EsError = false ;
                    }
                    else
                    {
                        TempData["TextData"] = result;
                        _ItemCarrito = new ItemCarrito();
                        _ItemCarrito.IdArticulo = idArticulo;
                        _ItemCarrito.Precio = Precio;
                        _ItemCarrito.Cantidad = Cantidad;
                        _ItemCarrito.Descripcion= collection["Descripcion"];
                        ViewData.Model = _ItemCarrito;
                        EsError = true;
                    }
                }
                else
                {
                    TempData["TextData"] = "La Cantidad debe ser mayor que cero.";
                    _ItemCarrito = new ItemCarrito();
                    _ItemCarrito.IdArticulo = idArticulo;
                    _ItemCarrito.Precio = Precio;
                    _ItemCarrito.Cantidad = Cantidad;
                    _ItemCarrito.Descripcion = collection["Descripcion"];
                    ViewData.Model = _ItemCarrito;
                    EsError = true;
                }

            }
            catch (Exception e)
            {
                TempData["TextData"] = e.ToString();
                EsError = true;
            }
            if (EsError) {
                return View(); //Se encontro error en el proceso y regresa a la pantalla. 
            }
             else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: CarritoController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            //Trae los datos. 

            String idCarrito = HttpContext.Session.GetString("IdCarrito");
            if (!String.IsNullOrEmpty(idCarrito))
            {
                ItemCarrito _ItemCarrito = new ItemCarrito();
                int _IdCarrito = 0;
                int.TryParse(idCarrito, out _IdCarrito);
                _ItemCarrito._IdCarrito = _IdCarrito;
                if (id == 0)
                {
                    String _IdArticulo = HttpContext.Request.Query["IdArticulo"].ToString();
                    int.TryParse(_IdArticulo, out id);
                }
                //Buscamos el articlo en el servicio 
                // https://t8-2020630308-af.azurewebsites.net/api/FnGetItemCarrito?code=zcKnj5y--hLw-PnMlHimlNTcXeRECKNfInLW2h_smGJoAzFuYnpCZg==
                //Configuramos el endpoint
                RestApiClient _restApiClient = new RestApiClient("https://t8-2020630308-af.azurewebsites.net/api");
                //String EndPoint = "FnAgregarItemCarrito?code=4uw9OHi5S2j1EroWEg83orMXs3w5XGCwz-PtXY0Klhw1AzFumUv6CA==&IdCarrito=" + idCarrito + "&IdArticulo=" + idArticulo + "&Cantidad=" + Cantidad + "&Precio=" + Precio;

                String EndPoint = "FnGetItemCarrito?code=zcKnj5y--hLw-PnMlHimlNTcXeRECKNfInLW2h_smGJoAzFuYnpCZg==&IdCarrito=" + _IdCarrito + "&IdArticulo=" + id;
                
                string result = await _restApiClient.PostAsync(EndPoint, "{}");
                TempData["TextData"] = "";
                if (result != null && result.StartsWith("Error"))
                {
                    TempData["TextData"] = result;
                }
                if (result != null && result.StartsWith("{"))
                {
                    //Deserializamos el objeto encontrado
                    _ItemCarrito = JsonConvert.DeserializeObject<ItemCarrito>(result);
                   
                    TempData["Img"] = _ItemCarrito.SFoto;
                }

                if (_ItemCarrito.IdArticulo > 0)
                {
                    ViewData.Model = _ItemCarrito;
                }
                else
                {
                    TempData["TextData"] = "Error: El articulo " + id + "no se encontro";
                }



            }
            else
            {
                TempData["TextData"] = "Error: Seleccione un carrito  valido";

            }
            return View();

            } //fin Delete

            // POST: CarritoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            bool ExisteError = false; 
            String idCarrito = HttpContext.Session.GetString("IdCarrito");
            TempData["TextData"] = "";
            if (String.IsNullOrEmpty(idCarrito)) {
             
                TempData["TextData"] = "El Id del Carrito no se ha establecido, genere uno por favor.";
                ExisteError = true;

            }
            else
            {
                if (id==0)
                {
                    TempData["TextData"] = "El Id del Articlo no es valido.";
                    ExisteError = true;

                } 
                else
                {
                    //Ejecuta el servicio https://t8-2020630308-af.azurewebsites.net/api/FnDeleteItemCarrito?code=KbzdT55Bkv-DasJO7vZsdNMr8K_-Flttvg5c5rMhFoJlAzFuJo2hzA==
                    RestApiClient _restApiClient = new RestApiClient("https://t8-2020630308-af.azurewebsites.net/api");
                    //String EndPoint = "FnAgregarItemCarrito?code=4uw9OHi5S2j1EroWEg83orMXs3w5XGCwz-PtXY0Klhw1AzFumUv6CA==&IdCarrito=" + idCarrito + "&IdArticulo=" + idArticulo + "&Cantidad=" + Cantidad + "&Precio=" + Precio;

                    String EndPoint = "FnDeleteItemCarrito?code=KbzdT55Bkv-DasJO7vZsdNMr8K_-Flttvg5c5rMhFoJlAzFuJo2hzA==&IdCarrito=" + idCarrito + "&IdArticulo=" + id;
                    string result = await _restApiClient.PostAsync(EndPoint, "{}");
                    if (result.StartsWith("Error")) {
                        TempData["TextData"] = result;
                        ExisteError = true;
                    }
                    else
                    {
                        ExisteError = false ;
                    }
                }
                
            }

            if (ExisteError)
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(Index));
            } 

        }
    }
}
