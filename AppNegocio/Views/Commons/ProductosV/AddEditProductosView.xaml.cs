using AppNegocio.Models.Commons;
using AppNegocio.ViewModels.Commons.ProdutosVM;

namespace AppNegocio.Views.Commons.ProductosV;

[QueryProperty(nameof(ProductoToEdit), "ProductoAEditar")]
public partial class AddEditProductosView : ContentPage
{
    public Producto ProductoToEdit
    {
        set
        {
            var producto = value;
            var viewmodel = this.BindingContext as AddEditProductoViewModel;
            viewmodel.Producto = producto;
        }
    }
    AddEditProductoViewModel viewModel;
    public AddEditProductosView()
	{
		InitializeComponent();
	}
}