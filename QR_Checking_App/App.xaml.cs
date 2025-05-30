using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QR_Checking_App
{
    public partial class App : Application
    {
        private static AuthPage AuthPage;
        public App()
        {
            InitializeComponent();
            AuthPage = new AuthPage();
            MainPage = AuthPage;
        }

        protected async override void OnStart()
        {
            await QueryConnection.OpenConnection(AuthPage);

            var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (cameraStatus != PermissionStatus.Granted)
            {
                cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (cameraStatus != PermissionStatus.Granted)
            {
                await MainPage.DisplayAlert("Ошибка", "Для работы сканера необходимо в настройках выдать приложению доступ к камере", "OK");
            }

            if (!Application.Current.Properties.ContainsKey("UUID"))
            {
                Guid uuid = Guid.NewGuid();
                Application.Current.Properties.Add("UUID", uuid.ToString());
            }
        }

        protected override void OnSleep()
        {
            Task.Run(() => QueryConnection.CloseConnection());
        }

        protected override void OnResume()
        {
            Task.Run(() => QueryConnection.OpenConnection(AuthPage));
            MainPage = AuthPage;           
        }
    }
}
