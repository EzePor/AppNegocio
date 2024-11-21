using AppNegocio.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppNegocio.Models.Commons;
using AppNegocio.Models.Details;
using AppNegocio.Services;
using AppNegocio.Enums;
using CommunityToolkit.Mvvm.Messaging;


namespace AppNegocio.ViewModels.PedidoVM
{
    public class CrearPedidoViewModel : INotifyPropertyChanged
    {
        private Cliente selectedCliente;
        public Cliente SelectedCliente
        {
            get => selectedCliente;
            set
            {
                selectedCliente = value;
                NuevoPedido.ClienteId = selectedCliente?.id ?? 0; // Asignar el ID del cliente seleccionado
                OnPropertyChanged();
            }
        }

        private ModoPago selectedModoPago;
        public ModoPago SelectedModoPago
        {
            get => selectedModoPago;
            set
            {
                selectedModoPago = value;
                NuevoPedido.ModoPagoId = selectedModoPago?.id ?? 0; // Asignar el ID del modo de pago seleccionado
                OnPropertyChanged();
            }
        }



        // Propiedades para los elementos seleccionados
        private Producto selectedProducto;
        public Producto SelectedProducto
        {
            get => selectedProducto;
            set
            {
                selectedProducto = value;
                OnPropertyChanged();
            }
        }

        private Impresion selectedImpresion;
        public Impresion SelectedImpresion
        {
            get => selectedImpresion;
            set
            {
                selectedImpresion = value;
                OnPropertyChanged();
            }
        }

        // Propiedades para la cantidad
        private int cantidadProducto;
        public int CantidadProducto
        {
            get => cantidadProducto;
            set
            {
                cantidadProducto = value;
                OnPropertyChanged();
            }
        }

        private int cantidadImpresion;
        public int CantidadImpresion
        {
            get => cantidadImpresion;
            set
            {
                cantidadImpresion = value;
                OnPropertyChanged();
            }
        }

        private Pedido nuevoPedido;
        public Pedido NuevoPedido
        {
            get => nuevoPedido;
            set
            {
                nuevoPedido = value;
                OnPropertyChanged();
            }
        }

        // Propiedad para almacenar los detalles del pedido
        private ObservableCollection<object> detalles;
        public ObservableCollection<object> Detalles
        {
            get => detalles;
            set
            {
                detalles = value;
                OnPropertyChanged();
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }

        private string activityMessage;
        public string ActivityMessage
        {
            get => activityMessage;
            set
            {
                activityMessage = value;
                OnPropertyChanged();
            }
        }

        private void ShowActivityIndicator(string message)
        {
            ActivityMessage = message;
            IsBusy = true;
        }

        private void HideActivityIndicator()
        {
            ActivityMessage = string.Empty;
            IsBusy = false;
        }
        public ObservableCollection<DetalleProducto> DetallesProducto { get; set; }
        public ObservableCollection<DetalleImpresion> DetallesImpresion { get; set; }

        // Comando para agregar productos o impresiones
        public ICommand AgregarCommand { get; }
        public ICommand AgregarProductoCommand { get; }
        public ICommand AgregarImpresionCommand { get; }
        public ICommand CrearPedidoCommand { get; }

        public CrearPedidoViewModel()
        {
            clienteService = new GenericService<Cliente>();
            modoPagoService = new GenericService<ModoPago>();
            productoService = new GenericService<Producto>();
            impresionService = new GenericService<Impresion>();
            pedidoService = new GenericService<Pedido>();

            DetallesProducto = new ObservableCollection<DetalleProducto>();
            DetallesImpresion = new ObservableCollection<DetalleImpresion>();
            Detalles = new ObservableCollection<object>();

            NuevoPedido = new Pedido
            {
                DetallesProducto = new List<DetalleProducto>(),
                DetallesImpresion = new List<DetalleImpresion>()
            };

            //AgregarCommand = new Command(Agregar);
            AgregarProductoCommand = new Command(AgregarProducto);
            AgregarImpresionCommand = new Command(AgregarImpresion);
            CrearPedidoCommand = new Command(CrearPedido);
            CargarDatos();
        }

       

        private async void CrearPedido(object obj)
        {
            // necesito crear la lógica para guardar un pedido completo según el modelo de pedido 

            try
            {
                // Validar datos requeridos
                if (NuevoPedido.ClienteId <= 0  || SelectedCliente == null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Debe seleccionar un cliente.", "OK");
                    return;
                }

                if (NuevoPedido.ModoPagoId <= 0)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Debe seleccionar un modo de pago.", "OK");
                    return;
                }

                if (!DetallesProducto.Any() && !DetallesImpresion.Any())
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Debe agregar al menos un producto o impresión.", "OK");
                    return;
                }

                ShowActivityIndicator("Guardando pedido...");

                // Crear un nuevo pedido basado en los datos actuales
                var nuevoPedido = new Pedido
                {
                    ClienteId = NuevoPedido.ClienteId,
                    cliente = SelectedCliente,
                    fechaPedido = DateTime.Now,
                    estadoPedido = NuevoPedido.estadoPedido,
                    ModoPagoId = NuevoPedido.ModoPagoId,
                    modoPago = SelectedModoPago,
                    FuePagado = NuevoPedido.FuePagado,
                    DetallesProducto = DetallesProducto.ToList(),
                    DetallesImpresion = DetallesImpresion.ToList()
                };

                await pedidoService.AddAsync(nuevoPedido);

                HideActivityIndicator();
                await App.Current.MainPage.DisplayAlert("Éxito", "Pedido guardado correctamente.", "OK");
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                HideActivityIndicator();
                await App.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al guardar el pedido: {ex.Message}", "OK");
            }
        }

