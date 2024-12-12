using AppNegocio.Class;
using AppNegocio.Enums;
using AppNegocio.Models.Commons;
using AppNegocio.Models.Details;
using AppNegocio.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AppNegocio.ViewModels.PedidoVM
{

    public class EditarPedidoViewModel : INotifyPropertyChanged 
    {
    
        //public void ApplyQueryAttributes(IDictionary<string, object> query)
        //{
        //    if (query.ContainsKey("PedidoAEditar"))
        //    {
        //        PedidoAEditar = query["PedidoAEditar"] as Pedido;
        //        Debug.WriteLine($"[EditarPedidoViewModel] PedidoAEditar recibido: {PedidoAEditar?.id}");
        //    }
        //}

        //private Pedido pedidoAEditar;
        //public Pedido PedidoAEditar
        //{
        //    get => pedidoAEditar;
        //    set
        //    {
        //        pedidoAEditar = value;
        //        OnPropertyChanged();
        //        Debug.WriteLine($"[EditarPedidoViewModel] PedidoAEditar recibido: {pedidoAEditar?.id}");
        //        CargarPedido(pedidoAEditar);

        //        // quiero ver en consola lo que contiene PedidoAEditar
        //        Debug.WriteLine($"[EditarPedidoViewModel] PedidoAEditar recibido: {pedidoAEditar}");



        //    }
        //}
         


        private Pedido pedido;
        public Pedido Pedido
        {
            get => pedido;
            set
            {
                //SetProperty(ref pedido, value);
                pedido = value;
                OnPropertyChanged();
            }
        }

        private Cliente selectedCliente;
        public Cliente SelectedCliente
        {
            get => selectedCliente;
            set
            {
                selectedCliente = value;
                if (NuevoPedido != null)
                {
                    NuevoPedido.ClienteId = selectedCliente?.id ?? 0;
                    NuevoPedido.cliente = selectedCliente; // Asignar cliente completo
                }
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
                if (NuevoPedido != null)
                {
                    NuevoPedido.ModoPagoId = selectedModoPago?.id ?? 0;
                    NuevoPedido.modoPago = selectedModoPago; // Asignar modo de pago completo
                }
                OnPropertyChanged();
            }
        }

        private EstadoPedidoEnum selectedEstadoPedido;
        public EstadoPedidoEnum SelectedEstadoPedido
        {
            get => selectedEstadoPedido;
            set
            {
                selectedEstadoPedido = value;
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

        private bool esEdicion;
        public bool EsEdicion
        {
            get => esEdicion;
            set
            {
                esEdicion = value;
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
        public ICommand GuardarPedidoCommand { get; }

        public EditarPedidoViewModel()
        {
            clienteService = new GenericService<Cliente>();
            modoPagoService = new GenericService<ModoPago>();
            productoService = new GenericService<Producto>();
            impresionService = new GenericService<Impresion>();
            pedidoService = new GenericService<Pedido>();

            // Inicializar la colección de estados del pedido
            EstadosPedido = new ObservableCollection<EstadoPedidoEnum>
    {
        EstadoPedidoEnum.Recepcionado,
        EstadoPedidoEnum.PendientePago
    };

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
            GuardarPedidoCommand = new Command(GuardarPedido);
            CargarDatos();
            
        }



        //public EditarPedidoViewModel(Pedido pedido) : this()
        //{

        //    if (pedido != null)
        //    {
        //        CargarPedido(pedido);
        //    }
        //}
        //public  void CargarPedido()
        //{
        //    // No hacer nada o inicializar valores predeterminados
        //}

        public async Task CargarListas()
        {
            Clientes = new ObservableCollection<Cliente>(await clienteService.GetAllAsync());
            ModosPago = new ObservableCollection<ModoPago>(await modoPagoService.GetAllAsync());
            EstadosPedido = new ObservableCollection<EstadoPedidoEnum>(Enum.GetValues(typeof(EstadoPedidoEnum)).Cast<EstadoPedidoEnum>());

            OnPropertyChanged(nameof(Clientes));
            OnPropertyChanged(nameof(ModosPago));
            OnPropertyChanged(nameof(EstadosPedido));
        }

        private async Task CargarDatos()
        {

            Clientes = new ObservableCollection<Cliente>(await clienteService.GetAllAsync());
            ModosPago = new ObservableCollection<ModoPago>(await modoPagoService.GetAllAsync());
            Productos = new ObservableCollection<Producto>(await productoService.GetAllAsync());
            Impresiones = new ObservableCollection<Impresion>(await impresionService.GetAllAsync());
            EstadosPedido = new ObservableCollection<EstadoPedidoEnum>(Enum.GetValues(typeof(EstadoPedidoEnum)).Cast<EstadoPedidoEnum>());

            // Inicializar las propiedades seleccionadas
            SelectedCliente = Clientes.FirstOrDefault(c => c.id == Pedido.ClienteId);
            SelectedModoPago = ModosPago.FirstOrDefault(m => m.id == Pedido.ModoPagoId);
            Pedido.estadoPedido = EstadosPedido.FirstOrDefault(e => e == Pedido.estadoPedido);
            // Notificar cambios
            OnPropertyChanged(nameof(SelectedCliente));
            OnPropertyChanged(nameof(SelectedModoPago));
            OnPropertyChanged(nameof(Pedido));
            OnPropertyChanged(nameof(EstadosPedido));

            // Notificar cambios
            OnPropertyChanged(nameof(Clientes));
            OnPropertyChanged(nameof(ModosPago));
            OnPropertyChanged(nameof(Productos));
            OnPropertyChanged(nameof(Impresiones));
            OnPropertyChanged(nameof(EstadosPedido));


        }


        public async void CargarPedido()
        {
            try
            {
                ShowActivityIndicator("Cargando pedido...");

                // Obtener el pedido existente por ID
                Pedido pedidoExistente = await pedidoService.GetByIdAsync(pedido.id);
                if (pedidoExistente != null)
                {
                    // Asignar los datos del pedido existente a NuevoPedido
                    NuevoPedido = pedidoExistente;
                    OnPropertyChanged(nameof(NuevoPedido)); // Notificar cambio

                    // Asignar otros datos necesarios
                    SelectedCliente = pedidoExistente.cliente;
                    OnPropertyChanged(nameof(SelectedCliente)); // Notificar cambio
                    SelectedModoPago = pedidoExistente.modoPago;
                    OnPropertyChanged(nameof(SelectedModoPago)); // Notificar cambio
                    EstadosPedido = new ObservableCollection<EstadoPedidoEnum>(Enum.GetValues(typeof(EstadoPedidoEnum)).Cast<EstadoPedidoEnum>());
                    OnPropertyChanged(nameof(EstadosPedido)); // Notificar cambio
                    DetallesProducto = new ObservableCollection<DetalleProducto>(pedidoExistente.DetallesProducto);
                    OnPropertyChanged(nameof(DetallesProducto)); // Notificar cambio
                    DetallesImpresion = new ObservableCollection<DetalleImpresion>(pedidoExistente.DetallesImpresion);
                    OnPropertyChanged(nameof(DetallesImpresion)); // Notificar cambio
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "No se encontró el pedido.", "OK");
                }

                HideActivityIndicator();
            }
            catch (Exception ex)
            {
                HideActivityIndicator();
                var errorDetails = $"Mensaje: {ex.Message}\nStackTrace: {ex.StackTrace}";
                Console.WriteLine(errorDetails);
                await App.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al cargar el pedido: {ex.Message}", "OK");
            }

            CalcularTotales();
        }

        private bool ValidarPedido(Pedido pedido)
        {
            if (pedido.ClienteId == 0)
            {
                Console.WriteLine("Error: Cliente no asignado.");
                return false;
            }

            if (pedido.ModoPagoId == 0)
            {
                Console.WriteLine("Error: Modo de pago no asignado.");
                return false;
            }

            if (!pedido.DetallesProducto.Any() && !pedido.DetallesImpresion.Any())
            {
                Console.WriteLine("Error: No se han agregado productos o impresiones.");
                return false;
            }

            return true;
        }


        private async void GuardarPedido(object obj)
        {
            try
            {
                // Validar datos requeridos
                if (NuevoPedido.ClienteId <= 0 || SelectedCliente == null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Debe seleccionar un cliente.", "OK");
                    return;
                }

                if (NuevoPedido.ModoPagoId <= 0 || SelectedModoPago == null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Debe seleccionar un modo de pago.", "OK");
                    return;
                }

                NuevoPedido.ModoPagoId = SelectedModoPago.id;
                NuevoPedido.modoPago = SelectedModoPago;


                NuevoPedido.estadoPedido = SelectedEstadoPedido;



                if (!DetallesProducto.Any() && !DetallesImpresion.Any())
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Debe agregar al menos un producto o impresión.", "OK");
                    return;
                }

                ShowActivityIndicator("Actualizando pedido...");

                // Actualizar pedido existente
                NuevoPedido.DetallesProducto = DetallesProducto.ToList();
                NuevoPedido.DetallesImpresion = DetallesImpresion.ToList();

                // Limpiar referencias circulares en las colecciones
                NuevoPedido.DetallesImpresion?.ToList().ForEach(detalle => detalle.impresion = null);
                NuevoPedido.DetallesProducto?.ToList().ForEach(detalle => detalle.producto = null);

                if (!ValidarPedido(NuevoPedido))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Hay errores en los datos del pedido. Por favor, revise los campos obligatorios.", "OK");
                    return;
                }

                // Realizar la actualización
                await pedidoService.UpdateAsync(NuevoPedido);

                HideActivityIndicator();
                await App.Current.MainPage.DisplayAlert("Éxito", "Pedido actualizado correctamente.", "OK");

                LimpiarFormulario();

                // Navegar a ResumenPedidoView
                await Shell.Current.GoToAsync("//ListaResumenes");
            }
            catch (Exception ex)
            {
                HideActivityIndicator();
                var errorDetails = $"Mensaje: {ex.Message}\nStackTrace: {ex.StackTrace}";
                Console.WriteLine(errorDetails);
                await App.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al actualizar el pedido: {ex.Message}", "OK");
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
            SelectedProducto = default!;
            SelectedImpresion = default!;
            SelectedCliente = default!;
            SelectedModoPago = default!;
            SelectedEstadoPedido = default!;
            EstadosPedido = new ObservableCollection<EstadoPedidoEnum>();
            PrecioAcumulado = 0;
            TotalProductos = 0;
            TotalImpresiones = 0;

            OnPropertyChanged(nameof(SelectedCliente));
            OnPropertyChanged(nameof(SelectedModoPago));
            OnPropertyChanged(nameof(Pedido));
            OnPropertyChanged(nameof(EstadosPedido));

            OnPropertyChanged(nameof(NuevoPedido)); // Notificar que NuevoPedido ha cambiado
            OnPropertyChanged(nameof(TotalGeneral)); // Notificar cambio de TotalGeneral
        }

        private decimal precioAcumulado;
        public decimal PrecioAcumulado
        {
            get => precioAcumulado;
            set
            {
                precioAcumulado = value;
                OnPropertyChanged();
            }
        }

        private decimal _totalProductos;
        public decimal TotalProductos
        {
            get => _totalProductos;
            set => SetProperty(ref _totalProductos, value);
        }

        private decimal _totalImpresiones;
        public decimal TotalImpresiones
        {
            get => _totalImpresiones;
            set => SetProperty(ref _totalImpresiones, value);
        }

        public decimal TotalGeneral => TotalProductos + TotalImpresiones;

        private void AgregarProducto(object obj)
        {
            if (SelectedProducto != null && CantidadProducto > 0)
            {
                // Control de stock
                if (CantidadProducto > SelectedProducto.stock)
                {
                    App.Current.MainPage.DisplayAlert("Error", "No hay suficiente stock disponible.", "OK");
                    return;
                }

                // Verificar duplicados
                if (DetallesProducto.Any(d => d.ProductoId == SelectedProducto.id))
                {
                    App.Current.MainPage.DisplayAlert("Error", "Este producto ya está agregado.", "OK");
                    return;
                }

                // Crear el detalle del producto
                var detalle = new DetalleProducto
                {
                    ProductoId = SelectedProducto.id,
                    producto = SelectedProducto,
                    cantidad = CantidadProducto,
                    precioUnitario = SelectedProducto.precio
                };

                // Actualizar stock en el producto
                SelectedProducto.stock -= CantidadProducto;

                // Agregar el detalle a la colección
                DetallesProducto.Add(detalle);
                NuevoPedido.DetallesProducto.Add(detalle);

                // Calcular el precio acumulado
                PrecioAcumulado += detalle.precioUnitario * detalle.cantidad;

                // Limpiar los campos de entrada
                CantidadProducto = 0;
                SelectedProducto = null;
                OnPropertyChanged(nameof(DetallesProducto));
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Error", "Seleccione un producto y cantidad válida.", "OK");
            }

            CalcularTotales();
        }

        private void AgregarImpresion(object obj)
        {
            if (SelectedImpresion != null && CantidadImpresion > 0)
            {
                // Crear el detalle de la impresión
                var detalle = new DetalleImpresion
                {
                    ImpresionId = SelectedImpresion.id,
                    impresion = SelectedImpresion,
                    cantidad = CantidadImpresion,
                    precioUnitario = SelectedImpresion.precioBase
                };

                // Agregar el detalle a la colección
                DetallesImpresion.Add(detalle);
                NuevoPedido.DetallesImpresion.Add(detalle);

                // Calcular el precio acumulado
                PrecioAcumulado += detalle.precioUnitario * detalle.cantidad;

                // Limpiar los campos de entrada
                CantidadImpresion = 0;
                SelectedImpresion = null;
                OnPropertyChanged(nameof(DetallesImpresion));
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Error", "Seleccione una impresión y cantidad válida.", "OK");
            }

            CalcularTotales();
        }

        private void CalcularTotales()
        {
            TotalProductos = DetallesProducto.Sum(dp => dp.cantidad * dp.precioUnitario);
            TotalImpresiones = DetallesImpresion.Sum(di => di.cantidad * di.precioUnitario);
            OnPropertyChanged(nameof(TotalGeneral)); // Notifica el total general

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


       

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

       
    }

}
