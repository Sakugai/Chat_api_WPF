using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kirillov_chat_api
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public HttpClient httpClient = new HttpClient();
        public static Employee employee;
        
        public MainWindow()
        {
            InitializeComponent();
            LoginTb.Text=Properties.Settings.Default.Login;
            PasswordPb.Password = Properties.Settings.Default.Password;
        }
        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new userData { username = LoginTb.Text, password = PasswordPb.Password.ToString() };
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(content),
                Encoding.UTF8, "application/json");
            HttpResponseMessage message = await httpClient.PostAsync("http://localhost:61677/api/Login", httpContent);
            if (message.IsSuccessStatusCode)
            {
                var curUser = await message.Content.ReadAsStringAsync();
                employee = JsonConvert.DeserializeObject<Employee>(curUser);
                if((bool)RememberCb.IsChecked)
                {
                    Properties.Settings.Default.Login = LoginTb.Text;
                    Properties.Settings.Default.Password = PasswordPb.Password.ToString();
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.Login = string.Empty;
                    Properties.Settings.Default.Password = string.Empty;
                    Properties.Settings.Default.Save();
                }
                MessageBox.Show("Вы авторизовались!");
                ChatSelection chatSelection = new ChatSelection();       
                chatSelection.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }
        public class userData
        {
            public string username { get; set; } 
            public string password { get; set; } 
        }
    }
}
