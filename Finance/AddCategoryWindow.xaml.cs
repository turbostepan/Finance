using LiveCharts.Wpf;
using LiveCharts;
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
    /// Логика взаимодействия для AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        private string connectionString = "Server=510EC16;Database=FINANCE;Trusted_Connection=True;";
        private IncomePage _incomePage;
        private SpendingsPage spendingsPage;

        public AddCategoryWindow(IncomePage incomePage)
        {
            InitializeComponent(); // Важно: этот метод должен быть вызван
            _incomePage = incomePage;
        }

        public AddCategoryWindow(SpendingsPage spendingsPage)
        {
            this.spendingsPage = spendingsPage;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CategoryNameTextBox.Text))
            {
                string query = "INSERT INTO Categories (Name) VALUES (@Name)";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", CategoryNameTextBox.Text);
                        command.ExecuteNonQuery();
                    }
                }

                // Обновляем список категорий в IncomePage
                _incomePage.UpdatePieChart();

                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите название категории.");
            }
        }
    }
}