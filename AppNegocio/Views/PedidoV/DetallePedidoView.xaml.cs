using AppNegocio.ViewModels.PedidoVM;
using System.Security.Cryptography.X509Certificates;

namespace AppNegocio.Views.PedidoV;

public partial class DetallePedidoView : ContentPage
{
	public DetallePedidoView()
	{
		InitializeComponent();
        BindingContext = new DetallePedidoViewModel();

	
    }
}