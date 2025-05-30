using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QR_Checking_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserInfo : ContentPage
    {
        private static DataClass DataClass;
        public UserInfo(DataClass dataClass)
        {
            InitializeComponent();
            DataClass = dataClass;
            FIO.Text = $"{dataClass.Name} { dataClass.Surname} {dataClass.Patronymic}";
            Login.Text = dataClass.Login;
            Email.Text = dataClass.Email;
            Phone_Number.Text = dataClass.Phone_Number;
            Phone_Model.Text = dataClass.Phone_Model;
            Group_Number.Text = dataClass.Group_Number;

        }
    }
}