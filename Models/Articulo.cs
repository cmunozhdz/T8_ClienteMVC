

using System.Net.NetworkInformation;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CteTarea8MVC.Models
{

    public class Articulo
    {
        public int Id { get; set; }
        [StringLength(150, MinimumLength =3,ErrorMessage ="Minimo 3 caracteres por favor." ) ]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Requerido")]
        public string Descripcion { get; set; }
        [Required(AllowEmptyStrings = false,ErrorMessage = "Requerido") ]
        public Double Cantidad { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Requerido")]
        public Double Precio { get; set; }
        [Display(Name ="Foto")]
        public String FotoUrl { get; set; }
        
        // MÉTODOS PÚBLICOS
        public void SubirArchivo(byte[] archivo)
        {
            File.WriteAllBytes(this.PathCompleto, archivo);
        }
        public string Extension { get; set; }

        public string PathRelativo
        {
            get
            {
               
                return System.Configuration.ConfigurationManager.AppSettings["PathArchivos"] +
                                            this.Id.ToString() + "." +
                                            this.Extension;
            }
        }

        public string PathCompleto
        {
            get
            {

                var _PathAplicacion = "C:\\Prrojects\\CteTarea8MVC\\wwwroot";  //Request.ApplicationPath;


                
                return Path.Combine(_PathAplicacion, this.PathRelativo);
            }
        }

        public byte[] DescargarArchivo()
        {
            return File.ReadAllBytes(this.PathCompleto);
        }

        public void EliminarArchivo()
        {
            File.Delete(this.PathCompleto);
        }

    }
}
