using AppNegocio.Class;
using AppNegocio.Models.Details;
using AppNegocio.Services;
using AppNegocio.Views.PedidoV;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppNegocio.ViewModels.PedidoVM
{
    public class ResumenPedidoViewModel : NotificationObject
    {
       GenericService<DetalleProducto> detalleProductoService = new GenericService<DetalleProducto>();
        GenericService<DetalleImpresion> detalleImpresionService = new GenericService<DetalleImpresion>();
        GenericService<Pedido> pedidoService = new GenericService<Pedido>();

        private bool activityStart;
        public bool ActivityStart
        {
            get { return activityStart; }
            set
            {
                activityStart = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DetalleProducto> detalleProductos;
        public ObservableCollection<DetalleProducto> DetalleProductos
        {
            get { return detalleProductos; }
            set
            {
                detalleProductos = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<DetalleImpresion> detalleImpresiones;
        public ObservableCollection<DetalleImpresion> DetalleImpresiones
        {
            get { return detalleImpresiones; }
            set
            {
                detalleImpresiones = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Pedido> pedidos;
        public ObservableCollection<Pedido> Pedidos
        {
            get { return pedidos; }
            set
            {
                pedidos = value;
                OnPropertyChanged();
            }
        }

        private Pedido pedido;
        public Pedido Pedido
        {
            get { return pedido; }
            set
            {
                pedido = value;
                OnPropertyChanged();
            }
        }

        private Pedido pedidoSeleccionado;
        public Pedido PedidoSeleccionado
        {
            get { return pedidoSeleccionado; }
            set
            {
                pedidoSeleccionado = value;
                OnPropertyChanged();
            }
        }

        private Pedido pedidoCurrent;
        public Pedido PedidoCurrent
        {
            get { return pedidoCurrent; }
            set
            {
                pedidoCurrent = value;
                OnPropertyChanged();
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public Command DetalleCommand { get; }

        public ResumenPedidoViewModel()
        {
            DetalleProductos = new ObservableCollection<DetalleProducto>();
            DetalleImpresiones = new ObservableCollection<DetalleImpresion>();
            DetalleCommand = new Command(Detalle);

            Task.Run(async () => await CargarResumen());

            // Suscripción al mensaje para refrescar la lista de clientes
            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                if (m.Value == "VolverAResumenes" || m.Value == "RefrescarListaResumenes")
                {
                    Task.Run(async () => await RefreshResumenes());
                }
            });
        }

        private async Task RefreshResumenes()
        {
            IsRefreshing = true;
            await CargarResumen();
            IsRefreshing = false;
        }

        public async Task CargarResumen()
        {
            ActivityStart = true;
            var pedidos = await pedidoService.GetAllAsync();
            Pedidos = new ObservableCollection<Pedido>(pedidos); 
            ActivityStart = false;
        }

        private async void Detalle(object obj)
        {
            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "PedidoAMostrar", pedidoCurrent }
            };
            await Shell.Current.GoToAsync("//DetallePedido", navigationParameter);
        }
    }
}
