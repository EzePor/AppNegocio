using AppNegocio.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNegocio.ViewModels.IniciarSesionVM
{
    public class NegocioShellViewModel : NotificationObject
    {
        private bool userIsLogout = true;
        public bool UserIsLogout
        {
            get { return userIsLogout; }
            set
            {
                userIsLogout = value;
                OnPropertyChanged();
            }
        }
        public Command LogoutCommand { get; }

        public NegocioShellViewModel()
        {
            LogoutCommand = new Command(OnLogout);
        }

        private void OnLogout(object obj)
        {
            UserIsLogout = true;
            var NegocioShell = (NegocioShell)App.Current.MainPage;
            NegocioShell.DisableAppAfterLogin();
        }
    }
}
