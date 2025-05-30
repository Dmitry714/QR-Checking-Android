using System;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QR_Checking_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private static DataClass DataClass;
        private static MySqlQuery MySqlQuery;
        private string emailCode;
        private string email;
        public MainPage(DataClass dataClass, MySqlQuery mySqlQuery)
        {
            InitializeComponent();
            DataClass = dataClass;
            MySqlQuery = mySqlQuery;
            greetings.Text = "Здравствуйте, " + dataClass.Name;
            LoginEntry.Text = dataClass.Login;
            EmailEntry.Text = dataClass.Email;

            frame_account.IsVisible = false;
            frame_changeLogin.IsVisible = false;
            frame_changePassword.IsVisible = false;
            frame_changeEmail.IsVisible = false;
            frame_acceptCode.IsVisible = false;

            frame_account.Opacity = 0;
            frame_changeLogin.Opacity = 0;
            frame_changePassword.Opacity = 0;
            frame_changeEmail.Opacity = 0;
            frame_acceptCode.Opacity = 0;

            frame_account.PropertyChanged += AccountSettings_PropertyChanged;
            frame_changeLogin.PropertyChanged += ChangeLogin_PropertyChanged;
            frame_changePassword.PropertyChanged += ChangePassword_PropertyChanged;
            frame_changeEmail.PropertyChanged += ChangeEmail_PropertyChanged;
            frame_acceptCode.PropertyChanged += AcceptCodeFrame_PropertyChanged;
        }

        private async void AcceptCodeFrame_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Opacity" && ((Frame)sender).Opacity == 1)
            {
                await frame_acceptCode.FadeTo(1, 300);
            }
        }

        private async void AccountSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Opacity" && ((Frame)sender).Opacity == 1)
            {
                await frame_account.FadeTo(1, 300);
            }
        }

        private async void ChangeLogin_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Opacity" && ((Frame)sender).Opacity == 1)
            {
                await frame_changeLogin.FadeTo(1, 300);
            }
        }

        private async void ChangePassword_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Opacity" && ((Frame)sender).Opacity == 1)
            {
                await frame_changePassword.FadeTo(1, 300);
            }
        }

        private async void ChangeEmail_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Opacity" && ((Frame)sender).Opacity == 1)
            {
                await frame_changeEmail.FadeTo(1, 300);
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            ShowLoading();

            QR_SCANNER scanner = new QR_SCANNER(DataClass);
            NavigationPage.SetHasNavigationBar(scanner, false);
            await Navigation.PushAsync(scanner);

            HideLoading();
        }

        private void ShowLoading()
        {
            QRbutton.Text = "Загрузка...";
            frame_controls.IsEnabled = false;
        }

        private void HideLoading()
        {
            QRbutton.Text = "Сканировать QR-код";
            frame_controls.IsEnabled = true;
        }

        private async void AccSettbutton_Clicked(object sender, EventArgs e)
        {           
            await frame_controls.FadeTo(0, 300);
            frame_controls.IsVisible = false;
            frame_account.IsVisible = true;
            await frame_account.FadeTo(1, 300);
        }

        private async void TechSupportbutton_Clicked(object sender, EventArgs e)
        {                        
            Uri uri = null;
            try
            {
                TechSupportbutton.Text = "Загрузка..."; 
                string telegramUsername = await MySqlQuery.GetTelegramSupportId();
                uri = new Uri($"tg://resolve?domain={telegramUsername}");
                await Launcher.OpenAsync(uri);
            }
            catch (Exception)
            {
                uri = new Uri("https://telegram.org/dl");
                await Launcher.OpenAsync(uri);
            }
            finally
            {
                TechSupportbutton.Text = "Тех.поддержка";
            }
        }

        private async void back_label_Tapped(object sender, EventArgs e)
        {
            await ((Label)sender).ScaleTo(0.95, 100, Easing.SinIn);
            await ((Label)sender).ScaleTo(1, 100, Easing.SinOut);
            await frame_account.FadeTo(0, 300);
            frame_controls.IsVisible = true;
            frame_account.IsVisible = false;
            await frame_controls.FadeTo(1, 300);
        }

        private async void LoginChangeButton_Clicked(object sender, EventArgs e)
        {
            await frame_account.FadeTo(0, 300);
            frame_account.IsVisible = false;
            frame_changeLogin.IsVisible = true;
            await frame_changeLogin.FadeTo(1, 300);
        }

        private async void backToAccountFrameFLF_label_Tapped(object sender, EventArgs e)
        {
            await ((Label)sender).ScaleTo(0.95, 100, Easing.SinIn);
            await ((Label)sender).ScaleTo(1, 100, Easing.SinOut);
            await frame_changeLogin.FadeTo(0, 300);
            frame_account.IsVisible = true;
            frame_changeLogin.IsVisible = false;
            await frame_account.FadeTo(1, 300);
        }

        private async void backToAccountFrameFPF_label_Tapped(object sender, EventArgs e)
        {
            await ((Label)sender).ScaleTo(0.95, 100, Easing.SinIn);
            await ((Label)sender).ScaleTo(1, 100, Easing.SinOut);
            await frame_changePassword.FadeTo(0, 300);
            frame_account.IsVisible = true;
            frame_changePassword.IsVisible = false;
            await frame_account.FadeTo(1, 300);
        }

        private async void PassChangeButton_Clicked(object sender, EventArgs e)
        {
            await frame_account.FadeTo(0, 300);
            frame_account.IsVisible = false;
            frame_changePassword.IsVisible = true;
            await frame_changePassword.FadeTo(1, 300);
        }

        private async void backToAccountFrameFEF_label_Tapped(object sender, EventArgs e)
        {
            await ((Label)sender).ScaleTo(0.95, 100, Easing.SinIn);
            await ((Label)sender).ScaleTo(1, 100, Easing.SinOut);
            await frame_changeEmail.FadeTo(0, 300);
            frame_account.IsVisible = true;
            frame_changeEmail.IsVisible = false;
            await frame_account.FadeTo(1, 300);
        }

        private async void EmailChangeButton_Clicked(object sender, EventArgs e)
        {
            await frame_account.FadeTo(0, 300);
            frame_account.IsVisible = false;
            frame_changeEmail.IsVisible = true;
            await frame_changeEmail.FadeTo(1, 300);
        }

        private async void LoginChangeAcceptButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                frame_changeLogin.IsEnabled = false;
                LoginChangeAcceptButton.Text = "Загрузка...";


                var current = Connectivity.NetworkAccess;
                if (current == Xamarin.Essentials.NetworkAccess.Internet)
                {
                    if (!string.IsNullOrWhiteSpace(LoginEntry.Text) || !string.IsNullOrWhiteSpace(AcceptPassEntryFL.Text))
                    {
                        string pass = SHA256Converter.ConvertToSHA256(AcceptPassEntryFL.Text);
                        bool checkLogin = await MySqlQuery.SelectUsersLogin(LoginEntry.Text);
                        if (LoginEntry.Text.Length > 8)
                        {
                            if (pass == DataClass.Password)
                            {
                                if (checkLogin)
                                {
                                    bool result = await MySqlQuery.UpdateUserLogin(DataClass.ID_User, LoginEntry.Text);
                                    if (result)
                                    {
                                        await DisplayAlert("Выполнено", "Логин успешно изменен", "Ok");
                                        DataClass.Login = LoginEntry.Text;
                                        AcceptPassEntryFL.Text = "";
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Ошибка", "Данный логин уже занят", "Ok");
                                }

                            }
                            else
                            {
                                await DisplayAlert("Ошибка", "Пароль не верный", "Ok");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Ошибка", "Логин должен быть больше 8 символов", "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Ошибка", "Поля не должны быть пустыми", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Ошибка подключения", "Ошибка подключения к серверу. Проверьте интернет соединение", "Ok");
                }                                    
            }
            catch (Exception)
            {
                await DisplayAlert("Упс!", "Что-то пошло не так", "Ok");
            }
            finally
            {
                frame_changeLogin.IsEnabled = true;
                LoginChangeAcceptButton.Text = "Сохранить";
            }
        }

        private async void PasswordChangeAcceptButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                frame_changePassword.IsEnabled = false;
                PasswordChangeAcceptButton.Text = "Загрузка...";

                var current = Connectivity.NetworkAccess;
                if (current == Xamarin.Essentials.NetworkAccess.Internet) 
                {
                    if (!string.IsNullOrWhiteSpace(OldPasswordEntry.Text) || !string.IsNullOrWhiteSpace(NewPasswordEntry.Text) || !string.IsNullOrWhiteSpace(RepeatNewPasswordEntry.Text))
                    {
                        if (NewPasswordEntry.Text.Length > 8)
                        {
                            string pass = SHA256Converter.ConvertToSHA256(OldPasswordEntry.Text);
                            if (pass == DataClass.Password)
                            {
                                if (NewPasswordEntry.Text == RepeatNewPasswordEntry.Text)
                                {
                                    string newpass = SHA256Converter.ConvertToSHA256(NewPasswordEntry.Text);
                                    bool result = await MySqlQuery.UpdateUserPassword(DataClass.ID_User, newpass);
                                    if (result)
                                    {
                                        await DisplayAlert("Выполнено", "Пароль успешно изменен", "Ok");
                                        DataClass.Password = newpass;
                                        OldPasswordEntry.Text = "";
                                        NewPasswordEntry.Text = "";
                                        RepeatNewPasswordEntry.Text = "";
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Ошибка", "Пароли не сходятся", "Ok");
                                }
                            }
                            else
                            {
                                await DisplayAlert("Ошибка", "Пароль не верный", "Ok");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Ошибка", "Пароль должен быть больше 8 символов", "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Ошибка", "Поля не должны быть пустыми", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Ошибка подключения", "Ошибка подключения к серверу. Проверьте интернет соединение", "Ok");
                }               
            }
            catch (Exception)
            {
                await DisplayAlert("Упс!", "Что-то пошло не так", "Ok");
            }
            finally
            {
                frame_changePassword.IsEnabled = true;
                PasswordChangeAcceptButton.Text = "Сохранить";
            }
        }

        private async void EmailChangeAcceptButton_Clicked(object sender, EventArgs e)
        {
            frame_changeEmail.IsEnabled = false;
            EmailChangeAcceptButton.Text = "Загрузка...";

            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == Xamarin.Essentials.NetworkAccess.Internet) 
                {
                    if (!string.IsNullOrWhiteSpace(EmailEntry.Text) || !string.IsNullOrWhiteSpace(AcceptPassEntryFE.Text))
                    {
                        if (IsEmailValid(EmailEntry.Text))
                        {
                            string pass = SHA256Converter.ConvertToSHA256(AcceptPassEntryFE.Text);
                            if (pass == DataClass.Password)
                            {
                                bool checkEmail = await MySqlQuery.SelectUsersEmail(EmailEntry.Text);
                                if (checkEmail)
                                {
                                    emailCode = RandomText(8);
                                    string subject = "Код подтверждения";
                                    string body = $"Код подтверждения для смены Email в приложении QR CHECKING: {emailCode}";
                                    bool sendMail = await MailSender.SendEmail(subject, body, EmailEntry.Text);
                                    if (sendMail)
                                    {
                                        AcceptPassEntryFE.Text = "";
                                        EmailEntry.Text = "";
                                        await frame_changeEmail.FadeTo(0, 300);
                                        frame_changeEmail.IsVisible = false;
                                        frame_acceptCode.IsVisible = true;
                                        await frame_acceptCode.FadeTo(1, 300);
                                    }
                                    else
                                    {
                                        await DisplayAlert("Ошибка", "Не удалось отправить сообщение", "Ok");
                                    }
                                }
                            }
                            else
                            {
                                await DisplayAlert("Ошибка", "Пароль не верный", "Ok");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Ошибка", "Некорректный формат Email", "Ok");
                        }
                        
                    }
                    else
                    {
                        await DisplayAlert("Ошибка", "Поля не должны быть пустыми", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Ошибка подключения", "Ошибка подключения к серверу. Проверьте интернет соединение", "Ok");
                }                
            }
            catch (Exception)
            {
                await DisplayAlert("Упс!", "Что-то пошло не так", "Ok");
            }
            finally
            {
                frame_changeEmail.IsEnabled = true;
                EmailChangeAcceptButton.Text = "Продолжить";
            }
        }

        private void LoginEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputText = e.NewTextValue;

            string sanitizedText = Regex.Replace(inputText, @"[^0-9a-zA-Z:.-]", "");

            if (sanitizedText != inputText)
            {
                ((Entry)sender).Text = sanitizedText;
            }
        }

        private void PasswordEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputText = e.NewTextValue;

            string sanitizedText = Regex.Replace(inputText, @"[^a-zA-Z0-9_@#%+-]", "");

            if (sanitizedText != inputText)
            {
                ((Entry)sender).Text = sanitizedText;
            }
        }

        private async void AcceptCodeButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                frame_acceptCode.IsEnabled = false;
                if (CodeEntry.Text == emailCode)
                {
                    bool result = await MySqlQuery.UpdateUserEmail(DataClass.ID_User, email);
                    if (result)
                    {
                        await DisplayAlert("Выполнено", "Email успешно изменен", "Ok");
                        DataClass.Email = email;
                        CodeEntry.Text = "";
                        await frame_acceptCode.FadeTo(0, 300);
                        frame_account.IsVisible = true;
                        frame_acceptCode.IsVisible = false;
                        await frame_account.FadeTo(1, 300);
                    }
                }
                else
                {
                    await DisplayAlert("Ошибка", "Код не верный", "Ok");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Упс!", "Что-то пошло не так", "Ok");
            }
            finally
            {
                frame_acceptCode.IsEnabled = true;
            }
        }

        public static string RandomText(int length)
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();
            for (int i = 0; i < length; i++)
            {
                int index = rnd.Next(chars.Length);
                sb.Append(chars[index]);
            }
            return sb.ToString();
        }

        private async void backToEmailFrameFCF_label_Tapped(object sender, EventArgs e)
        {
            await((Label)sender).ScaleTo(0.95, 100, Easing.SinIn);
            await((Label)sender).ScaleTo(1, 100, Easing.SinOut);
            await frame_acceptCode.FadeTo(0, 300);
            frame_account.IsVisible = true;
            frame_acceptCode.IsVisible = false;
            await frame_account.FadeTo(1, 300);
        }

        private async void ProfileInfoButton_Clicked(object sender, EventArgs e)
        {
            UserInfo userInfo = null;
            userInfo = new UserInfo(DataClass);
            NavigationPage.SetHasNavigationBar(userInfo, false);
            NavigationPage navigationPage = null;
            navigationPage = new NavigationPage(userInfo);
            navigationPage.Title = "Мой аккаунт";
            await Navigation.PushAsync(navigationPage);
        }

        private bool IsEmailValid(string email)
        {
            string pattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
    }
}