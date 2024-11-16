

using AppNegocio.ViewModels.Commons.ClienteVM;

namespace AppNegocio.Views.Commons.ClientesV;

public partial class ClientesView : ContentPage
{
	public ClientesView()
	{
        InitializeComponent();
        BindingContext = new ClientesViewModel();
    }
}