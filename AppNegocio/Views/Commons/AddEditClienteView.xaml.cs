
using AppNegocio.Interfaces;
using AppNegocio.Models.Commons;
using AppNegocio.ViewModels.Commons.ClientesViewModels;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace AppNegocio.Views.Commons;

[QueryProperty(nameof(ClienteToEdit), "ClienteAEditar")]
public partial class AddEditClienteView : ContentPage
{
    public Cliente ClienteToEdit
    {
        set
        {
            var cliente = value;
            var viewmodel = this.BindingContext as AddEditClienteViewModel;
            viewmodel.Cliente = cliente;
        }
    }
    AddEditClienteViewModel viewModel;
    public AddEditClienteView()
    {
        InitializeComponent();
        BindingContext = new AddEditClienteViewModel();
    }
}
