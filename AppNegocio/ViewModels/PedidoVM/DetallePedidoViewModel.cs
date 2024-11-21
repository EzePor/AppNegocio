using AppNegocio.Class;
using AppNegocio.Models.Details;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNegocio.ViewModels.PedidoVM
{
    [QueryProperty(nameof(Pedido), "PedidoAMostrar")]
    public class DetallePedidoViewModel : ObservableObject 
    {

        private Pedido pedido;
        public Pedido Pedido
        {
            get => pedido;
            set
            {
                SetProperty(ref pedido, value);
                if (pedido != null)
                {
                    CargarDetalles();
                }
            }
        }

        public ObservableCollection<DetalleProducto> DetalleProductos { get; set; } = new ObservableCollection<DetalleProducto>();
        public ObservableCollection<DetalleImpresion> DetalleImpresiones { get; set; } = new ObservableCollection<DetalleImpresion>();

        public DetallePedidoViewModel()
        {
            Pedido = new Pedido();
        }

        private void CargarDetalles()
        {
            // Verifica si el Pedido y sus detalles no son nulos antes de asignar
            if (Pedido != null)
            {
                DetalleProductos.Clear();
                DetalleImpresiones.Clear();

                if (Pedido.DetallesProducto != null)
                    foreach (var producto in Pedido.DetallesProducto)
                        DetalleProductos.Add(producto);

                if (Pedido.DetallesImpresion != null)
                    foreach (var impresion in Pedido.DetallesImpresion)
                        DetalleImpresiones.Add(impresion);

                // Notifica los cambios para que la vista se actualice
                OnPropertyChanged(nameof(DetalleProductos));
                OnPropertyChanged(nameof(DetalleImpresiones));
            }
        }
    }
}
