using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppNegocio.Enums;

namespace AppNegocio.Models.Commons
{
    public class Producto
    {

        public int id { get; set; }
        [Required(ErrorMessage = "El nombre del producto debe cargarse obligatoriamente")]
        public string nombre { get; set; } = string.Empty;
        [Required(ErrorMessage = "El rubro debe cargarse obligatoriamente")]

        public RubroEnum Rubro { get; set; }
        [Required(ErrorMessage = "El precio debe cargarse obligatoriamente")]

        public decimal precio { get; set; }

        [NotMapped]
        public string precioF
        {
            get { return precio.ToString("0.00"); }
            set
            {
                if (decimal.TryParse(value, out var parsedValue))
                {
                    precio = Math.Round(parsedValue, 2);
                }
            }
        }


        [Required(ErrorMessage = "El stock debe cargarse obligatoriamente")]

        public int stock { get; set; }

        public bool eliminado { get; set; } = false;

        public override string ToString()
        {
            return nombre;
        }
    }
}
