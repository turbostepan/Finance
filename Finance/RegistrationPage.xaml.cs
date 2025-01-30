using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
  
    public partial class RegistrationPage : Page
    {
        private string connectionString = "Server=DESKTOP-KG0LFL3\\SQLEXPRESS;Database=FINANCE;Trusted_Connection=True;";
        public RegistrationPage()
        {
            InitializeComponent();
        }
        private bool IsUserExists(string login, string email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM [Users] WHERE Login = @Login OR Email = @Email";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Email", email);

                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при проверке уникальности пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return true; 
            }
        }

        private bool RegisterUser(Users user)
        {
            if (IsUserExists(user.Login, user.Email))
            {
                MessageBox.Show("Пользователь с таким логином или email уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
                
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO [Users] (Name, Login, Password, Email) VALUES (@Name, @Login, @Password, @Email)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@Login", user.Login);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@Email", user.Email);

                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string login = LoginTextBox2.Text;
            string email = Email.Text;
            string password = PasswordTextBox.Password;
            using (SHA256 hash = SHA256.Create())
            {
                byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                password = builder.ToString();
            }
            NavigationService.GoBack();



            Users newUser = new Users
            {
                Name = name,
                Login = login,
                Password = password,
                Email = email,


            };

            if (RegisterUser(newUser))
            {
                MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                RegistrationPage registrationPage = new RegistrationPage();
            }
            else
            {
                MessageBox.Show("Не удалось зарегистрировать пользователя. Попробуйте еще раз.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

  

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    




        private void BackButtonClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }




     
    }
}
//это нужно удалить