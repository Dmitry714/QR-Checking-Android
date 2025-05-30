using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QR_Checking_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountSettings : ContentPage
    {

        private static DataClass UserInfo;
        public AccountSettings(DataClass userInfo)
        {
            InitializeComponent();
            UserInfo = userInfo;
            _Email.Text = userInfo.Email;
            _Login.Text = userInfo.Login;
            _Password.Text = userInfo.Password;
        }

        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                saveButton.IsVisible = false;
                save_loadingIndicator.IsVisible = true;
                save_loadingIndicator.IsRunning = true;
                await Task.Delay(500);

                int id = UserInfo.ID_User;
                string email = _Email.Text;
                string login = _Login.Text;
                string password = _Password.Text;

                MySqlQuery mySqlQuery = new MySqlQuery();
                bool result = await mySqlQuery.UpdateAccount(id, email, login, password);
                if (result)
                {
                    UserInfo.Email = email;
                    UserInfo.Login = login;
                    UserInfo.Password = password;
                    await DisplayAlert("Данные сохранены", "Ваши данные были успешно сохранены", "Ок");
                }
                else
                {
                    await DisplayAlert("Ошибка", "Возникла ошибка. Ваши данные не сохранены", "Ок");
                }

            }
            catch (Exception)
            {
                await DisplayAlert("Ошибка подключения", "Ошибка подключения к серверу. Проверьте интернет соединение", "Ок");
            }
            finally
            {
                saveButton.IsVisible = true;
                save_loadingIndicator.IsVisible = false;
                save_loadingIndicator.IsRunning = false;
            }

        }

        private void breakeConn_Clicked(object sender, EventArgs e)
        {
            Task.Run(() => QueryConnection.CloseConnection());
        }
    }
}