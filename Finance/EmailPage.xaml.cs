using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
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
    /// <summary>
    /// Логика взаимодействия для EmailPage.xaml
    /// </summary>
    public partial class EmailPage : Page
    {
        private string connectionString = "Server=DESKTOP-KG0LFL3\\SQLEXPRESS;Database=FINANCE;Trusted_Connection=True;";
        public EmailPage()
        {
            InitializeComponent();
        }

        private void Email_Button_Click(object sender, RoutedEventArgs e)
        {
            string code = RandomCode();
            string email = Email.Text;


            Users users = UserChek(email);
            if (users != null)
            {
                UpdateUserCode(email, code);
                MessageBox.Show(code, email, MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new CodePage());





            }
            else
            {
                MessageBox.Show("Пользователь с такой почтой еще не зарегистрирован.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }


        private void UpdateUserCode(string email, string code)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Users SET Code = @Code WHERE Email = @Email";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Code", code);
                        command.Parameters.AddWithValue("@Email", email);

                  
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Users UserChek(string email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Email FROM Users WHERE Email = @email";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Users
                                {
                                    Email = reader["Email"].ToString(),

                                };
                            }
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }
        private string RandomCode()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }



        private void BackButtonClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

    }
}