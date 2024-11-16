using AppNegocio.Class;
using AppNegocio.Models.Commons;
using AppNegocio.Services;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace AppNegocio.ViewModels.Commons.ClienteVM
{
    public class ClientesViewModel : NotificationObject
    {
        GenericService<Cliente> clienteService = new GenericService<Cliente>();


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

        private ObservableCollection<Cliente> clientes;
        public ObservableCollection<Cliente> Clientes
        {
            get { return clientes; }
            set
            {
                clientes = value;
                OnPropertyChanged();
            }
        }

        private Cliente clienteCurrent;

        public Cliente ClienteCurrent
        {
            get { return clienteCurrent; }
            set
            {
                clienteCurrent = value;
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


        public ClientesViewModel()
        {
            AgregarCommand = new Command(Agregar);
            EditarCommand = new Command(Editar, PermitirEditar);
            EliminarCommand = new Command(Eliminar, PermitirEliminar);
            Task.Run(async () => await CargarClientes());

            // Suscripción al mensaje para refrescar la lista de clientes
            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                if (m.Value == "VolverAClientes" || m.Value == "RefrescarListaClientes")
                {
                    Task.Run(async () => await RefreshClientes());
                }
            });
        }

        private async Task RefreshClientes()
        {
            IsRefreshing = true;
            await CargarClientes();
            IsRefreshing = false;
        }

        //private async void AlRecibirMensaje(MyMessage m)
        //{
        //    if (m.Value == "VolverAClientes")
        //    {
        //        await RefreshClientes(this);
        //    }
        //}

        //private async Task RefreshClientes(ClientesViewModel clientesViewModel)
        //{
        //    IsRefreshing = true;
        //    await CargarClientes();
        //    IsRefreshing = false;
        //}



        private async Task CargarClientes()
        {
            ActivityStart = true;
            var clientes = await clienteService.GetAllAsync();
            Clientes = new ObservableCollection<Cliente>(clientes); ;
            ActivityStart = false;
        }

        private bool PermitirEliminar(object arg)
        {
            return clienteCurrent != null;
        }

        private async void Eliminar(object obj)
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert("Eliminar una cliente", $"Está seguro que desea eliminar el cliente {clienteCurrent.apellidoNombre}", "Si", "No");
            if (respuesta)
            {
                ActivityStart = true;
                await clienteService.DeleteAsync(clienteCurrent.id);
                await CargarClientes();
            }
        }

        private bool PermitirEditar(object arg)
        {
            return clienteCurrent != null;
        }

        private async void Editar(object obj)
        {
            
            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "ClienteAEditar", clienteCurrent }
            };
            await Shell.Current.GoToAsync("//AgregarEditarCliente", navigationParameter);
            
        }

        private async void Agregar(object obj)
        {
            
            //var navigationParameter = new ShellNavigationQueryParameters
            //{
            //    { "ClienteAEditar", null }
            //};
            //await Shell.Current.GoToAsync("//AgregarEditarCliente", navigationParameter);
            await Shell.Current.GoToAsync("//AgregarEditarCliente");
        }
    }
}
