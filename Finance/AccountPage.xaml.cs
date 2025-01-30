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

    public partial class AccountPage : Page
    {
        private string connectionString = "Server=DESKTOP-KG0LFL3\\SQLEXPRESS;Database=FINANCE;Trusted_Connection=True;";

        public AccountPage()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Name, Login, Email FROM Users WHERE Id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", GetCurrentUserId());

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        NameTextBlock.Text = reader["Name"].ToString();
                        LoginTextBlock.Text = reader["Login"].ToString();
                        BlockEmail1.Text = reader["Email"].ToString();
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных пользователя: " + ex.Message);
            }
        }

        private int GetCurrentUserId()
        {
            return CurrentUser.Id;
        }

        private string HashPassword(string password)
        {
            using (SHA256 hash = SHA256.Create())
            {
                byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private bool IsUserExists(string login, string email, int currentUserId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
            SELECT COUNT(*) 
            FROM Users 
            WHERE (Login = @Login OR Email = @Email) AND Id != @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Id", currentUserId);

                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при проверке данных пользователя: " + ex.Message);
                return false;
            }
        }

        private bool UpdateUserData(string name, string login, string email, string newPassword)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    if (IsUserExists(login, email, GetCurrentUserId()))
                    {
                        MessageBox.Show("Пользователь с таким логином или email уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                    string query = @"
            UPDATE Users 
            SET Name = @Name, 
                Login = @Login, 
                Email = @Email, 
                Password = @Password 
            WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", HashPassword(newPassword));
                        command.Parameters.AddWithValue("@Id", GetCurrentUserId());

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении данных пользователя: " + ex.Message);
                return false;
            }
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

        private void New_Button_Click(object sender, RoutedEventArgs e)
        {
            string newName = NameTextBox1.Text;
            string newLogin = LoginTextBox1.Text;
            string newEmail = Email1.Text;
            string newPassword = PasswordTextBox1.Text;

           
            if (string.IsNullOrEmpty(newName) || string.IsNullOrEmpty(newLogin) || string.IsNullOrEmpty(newEmail) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

 
            bool isUpdated = UpdateUserData(newName, newLogin, newEmail, newPassword);

            if (isUpdated)
            {
                MessageBox.Show("Данные успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadUserData(); 
            }
            else
            {
                MessageBox.Show("Не удалось обновить данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}