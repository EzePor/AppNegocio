using AppNegocio.Class;
using AppNegocio.Models.Commons;
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
        GenericService<ModoPago> modoPagoService = new GenericService<ModoPago>();

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
            get => pedidoSeleccionado;
            set
            {
                if (pedidoSeleccionado != value)
                {
                    pedidoSeleccionado = value;
                    OnPropertyChanged();
                    Debug.WriteLine($"[PedidoSeleccionado] Cambiado: {pedidoSeleccionado?.id}");
                }
            }
        }

        private ModoPago modoPago;
        public ModoPago ModoPago
        {
            get
            {
                return modoPago;
            }
            set { modoPago = value;
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
                Debug.WriteLine($"[PedidoCurrent] Cambiado: {pedidoCurrent?.id}");
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
       public Command EditarCommand { get; }

       

        public ResumenPedidoViewModel()
        {
            DetalleProductos = new ObservableCollection<DetalleProducto>();
            DetalleImpresiones = new ObservableCollection<DetalleImpresion>();
            DetalleCommand = new Command(Detalle);
            EditarCommand = new Command(Editar, PermitirEditarPedido);

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

        private bool PermitirEditarPedido(object arg)
        {
            Debug.WriteLine($"PermitirEditarPedido: PedidoSeleccionado = {PedidoSeleccionado}");
            return PedidoSeleccionado != null;
        }

        private async void Editar(object obj)
        {
            if (PedidoSeleccionado != null)
            {
                Debug.WriteLine($"[ResumenPedidoViewModel] Navegando a EditarPedidoView con PedidoSeleccionado: {PedidoSeleccionado?.id}");
                var navigationParameter = new Dictionary<string, object>
        {
            { "PedidoAEditar", PedidoSeleccionado }
        };

                // Imprimir los parámetros que se están enviando
                foreach (var param in navigationParameter)
                {
                    Debug.WriteLine($"Enviando parámetro: {param.Key} = {param.Value}");
                }

                await Shell.Current.GoToAsync("//EditarPedidoView", navigationParameter);
            }
            else
            {
                Debug.WriteLine("[ResumenPedidoViewModel] PedidoSeleccionado es nulo.");
            }

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

            // Cargar todos los modos de pago
            var modosPago = await modoPagoService.GetAllAsync();
            var modosPagoDict = modosPago.ToDictionary(m => m.id);

            // Asignar los modos de pago a los pedidos
            foreach (var pedido in pedidos)
            {
                if (pedido.ModoPagoId > 0 && modosPagoDict.TryGetValue(pedido.ModoPagoId, out var modoPago))
                {
                    pedido.modoPago = modoPago;
                }
            }

            //foreach (var pedido in pedidos)
            //{
            //    if (pedido.ModoPagoId > 0)
            //    {
            //        pedido.modoPago = await modoPagoService.GetByIdAsync(pedido.ModoPagoId);
            //    }
            //}

            // Verificar que ModoPago se asigna correctamente
            //foreach (var pedido in Pedidos)
            //{
            //    Debug.WriteLine($"Pedido ID: {pedido.id}, ModoPago: {pedido.modoPago?.nombre}");
            //}


            ActivityStart = false;
        }

        private async void Detalle(object obj)
        {
            if (PedidoSeleccionado != null)
            {
                var navigationParameter = new ShellNavigationQueryParameters
        {
            { "PedidoAMostrar", PedidoSeleccionado }
        };
                await Shell.Current.GoToAsync("//DetallePedido", navigationParameter);
            }
            else
            {
                Debug.WriteLine("[ResumenPedidoViewModel] PedidoSeleccionado es nulo.");
            }
        }
    }
}
