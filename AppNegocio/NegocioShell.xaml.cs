using AppNegocio.ViewModels.IniciarSesionVM;
using AppNegocio.Views;

namespace AppNegocio
{
    public partial class NegocioShell : Shell
    {
        public NegocioShell()
        {
            InitializeComponent();
            FlyoutItemsPrincipal.IsVisible = false; // Oculta el menú lateral
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute("Registrarse", typeof(RegistrarseView));
        }

        public void EnableAppAfterLogin()
        {
            FlyoutBehavior = FlyoutBehavior.Flyout; // Habilita el FlyOut
            FlyoutItemsPrincipal.IsVisible = true; // Muestra el menú lateral
            Shell.Current.GoToAsync("//MainPage"); // Navega a la página principal
            var viewmodel = this.BindingContext as NegocioShellViewModel;
            viewmodel.UserIsLogout = false;
        }
        public void DisableAppAfterLogin()
        {
            FlyoutItemsPrincipal.IsVisible = false; // Oculta el menú lateral
            FlyoutBehavior = FlyoutBehavior.Disabled; // Deshabilita el FlyOut
            Shell.Current.GoToAsync("//Login"); // Navega a la página de login  
        }
    }
}

