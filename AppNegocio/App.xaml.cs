namespace AppNegocio
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NegocioShell();
        }
    }
}
