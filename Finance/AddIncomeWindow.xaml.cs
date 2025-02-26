using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Finance
{
    /// <summary>
    /// Логика взаимодействия для AddIncomeWindow.xaml
    /// </summary>
    public partial class AddIncomeWindow : Window
    {
        private string connectionString = "Server=510EC16;Database=FINANCE;Trusted_Connection=True;";

        public AddIncomeWindow()
        {
            InitializeComponent();
            LoadCategories();
        }
        public void LoadCategories()
        {
            CategoryComboBox.Items.Clear();
            string query = "SELECT Name FROM Categories";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CategoryComboBox.Items.Add(reader["Name"].ToString());
                        }
                    }
                }
            }
        }

        private int GetCategoryId(string categoryName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT Id FROM Categories WHERE Name = @Name";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", categoryName);
                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(AmountTextBox.Text, out decimal amount) && CategoryComboBox.SelectedItem != null && DatePicker.SelectedDate != null)
            {
                string categoryName = CategoryComboBox.SelectedItem.ToString();
                int categoryId = GetCategoryId(categoryName);
                DateTime date = DatePicker.SelectedDate.Value;

                string query = "INSERT INTO [Transactions] (Amount, CategoryId, TransactionDate) VALUES (@Amount, @CategoryId, @TransactionDate)";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.Parameters.AddWithValue("@CategoryId", categoryId);
                        command.Parameters.AddWithValue("@TransactionDate", date);
                        command.ExecuteNonQuery();
                    }
                }

                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.");
            }
        }
    }
}