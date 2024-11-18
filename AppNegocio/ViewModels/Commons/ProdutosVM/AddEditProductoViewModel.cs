using AppNegocio.Class;
using AppNegocio.Enums;
using AppNegocio.Models.Commons;
using AppNegocio.Services;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNegocio.ViewModels.Commons.ProdutosVM
{
    public class AddEditProductoViewModel : NotificationObject
    {
        GenericService<Producto> productoService = new GenericService<Producto>();

        public ObservableCollection<RubroEnum> Rubros { get; set; }

        private Producto producto;
        public Producto Producto
        {
            get { return producto; }
            set
            {
                producto = value;
                if (producto != null)
                {
                    Nombre = producto.nombre;
                    Rubro = producto.Rubro;
                    Precio = producto.precioF;
                    Stock = producto.stock;
                    // Guardamos el estado original
                    OriginalProducto = new Producto()
                    {
                        nombre = producto.nombre,
                        Rubro = producto.Rubro,
                        precioF = producto.precioF,
                        stock = producto.stock
                    };

                    OnPropertyChanged();
                }
                else
                {
                    LimpiarFormulario();
                }
            }
        }

        // Guardamos el producto original para comparar
        private Producto OriginalProducto;

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set
            {
                nombre = value;
                OnPropertyChanged();
                GuardarCommand.ChangeCanExecute();
            }
        }

        private RubroEnum rubro;
        public RubroEnum Rubro
        {
            get { return rubro; }
            set
            {
                rubro = value;
                OnPropertyChanged();
                GuardarCommand.ChangeCanExecute();
            }
        }

        private string precio;
        public string Precio
        {
            get { return precio; }
            set
            {
                precio = value;
                OnPropertyChanged();
                GuardarCommand.ChangeCanExecute();
            }
        }

        private int stock;
        public int Stock
        {
            get { return stock; }
            set
            {
                stock = value;
                OnPropertyChanged();
                GuardarCommand.ChangeCanExecute();
            }
        }

        public Command GuardarCommand { get; }
        public Command CancelarCommand { get; }

        public AddEditProductoViewModel()
        {
            Rubros = new ObservableCollection<RubroEnum>((RubroEnum[])Enum.GetValues(typeof(RubroEnum)));

            GuardarCommand = new Command(Guardar, PermitirGuardar);
            CancelarCommand = new Command(Cancelar);

            LimpiarFormulario();
           
        }

        private void LimpiarFormulario()
        {
            Producto = new Producto();
            Nombre = string.Empty;
            Rubro = RubroEnum.Otros;
            Precio = string.Empty;
            Stock = 0;
        }

        private async void Cancelar(object obj)
        {
            LimpiarFormulario();
            await Shell.Current.GoToAsync("//ListaProductos");
        }

        private bool PermitirGuardar(object arg)
        {
            // Verificar si algún campo ha cambiado en comparación con el producto original
            bool hayCambios =
                Nombre != OriginalProducto.nombre ||
                Rubro != OriginalProducto.Rubro ||
                Precio != OriginalProducto.precioF ||
                Stock != OriginalProducto.stock;

            // Validar los campos antes de permitir guardar
            bool camposValidos =
                !string.IsNullOrEmpty(Nombre) &&
                Enum.IsDefined(typeof(RubroEnum), Rubro) &&
                decimal.TryParse(Precio, out decimal precioDecimal) && precioDecimal > 0 &&
                Stock > 0;

            return hayCambios && camposValidos;
        }

        private async void Guardar(object obj)
        {
            bool esNuevo = Producto.id == 0;
            if (esNuevo)
            {
                var nuevoProducto = new Producto()
                {
                    nombre = Nombre,
                    Rubro = Rubro,
                    precioF = Precio,
                    stock = Stock
                };
                await productoService.AddAsync(nuevoProducto);
            }
            else
            {
                Producto.nombre = Nombre;
                Producto.Rubro = Rubro;
                Producto.precioF = Precio;
                Producto.stock = Stock;

                await productoService.UpdateAsync(Producto);
            }

            // Mostrar mensaje de éxito
            string mensaje = esNuevo ? "Producto agregado correctamente" : "Producto actualizado correctamente";
            await Application.Current.MainPage.DisplayAlert("Éxito", mensaje, "OK");

            // Enviar mensaje para refrescar la lista en ClientesViewModel
            WeakReferenceMessenger.Default.Send(new MyMessage("RefrescarListaProductos"));

            // Limpiar el formulario después de guardar
            LimpiarFormulario();

            // Navegar de vuelta a la lista de clientes
            await Shell.Current.GoToAsync("//ListaProductos");
        }
    }
}