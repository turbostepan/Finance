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
    public partial class CodePage : Window
    {
        public CodePage()
        {
            InitializeComponent();
        }

        private void Enter_Button_Click(object sender, RoutedEventArgs e)
        {
            // Проверка введенного кода
            string enteredCode = PasswordTextBox2.Text;

            string connectionString = @"Data Source=localhost;Initial Catalog=FINANCE;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Code FROM Users WHERE Code = @code", connection);
                command.Parameters.AddWithValue("@code", enteredCode);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // Если код существует в базе данных
                if (reader.HasRows)
                {
                    // Перейти на главную страницу
                    ManagerWindow managerWindow = new ManagerWindow();
                    managerWindow.Show();
                    
                }
                else
                {
                    // Отобразить сообщение об ошибке
                    MessageBox.Show("Неверный код", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}