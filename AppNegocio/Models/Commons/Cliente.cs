using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNegocio.Models.Details;

namespace AppNegocio.Models.Commons
{
    public class Cliente
    {

        public int id { get; set; }

        [Required(ErrorMessage = "El apellido y el nombre debe cargarse obligatoriamente")]
        public string apellidoNombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El cuit/dni debe cargarse obligatoriamente")]
        public string cuitDni { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección debe cargarse obligatoriamente")]
        public string direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El telefono debe cargarse obligatoriamente")]
        public string telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email debe cargarse obligatoriamente")]
        public string email { get; set; } = string.Empty;

        public string Localidad { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;

        public bool eliminado { get; set; } = false;

        public ICollection<Pedido>? Pedidos { get; set; }

        public override string ToString()
        {
            return apellidoNombre;
        }
    }
}
