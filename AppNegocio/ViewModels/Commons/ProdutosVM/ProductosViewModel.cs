using AppNegocio.Class;
using AppNegocio.Interfaces;
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
    public class ProductosViewModel : NotificationObject
    {
        GenericService<Producto> productoService = new GenericService<Producto>();

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

        private ObservableCollection<Producto> productos = new ObservableCollection<Producto>();
        public ObservableCollection<Producto> Productos
        {
            get { return productos; }
            set
            {
                productos = value;
                OnPropertyChanged();
            }
        }

        private Producto productoCurrent;
        public Producto ProductoCurrent
        {
            get { return productoCurrent; }
            set
            {
                productoCurrent = value;
                OnPropertyChanged();

                EditarCommand.ChangeCanExecute();
                EliminarCommand.ChangeCanExecute();
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

        public Command AgregarCommand { get; }
        public Command EditarCommand { get; }
        public Command EliminarCommand { get; }


        public ProductosViewModel()
        {
            AgregarCommand = new Command(Agregar);
            EditarCommand = new Command(Editar, PermitirEditar);
            EliminarCommand = new Command(Eliminar, PermitirEliminar);

            // Suscripción al mensaje para refrescar la lista de productos
            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                if (m.Value == "VolverAProductos" || m.Value == "RefrescarListaProductos")
                {
                    Task.Run(async () => await RefreshProductos(this));
                }
            });

            // Cargar productos al iniciar
            Task.Run(async () => await CargarProductos());
        }

        private async Task RefreshProductos(object obj)
        {
            IsRefreshing = true;
            await CargarProductos();
            IsRefreshing = false;
        }

        private async Task CargarProductos()
        {
            ActivityStart = true;
            var productos = await productoService.GetAllAsync();
            Productos = new ObservableCollection<Producto>(productos ?? new List<Producto>());
            ActivityStart = false;
        }

        private bool PermitirEliminar(object arg)
        {
            return ProductoCurrent != null;
        }

        private async void Eliminar(object obj)
        {
            if (productoCurrent == null) return;

            bool respuesta = await Application.Current.MainPage.DisplayAlert("Eliminar un producto", $"Está seguro que desea eliminar el producto {productoCurrent.nombre}", "Si", "No");
            if (respuesta)
            {
                ActivityStart = true;
                await productoService.DeleteAsync(productoCurrent.id);
                await CargarProductos();
            }
        }

        private bool PermitirEditar(object arg)
        {
            return ProductoCurrent != null;
        }

        private async void Editar(object obj)
        {
            if (productoCurrent == null) return;

            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "ProductoAEditar", productoCurrent }
            };
            await Shell.Current.GoToAsync("//AgregarEditarProducto", navigationParameter);

            // Deshabilitar los botones Editar y Eliminar después de la edición
            ProductoCurrent = null;
            EditarCommand.ChangeCanExecute();
            EliminarCommand.ChangeCanExecute();
        }

        private async void Agregar(object obj)
        {
            await Shell.Current.GoToAsync("//AgregarEditarProducto");
        }
    }
}
