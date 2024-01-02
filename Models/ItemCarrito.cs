using System.ComponentModel.DataAnnotations;

namespace CteTarea8MVC.Models
{
    public class ItemCarrito
    {
        [Display(Name = "Id Carrito")]
        public int _IdCarrito { get; set; }
        //public datetime Fecha; 
        [Display(Name = "Id Articulo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int IdArticulo { get; set; }
        public String Descripcion { get; set; }
        [Required(ErrorMessage="El campo {0} es obligatorio." )]
        [Range(10, 20)]
        public double Cantidad { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public double Precio { get; set; }
        public String Archivo { get; set; }
        [Display (Name ="Foto")]
        public String SFoto { get; set; }
        [Display(Name = "Sub Total")]
        public double Subtotal {  
            get { return Cantidad * Precio; }
                }

    }
    
}
