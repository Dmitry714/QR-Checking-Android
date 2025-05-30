using System;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MySqlConnector;
using Xamarin.Forms;

namespace QR_Checking_App
{
    public class MySqlQuery
    {
        private readonly DataClass _dataClass;

        public MySqlQuery()
        {
            _dataClass = new DataClass();
        }

        public async Task<string> GetTelegramSupportId()
        {
            try
            {
                string query = "SELECT Telegram_ID FROM App_config LIMIT 1";
                MySqlCommand command = new MySqlCommand(query, QueryConnection.connection);
                MySqlDataReader reader = await command.ExecuteReaderAsync();
                string tgid = null;
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        tgid = reader.GetString("Telegram_ID");
                    }
                    reader.Close();
                    return tgid;
                }
                else
                {
                    reader.Close();
                    return null;
                }
            }
            catch (Exception)
            {
                ExceptionAnswer();
            }
            return null;
        }

        public async Task<bool> SelectUsersLogin(string Login)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Users_Stud WHERE Binary Login = @Login";
                MySqlCommand command = new MySqlCommand(query, QueryConnection.connection);
                command.Parameters.AddWithValue("@Login", Login);

                int count = Convert.ToInt32(await command.ExecuteScalarAsync());

                return count == 0;
            }
            catch (Exception)
            {
                ExceptionAnswer();
            }
            return false;
        }

        public async Task<bool> SelectUsersEmail(string Email)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Users_Stud WHERE Binary Email = @Email";
                MySqlCommand command = new MySqlCommand(query, QueryConnection.connection);
                command.Parameters.AddWithValue("@Email", Email);

                int count = Convert.ToInt32(await command.ExecuteScalarAsync());

                return count == 0;
            }
            catch (Exception)
            {
                ExceptionAnswer();
            }
            return false;
        }

        public async Task<bool> InsertIntoAttendance(int ID_User, int ID_Event, DateTime Attendance_Date, string Attendance_Status)
        {
            try
            {
                string query = "insert into Attendance (ID_User_Stud, ID_Event, Attendance_Date, Attendance_Status) values (@ID_User_Stud, @ID_Event, @Attendance_Date, @Attendance_Status)";
                MySqlCommand command = new MySqlCommand(query, QueryConnection.connection);
                command.Parameters.AddWithValue("@ID_User_Stud", ID_User);
                command.Parameters.AddWithValue("@ID_Event", ID_Event);
                command.Parameters.AddWithValue("@Attendance_Date", Attendance_Date);
                command.Parameters.AddWithValue("@Attendance_Status", Attendance_Status);
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception)
            {
                ExceptionAnswer();
            }
            return false;
        }

        public async Task<bool> UpdateUserLogin(int ID_User, string Login)
        {
            try
            {
                string query = "UPDATE Users_Stud SET Login = @Login WHERE ID_User_Stud = @ID_User_Stud";
                MySqlCommand command = new MySqlCommand(query, QueryConnection.connection);
                command.Parameters.AddWithValue("@Login", Login);
                command.Parameters.AddWithValue("@ID_User_Stud", ID_User);
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception)
            {
                ExceptionAnswer();
            }
            return false;
        }

        public async Task<bool> UpdateUserPassword(int ID_User, string Password)
        {
            try
            {
                string query = "UPDATE Users_Stud SET Password = @Password WHERE ID_User_Stud = @ID_User_Stud";
                MySqlCommand command = new MySqlCommand(query, QueryConnection.connection);
                command.Parameters.AddWithValue("@Password", Password);
                command.Parameters.AddWithValue("@ID_User_Stud", ID_User);
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception)
            {
                ExceptionAnswer();
            }
            return false;
        }

        public async Task<bool> UpdateUserEmail(int ID_User, string Email)
        {
            try
            {
                string query = "UPDATE Users_Stud SET Email = @Email WHERE ID_User_Stud = @ID_User_Stud";
                MySqlCommand command = new MySqlCommand(query, QueryConnection.connection);
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@ID_User_Stud", ID_User);
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception)
            {
                ExceptionAnswer();
            }
            return false;
        }

        public async Task<int> QrCodeScan(string QR_Code_info)
        {
            try
            {
                string query = "select * From QR_Codes where QR_Code_info = @QR_Code_info";
                MySqlCommand command = new MySqlCommand(query, QueryConnection.connection);
                command.Parameters.AddWithValue("@QR_Code_info", QR_Code_info);
                MySqlDataReader reader = await command.ExecuteReaderAsync();
                int ID_Event = 0;
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        ID_Event = reader.GetInt32("ID_Event");
                    }
                    reader.Close();
                    return ID_Event;
                }
                else
                {
                    reader.Close();
                    return 0;
                }
            }
            catch (Exception)
            {
                ExceptionAnswer();                
            }
            return 0;
        }

        public async Task<int> CheckAttendanceEvent(int ID_User)
        {
            try
            {
                string query = $"select ID_Event From Attendance where ID_User_Stud = @ID_User_Stud";
                MySqlCommand command = new MySqlCommand(query, QueryConnection.connection);
                command.Parameters.AddWithValue("@ID_User_Stud", ID_User);
                MySqlDataReader reader = await command.ExecuteReaderAsync();
                int idEvent = 0;
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        idEvent = reader.GetInt32("ID_Event");
                    }
                    reader.Close();
                }
                reader.Close();
                return idEvent;
            }
            catch (Exception)
            {
                ExceptionAnswer();
            }
            return 0;            
        }

        public async Task<DateTime> CheckEvent(int ID_Event)
        {
            try
            {
                string query = $"select Event_Begin From Events where ID_Event = @ID_Event";
                MySqlCommand command = new MySqlCommand(query, QueryConnection.connection);
                command.Parameters.AddWithValue("@ID_Event", ID_Event);
                MySqlDataReader reader = await command.ExecuteReaderAsync();

                DateTime dateTime = new DateTime();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        dateTime = reader.GetDateTime("Event_Begin");
                    }
                    reader.Close();
                }
                reader.Close();
                return dateTime;
            }
            catch (Exception)
            {
                ExceptionAnswer();
            }
            return default;
        }
        
        public async Task<int?> CheckGroupEvent()
        {
            try
            {
                string query = $"select ID_Group From Events where IsActive = 'Да'";
                MySqlCommand command = new MySqlCommand(query, QueryConnection.connection);
                MySqlDataReader reader = await command.ExecuteReaderAsync();
                int? group = null;
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("ID_Group")))
                        {
                            group = reader.GetInt32("ID_Group");
                        }
                    }
                    reader.Close();
                }
                reader.Close();
                return group;
            }
            catch (Exception)
            {
                ExceptionAnswer();
            }
            return 0;
            
        }

        public async Task<string> CheckIP()
        {
            try
            {
                string IP = "";
                string query = $"select IP From App_config LIMIT 1";
                MySqlCommand command = new MySqlCommand(query, QueryConnection.connection);
                MySqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        IP = reader.GetString("IP");
                    }
                    reader.Close();
                    return IP;
                }
                else
                {
                    reader.Close();
                    return null;
                }
            }
            catch (Exception)
            {
                ExceptionAnswer();
            }
            return null;
        }

        public async Task<DataClass> SelectFromUsers(string login, string password)
        {
            try
            {
                string query = "SELECT u.*, g.Group_Number FROM Users_Stud u JOIN _Groups g ON u.ID_Group = g.ID_Group WHERE BINARY u.Login = @login AND BINARY u.Password = @password LIMIT 1";
                MySqlCommand command = new MySqlCommand(query, QueryConnection.connection);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);
                MySqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        _dataClass.ID_User = reader.GetInt32("ID_User_Stud");
                        _dataClass.Name = reader.GetString("Name");
                        _dataClass.Surname = reader.GetString("Surname");
                        _dataClass.Patronymic = reader.GetString("Patronymic");
                        _dataClass.Phone_Model = reader.GetString("Phone_Model");
                        _dataClass.Phone_Number = reader.GetString("Phone_Number");
                        _dataClass.App_ID = reader.IsDBNull(reader.GetOrdinal("App_ID")) ? string.Empty : reader.GetString("App_ID");
                        _dataClass.Email = reader.GetString("Email");
                        _dataClass.Login = reader.GetString("Login");
                        _dataClass.Password = reader.GetString("Password");
                        _dataClass.ID_Group = reader.GetInt32("ID_Group");
                        _dataClass.Enable = reader.GetString("Enable");
                        _dataClass.Group_Number = reader.GetString("Group_Number");
                    }

                    reader.Close();

                    if (string.IsNullOrWhiteSpace(_dataClass.App_ID))
                    {
                        string uuid = Application.Current.Properties["UUID"].ToString();
                        string updateQuery = "UPDATE Users_Stud SET App_ID = @app_id WHERE ID_User_Stud = @id_user";
                        MySqlCommand updateCommand = new MySqlCommand(updateQuery, QueryConnection.connection);
                        updateCommand.Parameters.AddWithValue("@app_id", uuid.ToString());
                        updateCommand.Parameters.AddWithValue("@id_user", _dataClass.ID_User);
                        await updateCommand.ExecuteNonQueryAsync();
                        _dataClass.App_ID = uuid;
                    }

                    return _dataClass;
                }
                else
                {
                    reader.Close();
                    return null;
                }
            }
            catch (Exception)
            {
                ExceptionAnswer();
            }
            return null;
                     
        }

        public async Task<bool> UpdateAccount(int ID_User, string email, string login, string password)
        {
            try
            {
                string query = $"update Users_Stud set Email = @Email, Login = @Login, Password = @Password where ID_User_Stud = {ID_User}";
                MySqlCommand command = new MySqlCommand(query, QueryConnection.connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception)
            {
                ExceptionAnswer();
            }
            return false;
        }

        public async Task<bool> CheckEmail(string email)
        {
            try
            {
                string query = $"select * from Users_Stud where BINARY Email = @Email";
                MySqlCommand mySqlCommand = new MySqlCommand(query, QueryConnection.connection);
                mySqlCommand.Parameters.AddWithValue("@Email", email);
                MySqlDataReader reader = await mySqlCommand.ExecuteReaderAsync();

                int ID_User = 0;

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        ID_User = reader.GetInt32("ID_User_Stud");
                    }
                    reader.Close();

                    string newlogin = GetLogin(8);
                    string newpassword = GetPassword(8);
                    string hashpass = SHA256Converter.ConvertToSHA256(newpassword);

                    string resetPasswordQuery = $"Update Users_Stud set Password = @Password, Login = @Login where ID_User_Stud = {ID_User}";
                    MySqlCommand mySqlCommand1 = new MySqlCommand(resetPasswordQuery, QueryConnection.connection);
                    mySqlCommand1.Parameters.AddWithValue("@Login", newlogin);
                    mySqlCommand1.Parameters.AddWithValue("@Password", hashpass);
                    mySqlCommand1.ExecuteNonQuery();

                    string subject = "Восстановление аккаунта";
                    string body = $"Ваши новые данные от аккаунта: Логин: {newlogin}, Пароль: {newpassword}";
                    await MailSender.SendEmail(subject, body, email);

                    reader.Close();
                    return true;
                }
                else
                {
                    reader.Close();
                    return false;
                }
            }
            catch (Exception)
            {
                ExceptionAnswer();
            }
            return false;
            
        }

        public static string GetLogin(int length)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();
            for (int i = 0; i < length; i++)
            {
                int index = rnd.Next(chars.Length);
                sb.Append(chars[index]);
            }
            return sb.ToString();
        }

        public static string GetPassword(int length)
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%";
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();
            for (int i = 0; i < length; i++)
            {
                int index = rnd.Next(chars.Length);
                sb.Append(chars[index]);
            }
            return sb.ToString();
        }

        private async void ExceptionAnswer()
        {
            bool checkConn = await Task.Run(() => QueryConnection.IsConnectionOpen());
            if (!checkConn)
            {
                AuthPage authPage = null;
                authPage = new AuthPage();
                await Task.Run(() => QueryConnection.OpenConnection(authPage));
                Application.Current.MainPage = authPage;
                await authPage.DisplayAlert("Отключено", "Подключение с сервером было разорвано", "Ок");
            }
        }
    }
}
