using AppNegocio.Class;
using AppNegocio.Models.Commons;
using AppNegocio.Services;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;


namespace AppNegocio.ViewModels.Commons.ClientesViewModels
{
    public class AddEditClienteViewModel : NotificationObject
    {
        GenericService<Cliente> clienteService = new GenericService<Cliente>();

        private Cliente cliente;
        public Cliente Cliente
        {
            get { return cliente; }
            set
            {
                cliente = value;
                if (value != null)
                {
                    ApellidoNombre = cliente.apellidoNombre;
                    CuitDni = cliente.cuitDni;
                    Direccion = cliente.direccion;
                    Telefono = cliente.telefono;
                    Email = cliente.email;
                    Localidad = cliente.Localidad;
                    CodigoPostal = cliente.CodigoPostal;
                    Provincia = cliente.Provincia;

                    OnPropertyChanged();
                }
                else
                {
                    ApellidoNombre = string.Empty;
                    CuitDni = string.Empty;
                    Direccion = string.Empty;
                    Telefono = string.Empty;
                    Email = string.Empty;
                    Localidad = string.Empty;
                    CodigoPostal = string.Empty;
                    Provincia = string.Empty;
                    OnPropertyChanged();
                }

            }
        }

        private string apellidoNombre;
        public string ApellidoNombre
        {
            get { return apellidoNombre; }
            set
            {
                apellidoNombre = value;
                OnPropertyChanged();
                GuardarCommand.ChangeCanExecute();
            }
        }
        private string cuitDni;
        public string CuitDni
        {
            get { return cuitDni; }
            set
            {
                cuitDni = value;
                OnPropertyChanged();
                GuardarCommand.ChangeCanExecute();
            }
        }
        private string direccion;
        public string Direccion
        {
            get { return direccion; }
            set
            {
                direccion = value;
                OnPropertyChanged();
                GuardarCommand.ChangeCanExecute();
            }
        }
        private string telefono;
        public string Telefono
        {
            get { return telefono; }
            set
            {
                telefono = value;
                OnPropertyChanged();
                GuardarCommand.ChangeCanExecute();
            }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
                GuardarCommand.ChangeCanExecute();
            }
        }
        private string localidad;
        public string Localidad
        {
            get { return localidad; }
            set
            {
                localidad = value;
                OnPropertyChanged();
                GuardarCommand.ChangeCanExecute();
            }
        }
        private string codigoPostal;
        public string CodigoPostal
        {
            get { return codigoPostal; }
            set
            {
                codigoPostal = value;
                OnPropertyChanged();
                GuardarCommand.ChangeCanExecute();
            }
        }
        private string provincia;
        public string Provincia
        {
            get { return provincia; }
            set
            {
                provincia = value;
                OnPropertyChanged();
                GuardarCommand.ChangeCanExecute();
            }
        }


        public Command GuardarCommand { get; }
        public Command CancelarCommand { get; }

        public AddEditClienteViewModel()
        {
            GuardarCommand = new Command(Guardar, Permitirguardar);
            CancelarCommand = new Command(Cancelar);
            LimpiarFormulario();
            Cliente = new Cliente();
        }

        private async void Cancelar(object obj)
        {
            LimpiarFormulario();
            await Shell.Current.GoToAsync("//ListaClientes");
        }

        private bool Permitirguardar(object arg)
        {
            return !string.IsNullOrEmpty(ApellidoNombre) && !string.IsNullOrEmpty(CuitDni) && !string.IsNullOrEmpty(Direccion) && !string.IsNullOrEmpty(Telefono) && !string.IsNullOrEmpty(Email);
        }

        private async void Guardar(object obj)
        {
            bool esNuevo = Cliente.id == 0;

            if (esNuevo) // Nuevo cliente
            {
                var nuevoCliente = new Cliente()
                {
                    apellidoNombre = ApellidoNombre,
                    cuitDni = CuitDni,
                    direccion = Direccion,
                    telefono = Telefono,
                    email = Email,
                    Localidad = Localidad,
                    CodigoPostal = CodigoPostal,
                    Provincia = Provincia
                };

                await clienteService.AddAsync(nuevoCliente);
            }
            else // Editar cliente existente
            {
                Cliente.apellidoNombre = ApellidoNombre;
                Cliente.cuitDni = CuitDni;
                Cliente.direccion = Direccion;
                Cliente.telefono = Telefono;
                Cliente.email = Email;
                Cliente.Localidad = Localidad;
                Cliente.CodigoPostal = CodigoPostal;
                Cliente.Provincia = Provincia;

                await clienteService.UpdateAsync(Cliente);
            }

            // Mostrar mensaje de éxito
            string mensaje = esNuevo ? "Cliente agregado correctamente" : "Cliente actualizado correctamente";
            await Application.Current.MainPage.DisplayAlert("Éxito", mensaje, "OK");

            // Enviar mensaje para refrescar la lista en ClientesViewModel
            WeakReferenceMessenger.Default.Send(new MyMessage("RefrescarListaClientes"));

            // Limpiar el formulario después de guardar
            LimpiarFormulario();

            // Navegar de vuelta a la lista de clientes
            await Shell.Current.GoToAsync("//ListaClientes");
        }

        private void LimpiarFormulario()
        {
            Cliente = new Cliente();
            ApellidoNombre = string.Empty;
            CuitDni = string.Empty;
            Direccion = string.Empty;
            Telefono = string.Empty;
            Email = string.Empty;
            Localidad = string.Empty;
            CodigoPostal = string.Empty;
            Provincia = string.Empty;
        }
    }
    
}
