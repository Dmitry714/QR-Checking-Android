using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Mobile;
using ZXing;
using ZXing.Net.Mobile.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using System.Globalization;
using Xamarin.Essentials;

namespace QR_Checking_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QR_SCANNER : ContentPage
    {
        private static DataClass DataClass;
        public QR_SCANNER(DataClass dataClass)
        {
            InitializeComponent();
            DataClass = dataClass;
            scannerView.IsScanning = true;
        }

        public void OnScanResult(Result result_scan)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (result_scan != null)
                {
                    var current = Connectivity.NetworkAccess;
                    if (current == Xamarin.Essentials.NetworkAccess.Internet)
                    {
                        string scannedValue = result_scan.Text;
                        scannerView.IsScanning = false;
                        MainGrid.IsVisible = false;
                        LoadGrid.IsVisible = true;
                        await ExecuteDatabaseQuery(scannedValue);
                    }
                    else
                    {
                        scannerView.IsScanning = false;
                        MainGrid.IsVisible = false;
                        LoadGrid.IsVisible = true;
                        await DisplayAlert("Ошибка подключения", "Ошибка подключения к сети. Проверьте интернет соединение", "Ок");
                        await Navigation.PopAsync();
                    }                    
                }
            });
        }

        private async Task ExecuteDatabaseQuery(string scannedValue)
        {
            try
            {
                MySqlQuery mySqlQuery = new MySqlQuery();

                int idEvent = await mySqlQuery.QrCodeScan(scannedValue);
                int checkAttendance = await mySqlQuery.CheckAttendanceEvent(DataClass.ID_User);
                int? idGroup = await mySqlQuery.CheckGroupEvent();

                if (idEvent != 0)
                {
                    if (idEvent != checkAttendance)
                    {
                        if (idGroup == DataClass.ID_Group || idGroup == null)
                        {
                            string attendanceStatus = "";
                            DateTime attendanceDate = new DateTime();

                            //Проверка даты
                            using (var client = new HttpClient())
                            {
                                var response = await client.GetAsync("https://timeapi.io/api/Time/current/zone?timeZone=Europe/Minsk");
                                response.EnsureSuccessStatusCode();
                                var getData = await response.Content.ReadAsStringAsync();
                                dynamic jsonObject = JsonConvert.DeserializeObject(getData);

                                string dateTimeString = jsonObject.dateTime;

                                DateTimeOffset dateTime;
                                if (DateTimeOffset.TryParseExact(dateTimeString, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                                {
                                    DateTimeOffset localDateTime = dateTime.DateTime;

                                    DateTime beginDateTime = await mySqlQuery.CheckEvent(idEvent);

                                    string attendanceString = localDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                                    attendanceDate = DateTime.ParseExact(attendanceString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);


                                    if (localDateTime == beginDateTime || localDateTime < beginDateTime)
                                    {
                                        attendanceStatus = "Пришел во время";
                                    }
                                    else if (localDateTime > beginDateTime)
                                    {
                                        TimeSpan timeDifference = localDateTime - beginDateTime;
                                        attendanceStatus = $"Опоздал на {timeDifference}";
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Ошибка", "Не удалось получить дату события", "ОК");
                                }
                            }

                            bool result = await mySqlQuery.InsertIntoAttendance(Convert.ToInt32(DataClass.ID_User), idEvent, attendanceDate, attendanceStatus);
                            if (result)
                            {
                                await DisplayAlert("Выполнено", "Ваши данные были записаны!", "Ок");
                            }
                            else
                            {
                                await DisplayAlert("Ошибка", "Данные о посещении не удалось записать", "Ок");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Ошибка", "Вы не можете отметиться на событии для другой группы", "Ок");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Ошибка", "Вы уже отмечались на этом событии", "Ок");
                    }
                }
                else
                {
                    await DisplayAlert("Ошибка", "QR-код не распознан!", "Ок");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Упс!", "Что-то пошло не так" + ex, "Ок");
            }
            finally
            {
                await Navigation.PopAsync();
            }            
        }        

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();             
        }
    }
}