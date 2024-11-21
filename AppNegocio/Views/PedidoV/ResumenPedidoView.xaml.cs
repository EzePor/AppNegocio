using AppNegocio.ViewModels.PedidoVM;

namespace AppNegocio.Views.PedidoV;

public partial class ResumenPedidoView : ContentPage
{
	public ResumenPedidoView()
	{
		InitializeComponent();
	}
    protected async override void OnAppearing()
    {
        var viewModel = (ResumenPedidoViewModel)BindingContext;
        await viewModel.CargarResumen();
    }
}