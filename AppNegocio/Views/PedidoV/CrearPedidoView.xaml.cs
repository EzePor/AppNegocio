using AppNegocio.ViewModels.PedidoVM;

namespace AppNegocio.Views.PedidoV;

public partial class CrearPedidoView : ContentPage
{
	public CrearPedidoView()
	{
		InitializeComponent();
	   BindingContext = new CrearPedidoViewModel();

    }
}