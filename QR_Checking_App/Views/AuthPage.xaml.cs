using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Globalization;
using Xamarin.Essentials;

namespace QR_Checking_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPage : ContentPage
    {
        public AuthPage()
        {
            InitializeComponent();
            reset_account_frame.IsVisible = false;
            reset_account_frame.Opacity = 0;
            reset_account_frame.PropertyChanged += ResetPasswordFrame_PropertyChanged;
        }

        private async void ResetPasswordFrame_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Opacity" && ((Frame)sender).Opacity == 1)
            {
                await reset_account_frame.FadeTo(1, 300);
            }
        }


        private async void auth_button_Clicked(object sender, EventArgs e)
        {
            try
            {
                main_stack.IsEnabled = false;
                auth_button.IsVisible = false;
                auth_loadingIndicator.IsVisible = true;
                auth_loadingIndicator.IsRunning = true;
                await Task.Delay(500);

                var current = Connectivity.NetworkAccess;

                if (current == Xamarin.Essentials.NetworkAccess.Internet)
                {
                    MySqlQuery mySqlQuery = new MySqlQuery();

                    string IP = await mySqlQuery.CheckIP();

                    string publicIpAddress = await GetIP();

                    if (publicIpAddress == IP)
                    {
                        string pass = SHA256Converter.ConvertToSHA256(_Password.Text);
                        DataClass dataClass = await mySqlQuery.SelectFromUsers(_Login.Text, pass);

                        if (dataClass != null)
                        {
                            if (dataClass.App_ID != Application.Current.Properties["UUID"].ToString())
                            {
                                await DisplayAlert("Ошибка", "Невозможно войти в аккаунт с этого устройства", "Ок");
                            }
                            else
                            {
                                if (dataClass.Enable != "Нет")
                                {
                                    MainPage main = new MainPage(dataClass, mySqlQuery);
                                    NavigationPage navigationPage = new NavigationPage(main);
                                    NavigationPage.SetHasNavigationBar(main, false);
                                    Application.Current.MainPage = navigationPage;
                                }
                                else
                                {
                                    await DisplayAlert("Ошибка", "Ваш аккаунт не активен", "Ок");
                                }
                            }
                        }
                        else
                        {
                            await DisplayAlert("Ошибка", "Неверный логин или пароль!", "Ок");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Ошибка", "Подключитесь к WIFI сети колледжа", "Ок");
                    }
                }
                else
                {
                    await DisplayAlert("Ошибка подключения", "Ошибка подключения к сети. Проверьте интернет соединение", "Ок");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Ошибка подключения", $"Ошибка подключения к серверу. Проверьте интернет соединение", "Ок");
            }
            finally
            {
                main_stack.IsEnabled = true;
                auth_button.IsVisible = true;
                auth_loadingIndicator.IsVisible = false;
                auth_loadingIndicator.IsRunning = false;
            }
        }

        private async Task<string> GetIP()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync("https://api.ipify.org");
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        private async void back_label_Tapped(object sender, EventArgs e)
        {
            await ((Label)sender).ScaleTo(0.95, 100, Easing.SinIn);
            await ((Label)sender).ScaleTo(1, 100, Easing.SinOut);
            await reset_account_frame.FadeTo(0, 300);
            main_frame.IsVisible = true;
            reset_account_frame.IsVisible = false;
            await main_frame.FadeTo(1, 300);
        }

        private async void forget_password_label_Tapped(object sender, EventArgs e)
        {
            await ((Label)sender).ScaleTo(0.95, 100, Easing.SinIn);
            await ((Label)sender).ScaleTo(1, 100, Easing.SinOut);
            await main_frame.FadeTo(0, 300);
            main_frame.IsVisible = false;
            reset_account_frame.IsVisible = true;
            await reset_account_frame.FadeTo(1, 300);
        }

        public void CheckConn()
        {
            AuthLabel.IsVisible = false;
            _Login.IsVisible = false;
            _Password.IsVisible = false;
            fp_Label.IsVisible = false;

            main_stack.IsEnabled = false;
            auth_button.IsVisible = false;
            conn_check_indicator.IsVisible = true;
            conn_check_indicator.IsRunning = true;
        }

        public void ConnSuccess()
        {
            AuthLabel.IsVisible = true;
            _Login.IsVisible = true;
            _Password.IsVisible = true;
            fp_Label.IsVisible = true;

            main_stack.IsEnabled = true;
            auth_button.IsVisible = true;
            conn_check_indicator.IsVisible = false;
            conn_check_indicator.IsRunning = false;
        }

        public async void ConnFailed()
        {
            conn_check_indicator.IsVisible = false;
            conn_check_indicator.IsRunning = false;


            bool shouldClose = await DisplayAlert("Ошибка", "Не удалось установить соединение с сервером", "Закрыть", "Повторить");
            if (shouldClose)
            {
                main_frame.IsVisible = false;
                ConnError.IsVisible = true;
            }
            else
            {
                await QueryConnection.OpenConnection(this);
                conn_check_indicator.IsVisible = true;
                conn_check_indicator.IsRunning = true;
            }
        }

        private async void _SendPassword_Clicked(object sender, EventArgs e)
        {
            try
            {
                _SendPassword.IsVisible = false;
                sendpassword_loadingIndicator.IsVisible = true;
                sendpassword_loadingIndicator.IsRunning = true;
                await Task.Delay(500);

                var current = Connectivity.NetworkAccess;

                if (current == Xamarin.Essentials.NetworkAccess.Internet)
                {
                    string email = _Email.Text;

                    MySqlQuery mySqlQuery = new MySqlQuery();
                    bool result = await mySqlQuery.CheckEmail(email);
                    if (result)
                    {
                        await DisplayAlert("Восстановление аккаунта", "Сообщение отправлено. Проверьте ваш Email", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Ошибка", "Аккаунта с таким Email не существует", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Ошибка подключения", "Ошибка подключения к серверу. Проверьте интернет соединение", "Ok");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Упс!", "Что-то пошло не так!", "Ok");
            }
            finally
            {
                _SendPassword.IsVisible = true;
                sendpassword_loadingIndicator.IsVisible = false;
                sendpassword_loadingIndicator.IsRunning = false;
            }
        }
    }
}