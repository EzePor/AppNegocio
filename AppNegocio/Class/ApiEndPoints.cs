using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNegocio.Class
{
    internal class ApiEndPoints
    {

        public static string Cliente { get; set; } = "clientes";
        public static string ModoPago { get; set; } = "modopagos";
        public static string Pedido { get; set; } = "pedidos";
        public static string DetalleProducto { get; set; } = "detalleproductos";
        public static string DetalleImpresion { get; set; } = "detalleimpresiones";
        public static string ResumenPedido { get; set; } = "pedidos";
        public static string EstadoPedido { get; set; } = "pedidos/estado";
        public static string Producto { get; set; } = "productos";
        public static string Impresion { get; set; } = "impresiones";
        public static string Usuario { get; set; } = "usuarios";
        public static string Empleado { get; set; } = "empleados";

        public static string GetEndPoint(string name)
        {
            return name switch
            {
                nameof(Cliente) => Cliente,
                nameof(ModoPago) => ModoPago,
                nameof(Pedido) => Pedido,
                nameof(DetalleProducto) => DetalleProducto,
                nameof(DetalleImpresion) => DetalleImpresion,
                nameof(ResumenPedido) => ResumenPedido,
                nameof(Producto) => Producto,
                nameof(Impresion) => Impresion,
                nameof(Usuario) => Usuario,
                nameof(Empleado) => Empleado,
                nameof(EstadoPedido) => EstadoPedido,
                _ => throw new ArgumentException($"Endpoint '{name}' no está definido.")
            };
        }
    }
}
