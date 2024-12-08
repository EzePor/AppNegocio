using AppNegocio.Models.Details;
using AppNegocio.ViewModels.PedidoVM;
using System.Diagnostics;

namespace AppNegocio.Views.PedidoV;

[QueryProperty(nameof(PedidoAEditar), "PedidoAEditar")]
public partial class EditarPedidoView : ContentPage
{
    public EditarPedidoView()
    {
        InitializeComponent();
        //BindingContext = new EditarPedidoViewModel();

    }




    private Pedido pedidoAEditar;
    public Pedido PedidoAEditar
    {
        get => pedidoAEditar;
        set
        {
            pedidoAEditar = value;
            if (pedidoAEditar != null)
            {
                Debug.WriteLine($"[EditarPedidoView] Pedido recibido: {pedidoAEditar.id}");
                (BindingContext as EditarPedidoViewModel).Pedido=pedidoAEditar;
                (BindingContext as EditarPedidoViewModel).CargarPedido();
            }
        }
    }
  
}