        private void LimpiarFormulario()
        {
            NuevoPedido = new Pedido
            {
                DetallesProducto = new List<DetalleProducto>(),
                DetallesImpresion = new List<DetalleImpresion>()
            };
            DetallesProducto.Clear();
            DetallesImpresion.Clear();
            Detalles.Clear();
            CantidadProducto = 0;
            CantidadImpresion = 0;
            SelectedProducto = null;
            SelectedImpresion = null;
        }

        private void AgregarProducto(object obj)
        {
            if (SelectedProducto != null && CantidadProducto > 0)
            {
                var detalle = new DetalleProducto
                {
                    ProductoId = SelectedProducto.id, // Asegúrate de que SelectedProducto tiene un ID válido
                    producto = SelectedProducto,
                    cantidad = CantidadProducto,
                    precioUnitario = SelectedProducto.precio,
                    

                };
                DetallesProducto.Add(detalle);
                NuevoPedido.DetallesProducto.Add(detalle);
                CantidadProducto = 0;
                SelectedProducto = null;
                OnPropertyChanged(nameof(DetallesProducto));
            }
        }

        private void AgregarImpresion(object obj)
        {
            if (SelectedImpresion != null && CantidadImpresion > 0)
    {
        var detalle = new DetalleImpresion
        {
            ImpresionId = SelectedImpresion.id, 
            impresion = SelectedImpresion,
            cantidad = CantidadImpresion,
            precioUnitario = SelectedImpresion.precioBase,
           

        };
        DetallesImpresion.Add(detalle);
        NuevoPedido.DetallesImpresion.Add(detalle);
        CantidadImpresion = 0;
        SelectedImpresion = null;
        OnPropertyChanged(nameof(DetallesImpresion));
    }
        }

        //private void Agregar()
        //{

        //    if (SelectedProducto != null && CantidadProducto > 0)
        //    {
        //        // Agregar producto al detalle con la cantidad especificada
        //        var detalleProducto = new DetalleProducto { producto = SelectedProducto, cantidad = CantidadProducto, precioUnitario = SelectedProducto.precio };
        //        DetallesProducto.Add(detalleProducto);
        //    }
        //    else if (SelectedImpresion != null && CantidadImpresion > 0)
        //    {
        //        // Agregar impresión al detalle con la cantidad especificada
        //        var detalleImpresion = new DetalleImpresion { impresion = SelectedImpresion, cantidad = CantidadImpresion, precioUnitario = SelectedImpresion.precioBase };
        //        DetallesImpresion.Add(detalleImpresion);
        //    }

        //    // Combinar las listas en la colección Detalles
        //    Detalles.Clear();
        //    foreach (var detalle in DetallesProducto)
        //    {
        //        Detalles.Add(detalle);
        //    }
        //    foreach (var detalle in DetallesImpresion)
        //    {
        //        Detalles.Add(detalle);
        //    }
        //}

        private readonly GenericService<Cliente> clienteService;
        private readonly GenericService<ModoPago> modoPagoService;
        private readonly GenericService<Producto> productoService;
        private readonly GenericService<Impresion> impresionService;
        private readonly GenericService<Pedido> pedidoService;

        private ObservableCollection<Cliente> clientes;
        private ObservableCollection<ModoPago> modosPago;
        private ObservableCollection<Producto> productos;
        private ObservableCollection<Impresion> impresiones;
        private ObservableCollection<EstadoPedidoEnum> estadosPedido;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Cliente> Clientes
        {
            get => clientes;
            set
            {
                clientes = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ModoPago> ModosPago
        {
            get => modosPago;
            set
            {
                modosPago = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Producto> Productos
        {
            get => productos;
            set
            {
                productos = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Impresion> Impresiones
        {
            get => impresiones;
            set
            {
                impresiones = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<EstadoPedidoEnum> EstadosPedido
        {
            get => estadosPedido;
            set
            {
                estadosPedido = value;
                OnPropertyChanged();
            }
        }

       

        private async Task CargarDatos()
        {
            Clientes = new ObservableCollection<Cliente>(await clienteService.GetAllAsync());
            ModosPago = new ObservableCollection<ModoPago>(await modoPagoService.GetAllAsync());
            Productos = new ObservableCollection<Producto>(await productoService.GetAllAsync());
            Impresiones = new ObservableCollection<Impresion>(await impresionService.GetAllAsync());
            EstadosPedido = new ObservableCollection<EstadoPedidoEnum>(Enum.GetValues(typeof(EstadoPedidoEnum)).Cast<EstadoPedidoEnum>());
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
