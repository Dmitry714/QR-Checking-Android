using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QR_Checking_App
{
    class QueryConnection
    {
        static string connectionString = "server=db4free.net;port=3306;user=dimedroll;password=dimasik123;database=qr_checking_db;";
        private static MySqlConnection sqlConnection = null;
        private static bool isConnectionOpen = false;

        public static MySqlConnection connection
        {
            get
            {
                if (sqlConnection == null)
                {
                    sqlConnection = new MySqlConnection(connectionString);
                    return sqlConnection;
                }
                return sqlConnection;
            }
        }
        public static async Task OpenConnection(AuthPage authPage)
        {
            if (connection == null || connection.State != ConnectionState.Open)
            {
                while (!isConnectionOpen)
                {
                    try
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            authPage.CheckConn();
                        });

                        if (connection.State != ConnectionState.Connecting)
                        {
                            await Task.Run(() => connection.Open());
                            isConnectionOpen = true;
                        }                                                

                        if (isConnectionOpen)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                authPage.ConnSuccess();
                            });
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(5));
                        }
                    }
                    catch (Exception)
                    {
                        await CloseConnection();

                        await Task.Delay(TimeSpan.FromSeconds(1));
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            authPage.ConnFailed();
                        });
                        break;
                    }
                }
            }            
        }

        public static async Task CloseConnection()
        {
            await Task.Run(() => connection.Close());
            isConnectionOpen = false;
        }

        public static bool IsConnectionOpen()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                isConnectionOpen = true;
                return true;
            }
            else
            {
                isConnectionOpen = false;
                return false;
            }
        }
    }

}
