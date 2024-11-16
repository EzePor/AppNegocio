using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppNegocio.Enums;
using AppNegocio.Models.Commons;
using AppNegocio.Models.Details;


namespace WebNegocio.Models.Details
{
    public class Pedido
    {
        public int id { get; set; }
        [Required(ErrorMessage = "El cliente debe cargarse obligatoriamente")]
        public int ClienteId { get; set; }
        public Cliente? cliente { get; set; }
        [Required(ErrorMessage = "La fecha del pedido debe cargarse obligatoriamente")]
        public DateTime fechaPedido { get; set; }
        [Required(ErrorMessage = "El modo de pago debe cargarse obligatoriamente")]
        public int ModoPagoId { get; set; }
        public ModoPago? modoPago { get; set; }
        [Required(ErrorMessage = "El estado del pedido debe cargarse obligatoriamente")]

        public EstadoPedidoEnum estadoPedido { get; set; }
        [Required(ErrorMessage = "El estado del pago debe cargarse obligatoriamente")]

        public bool FuePagado { get; set; } = false;

        public bool eliminado { get; set; } = false;

        // Relación: Un pedido puede tener múltiples productos y/o impresiones
        public ICollection<DetalleProducto>? DetallesProducto { get; set; } = new List<DetalleProducto>();  // Inicializar la colección
        public ICollection<DetalleImpresion>? DetallesImpresion { get; set; } = new List<DetalleImpresion>();  // Inicializar la colección


        // Propiedad calculada para el total del pedido
        public decimal TotalPedido
        {
            get
            {
                decimal totalDetalleProducto = DetallesProducto?.Sum(dp => dp.total  ) ?? 0m;
                decimal totalDetalleImpresion = DetallesImpresion?.Sum(di => di.total) ?? 0m;

                return totalDetalleProducto + totalDetalleImpresion;
            }
        }

        [NotMapped]
        public string TotalPedidoF
        {
            get { return TotalPedido.ToString("0.00"); }
            
           
        }
        public override string ToString()
        {
            return $"{cliente?.apellidoNombre}" ?? string.Empty;
        }
    }
}
