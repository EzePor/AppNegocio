using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNegocio.Models.Commons
{
    public class Impresion
    {

        public int id { get; set; }
        [Required(ErrorMessage = "El tamaño debe cargarse obligatoriamente")]
        public string tamanio { get; set; } = string.Empty;
        [Required(ErrorMessage = "El precio debe cargarse obligatoriamente")]

        public decimal precioBase { get; set; }
        [NotMapped]
        public string precioF
        {
            get { return precioBase.ToString("0.00"); }
            set
            {
                if (decimal.TryParse(value, out var parsedValue))
                {
                    precioBase = Math.Round(parsedValue, 2);
                }
            }
        }
        public decimal descuento10 { get; set; }
        public decimal descuento20 { get; set; }
        public decimal descuento50 { get; set; }

        public bool eliminado { get; set; } = false;

        public decimal CalcularPrecioFinal(int cantidad)
        {
            if (cantidad >= 50) return precioBase * (1 - descuento50);
            if (cantidad >= 20) return precioBase * (1 - descuento20);
            if (cantidad >= 10) return precioBase * (1 - descuento10);
            return precioBase;
        }

        public override string ToString()
        {
            return $"{tamanio} - Precio: {precioBase.ToString("0.00")}";
        }
    }
}

