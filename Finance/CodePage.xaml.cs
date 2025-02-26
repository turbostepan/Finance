using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
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


namespace Finance
{
    public partial class CodePage : Page
    {
        private string connectionString = "Server=510EC16;Database=FINANCE;Trusted_Connection=True;";


        public CodePage()
        {
            InitializeComponent();
        }

        private void Enter_Button_Click(object sender, RoutedEventArgs e)
        {
         
            string enteredCode = PasswordTextBox2.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Code = @Code";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Code", enteredCode);

                    int result = (int)command.ExecuteScalar();

                
                    if (result > 0)
                    {
                        //CurrentUser.Id = users.Id;

                        MessageBox.Show("Код верный! Вход выполнен.");

                        ManagerWindow managerWindow = new ManagerWindow();
                        managerWindow.Show();

                    }
                    else
                    {

                        MessageBox.Show("Неверный код. Попробуйте снова.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при подключении к базе данных: " + ex.Message);
                }
            }

        }
      
        private void ReturnButtonClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
          
            
                NavigationService.Navigate(new EmailPage());
            
        }

        private void RegistrationButtonClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
           
                NavigationService.Navigate(new RegistrationPage());
            
        }
    }
}